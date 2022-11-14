namespace EscobaServer;
public class GameNotifications
{
    public Messages Messages { get; }
    public Helper Helper { get; }

    public void StartGame()
    {
        Messages.WelcomeMessage();
    }

    public GameNotifications(Messages messages)
    {
        Messages = messages;
        Helper = new Helper(messages);
    }
    public void AnnounceWinner(Player playerOne, Player playerTwo)
    {
        var winnerMessage = new List<string>() { "x Fin de la partida x\n" };
        var playerOnePoints = playerOne.GetEarnedPoints();
        var playerTwoPoints = playerTwo.GetEarnedPoints();
        var differenceOfPoints = playerOnePoints - playerTwoPoints;
        switch (differenceOfPoints)
        {
            case 0:
                winnerMessage.Add($"Empate con {playerOnePoints} puntos.");
                break;
            case > 0:
                winnerMessage.Add($"Ganó el {playerOne.ToString()} con {playerOnePoints} puntos.");
                break;
            case < 0:
                winnerMessage.Add($"Ganó el {playerTwo.ToString()} con {playerTwoPoints} puntos.");
                break;
        }
        Messages.EndGameMessage(winnerMessage);
    }
    public void ReportEndOfTurn(Player playerOne, Player playerTwo)
    {
        var playerOneEarnedCards = Helper.ListOfCardsToString(playerOne.EarnedCards);
        var playerTwoEarnedCards = Helper.ListOfCardsToString(playerTwo.EarnedCards);
        var points = new List<int>() { playerOne.GetEarnedPoints(), playerTwo.GetEarnedPoints() };
        Messages.EndTurnReport(playerOneEarnedCards, playerTwoEarnedCards, points);
    }
    public int ShowPlayerPossiblePlays(List<List<Card>> possiblePlays, string playerName)
    {
        Messages.ShowPlays(Helper.ListOfPlaysToString(possiblePlays));
        return Helper.GetPlayerInput(Helper.ListOfPlaysToString(possiblePlays).Count);
    }
    public void ShowPlayerOptionToPlay(Player currentPlayer, Board board)
    {
        var playerName = currentPlayer.ToString();
        var hand = currentPlayer.PlayerHandToString();
        var cardsOnTable = board.CardsOnTableToString();
        Messages.MainTurn(playerName, hand, cardsOnTable);
    }
}