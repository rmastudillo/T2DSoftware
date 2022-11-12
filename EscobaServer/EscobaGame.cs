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
        int[] set = { 7, 8, 1, 7,6,2 };
        Console.WriteLine("Comienza iteracionxx");
        var cartas = Board.CardsOnTable;
        cartas.Insert(0, CurrentPlayer._hand[0]);
        var algo = GetCombinations(Board.CardsOnTable, 15, new List<Card>{});
        foreach (var s in algo) {
            Console.WriteLine("Comienza iteracion");
            Console.WriteLine(s);
            foreach (var VARIABLE in s)
            {
                Console.Write(VARIABLE);
            }
        }
    }

    public  IEnumerable<List<Card>> GetCombinations(List<Card> set, int sum, List<Card> values) {
        for (var i = 0; i < set.Count; i++) {
            var left = sum - set[i].Value;
            var vals = new List<Card>(){set[i]};
            vals.AddRange(values);
            if (left == 0) {
                yield return vals;
            } else {
                var possible = set.Take(i).Where(j => j.Value <= sum).ToArray();
                var asd = new List<Card>(possible);
                if (possible.Length <= 0) continue;
                foreach (var s in GetCombinations(asd, left, vals)) {
                    yield return s;
                }
            }
        }
    }
}