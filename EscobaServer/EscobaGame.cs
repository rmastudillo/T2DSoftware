using System.Runtime.InteropServices;

namespace EscobaServer;


public class EscobaGame
{
    public Board Board { get; set; }
    private bool Playing = true;
    public Messages Messages = new Messages();
    public Player CurrentPlayer { get; set; }

    public EscobaGame(Player player0, Player player1)
    {
        Board = new Board(player0, player1);
        CurrentPlayer = Board.PlayerOne;
    }

    private void StartGame()
    {
        Messages.WelcomeMessage();
    }

    public void NewGame()
    {
        StartGame();
        GameFlow();
    }
    private void GameFlow()
    {
        for (int j = 0; j < 3; j++)
        {
            PlayerTurn(CurrentPlayer);
        }
    }

    private void PlayerTurn(Player player)
    {
        var playerName = player.ToString();
        var hand = player.PlayerHandToString();
        var cardsOnTable = Board.CardsOnTableToString();
        Messages.MainTurn(playerName,hand,cardsOnTable);
    }
}