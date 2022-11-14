using static System.Int32;
namespace EscobaServer;
public class Helper
{
    public Messages Messages = new Messages();

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
    public int GetPlayerInput(int maxInputValue)
    {
        var inputIsInt = TryParse(Console.ReadLine(), out var playerValidInput);
        while (!inputIsInt || (playerValidInput < 1) || (playerValidInput > maxInputValue))
        {
            Messages.InvalidInput();
            inputIsInt = TryParse(Console.ReadLine(), out playerValidInput);
        }
        return playerValidInput - 1;
    }
}