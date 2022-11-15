using System.Net.Sockets;
using static System.Int32;
namespace EscobaServer;
public class Helper
{
    public Messages Messages { get; }
    public bool PlayingOnline { get; set; }

    public Helper(Messages messages)
    {
        Messages = messages;
    }

    public void OnlineHelper()
    {
        PlayingOnline = true;
    }
    public List<List<string>> ListOfPlaysToString(List<List<Card>> possiblePlays)
    {
        return possiblePlays.Select(ListOfCardsToString).ToList();
    }

    public List<string> ListOfCardsToString(List<Card> possiblePlay)
    {
        return possiblePlay.Select(card => card.ToString()).ToList();
    }

    public IEnumerable<List<Card>> CheckPosiblePlays(Card cardToPlay, Board Board)
    {
        var possibleCardsToCombine = new List<Card>(Board.CardsOnTable);
        possibleCardsToCombine.Insert(0, cardToPlay);
        return GetPlays(cardToPlay, possibleCardsToCombine, 15, new List<Card>());
    }

    private IEnumerable<List<Card>> GetPlays(
        Card playerCard, List<Card> cardsToCombine, int targetSumOfCards, List<Card> listOfPlays)
    {
        for (var card = 0; card < cardsToCombine.Count; card++)
        {
            var currentSum = targetSumOfCards - cardsToCombine[card].Value;
            var validPlays = new List<Card>() { cardsToCombine[card] };
            validPlays.AddRange(listOfPlays);
            if (currentSum == 0)
            {
                if (validPlays.First() == playerCard) yield return validPlays;
            }
            else
            {
                var possiblePlays = new List<Card>(cardsToCombine.Take(card).Where(
                        possibleCard => possibleCard.Value <= targetSumOfCards));
                if (possiblePlays.Count <= 0) continue;
                foreach (var plays in GetPlays(playerCard,
                             possiblePlays, currentSum, validPlays))
                {
                    yield return plays;
                }
            }
        }
    }

    private int GetPlayerInputOnline(int maxInputValue)
    {
        var inputCodeString = new List<string>(){"Code:input"};
        var inputIsValid = false;
        var clientMesage = "";
        while (!inputIsValid)
        {
            Messages.SendSpecificMessageToClient(inputCodeString,Messages.CurrentPlayerName);
            clientMesage = Messages.GetClientMsg(Messages.Clients[Messages.CurrentPlayerName]);
            Console.WriteLine(clientMesage);
            inputIsValid = InputIsValid( clientMesage,  maxInputValue);
            if (!inputIsValid){ Messages.InvalidInput();}

        }
        TryParse(clientMesage, out  var playerValidInput);
        return playerValidInput;
    }

    private bool InputIsValid(string playerInput, int maxInputValue)
    {
        var inputIsInt = TryParse(playerInput, out var playerValidInput);
        return inputIsInt && (playerValidInput >= 1) && (playerValidInput <= maxInputValue);
    }
    public int GetPlayerInput(int maxInputValue)
    {
        if (PlayingOnline)
        {
            var playerValidInputOnline = GetPlayerInputOnline(maxInputValue);
            return playerValidInputOnline - 1;
        }
        var playerInput = Console.ReadLine();
        var isValidInput = InputIsValid(playerInput,maxInputValue);;
        while (!isValidInput)
        {
            Messages.InvalidInput();
            playerInput = Console.ReadLine();
            isValidInput = InputIsValid(playerInput,maxInputValue);
        }
        TryParse(playerInput, out  var playerValidInput);
        return playerValidInput - 1;
    }
}