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
        for (var j = 0; j < 3; j++)
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
        ShowPlayerOptions();
        var playerInput = GetPlayerInput();
    }

    private void ShowPlayerOptions()
    {
        var playerName = CurrentPlayer.ToString();
        var hand = CurrentPlayer.PlayerHandToString();
        var cardsOnTable = Board.CardsOnTableToString();
        Messages.MainTurn(playerName,hand,cardsOnTable);
    }

    private int GetPlayerInput()
    {
        var playerInput = Convert.ToInt32(Console.ReadLine());
        CheckPosiblePlays(playerInput);
        return playerInput;
    }

    private void CheckPosiblePlays(int playerInput)
    {
        Console.WriteLine(Board.CardsOnTable[0].Value);
        TestFucnt();
    }

    private void TestFucnt()
    {
        var listmesa = new List<int> { 7, 11, 7, 2 };
        int[] set = { 7, 8, 1, 7, 6,2 };
        Console.WriteLine("Comienza iteracionxx");
        var algo = GetCombinations(set, 15, new List<int>());
        foreach (var s in algo) {
            Console.WriteLine("Comienza iteracion");
            Console.WriteLine(s);
        }
    }

    public  IEnumerable<int> GetCombinations(int[] set, int sum, IEnumerable<int> values) {
        for (var i = 0; i < set.Length; i++) {
            var left = sum - set[i];
            var vals = new List<int>(set[i]);
            vals.AddRange(values);
            if (left == 0) {
                yield return vals;
            } else {
                var possible = set.Take(i).Where(j => j <= sum).ToArray();
                if (possible.Length <= 0) continue;
                foreach (var s in GetCombinations(possible, left, vals)) {
                    yield return s;
                }
            }
        }
    }
}