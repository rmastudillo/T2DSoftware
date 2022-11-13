using System.Runtime.InteropServices;
using System.Windows.Markup;

namespace EscobaServer;


public class EscobaGame
{
    public Board Board { get; set; }
    private bool Playing = true;
    private Messages Messages = new Messages();
    public Player CurrentPlayer { get; set; }

    public EscobaGame(Player player0, Player player1)
    {
        Board = new Board(player0, player1);
        CurrentPlayer = Board.PlayerOne;
    }

    private void StartGame()
    {
        Messages.WelcomeMessage();
        Board.Deck.ShuffleDeck();
    }

    public void NewHand()
    {
        StartGame();
        GameFlow();
    }
    
    private void DealingCardsToPlayers()
    {
        for (var i = 0; i < 3; i++)
        {
            Board.PlayerOne.GetCard(Board.Deck.GetTopCard());
            Board.PlayerTwo.GetCard(Board.Deck.GetTopCard());
        }
    }

    private void DealingCardsIntoBoard()
    {
        for (var i = 0; i < 4; i++)
        {
            Board.AddCard(Board.Deck.GetTopCard());
        } 
    }
    private void GameFlow()
    {
        DealingCardsToPlayers();
        DealingCardsIntoBoard();
        for (var j = 0; j < 2; j++)
        {
            SwapPlayerTurn();
            PlayerTurn();
        }
    }

    private void SwapPlayerTurn()
    {
        CurrentPlayer = (CurrentPlayer == Board.PlayerOne) ? Board.PlayerTwo : Board.PlayerOne;
    }
    private void PlayerTurn()
    {
        ShowPlayerOptionToPlay();
        var playerInput = GetPlayerInput();
        PlayACardFromHand(playerInput);
    }

    private void PlayACardFromHand(int playerInput)
    {
        var cardToPlay = CurrentPlayer.PlayCardFromHand(playerInput);
        var possiblePlays= CheckPosiblePlays(cardToPlay).ToList();
        if (possiblePlays.Count > 1)
        {
            ShowPlayerPossiblePlays(possiblePlays);
        }
        else
        {
            MakingAPlay(possiblePlays[0]);
        }
        
    }

    private void MakingAPlay(List<Card> cards)
    {
        foreach (var card in cards)
        {
            Board.RemoveCard(card);
        }
        CurrentPlayer.AddEarnedCards(cards);
    }

    private List<string> PossiblePlayToString(List<Card> possiblePlay)
    {
        return possiblePlay.Select(card => card.ToString()).ToList();
    }

    private List<List<string>> ListOfPlaysToString(List<List<Card>> possiblePlays)
    {
        return possiblePlays.Select(PossiblePlayToString).ToList();
    }
    private void ShowPlayerPossiblePlays(IEnumerable<List<Card>> possiblePlays)
    {
        var plays = possiblePlays.ToList();
        Messages.ShowPlays(ListOfPlaysToString(plays));
    }

    private void ShowPlayerOptionToPlay()
    {
        var playerName = CurrentPlayer.ToString();
        var hand = CurrentPlayer.PlayerHandToString();
        var cardsOnTable = Board.CardsOnTableToString();
        Messages.MainTurn(playerName,hand,cardsOnTable);
    }

    private int GetPlayerInput()
    {
        var playerInput = Convert.ToInt32(Console.ReadLine());
        return playerInput - 1 ;
    }

    private IEnumerable<List<Card>> CheckPosiblePlays(Card cardToPlay)
    {
        var possibleCardsToCombine = new List<Card>(Board.CardsOnTable);
        possibleCardsToCombine.Insert(0,cardToPlay);
        return GetPlays(possibleCardsToCombine, 15, new List<Card>());
    }
    
    
    public  IEnumerable<List<Card>> GetPlays(List<Card> cardsToCombine, int targetSumOfCards, List<Card> listOfPlays) {
        for (var card = 0; card < cardsToCombine.Count; card++) {
            var currentSum = targetSumOfCards - cardsToCombine[card].Value;
            var validPlays = new List<Card>(){cardsToCombine[card]};
            validPlays.AddRange(listOfPlays);
            if (currentSum == 0) {
                if (validPlays.First() == cardsToCombine.First()) yield return validPlays;
            } else {
                var possiblePlays = new List<Card>(cardsToCombine.Take(card).Where(
                        possibleCard => possibleCard.Value <= targetSumOfCards));
                if (possiblePlays.Count <= 0) continue;
                foreach (var plays in GetPlays(
                             possiblePlays, currentSum, validPlays)) {
                    yield return plays;
                }
            }
        }
    }
}