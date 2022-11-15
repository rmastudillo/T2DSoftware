using static System.Int32;
namespace EscobaServer;
public class EscobaGame
{
    public Board Board { get; set; }
    public GameNotifications GameNotifications { get; }
    public Helper Helper { get; }
    public Player CurrentPlayer { get; set; }
    public bool Playing = true;
    public Messages Messages = new Messages();
    public bool PlayingOnline = false;
    private Player _lastPlayerThatMakeAPlay = new Player(0);

    public EscobaGame(Player player0, Player player1)
    {
        Board = new Board(player0, player1);
        GameNotifications = new GameNotifications(Messages);
        Helper = new Helper(Messages);
        CurrentPlayer = Board.PlayerOne;
    }

    public void NewHand()
    {
        GameNotifications.StartGame();
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
        while (!ThereIsAWinner())
        {
            Board.Deck.ShuffleDeck();
            while (Board.Deck.NumberOfCards() > 0)
            {
                Messages.DealingCards(CurrentPlayer.ToString());
                DealingCardsToPlayers();
                DealingCardsIntoBoard();
                for (var j = 0; j < 6; j++)
                {
                    SwapPlayerTurn();
                    PlayerTurn();
                }
            }
            PlayerGetBoardCards();
            AsignPlayersPoints();
            GameNotifications.ReportEndOfTurn(Board.PlayerOne, Board.PlayerTwo);
            RePopulateDeck();
            SwapPlayerTurn();
        }
        GameNotifications.AnnounceWinner(Board.PlayerOne, Board.PlayerTwo);
    }
    private void RePopulateDeck()
    {
        var playerOneEarnedCards = Board.PlayerOne.ReturnCardsToDeck();
        var playerTwoEarnedCards = Board.PlayerTwo.ReturnCardsToDeck();
        Board.Deck.AddCards(playerOneEarnedCards);
        Board.Deck.AddCards(playerTwoEarnedCards);
    }
    private bool ThereIsAWinner()
    {
        return Board.PlayerOne.GetEarnedPoints() >= 16 || Board.PlayerTwo.GetEarnedPoints() >= 16;
    }
    private void SwapPlayerTurn()
    {

        CurrentPlayer = (CurrentPlayer == Board.PlayerOne) ? Board.PlayerTwo : Board.PlayerOne;
        Messages.ChangeCurrentPlayer(CurrentPlayer.ToString());
    }
    private void PlayerTurn()
    {
        GameNotifications.ShowPlayerOptionToPlay(CurrentPlayer, Board);
        var playerInput = Helper.GetPlayerInput(CurrentPlayer._hand.Count);
        PlayACardFromHand(playerInput);
    }
    private void PlayACardFromHand(int playerInput)
    {
        var cardToPlay = CurrentPlayer.PlayCardFromHand(playerInput);
        var possiblePlays = Helper.CheckPosiblePlays(cardToPlay, Board).ToList();
        switch (possiblePlays.Count)
        {
            case 0:
                Board.AddCard(cardToPlay);
                break;
            case 1:
                MakingAPlay(possiblePlays[0]);
                break;
            case > 1:
                var playerPlay = GameNotifications.ShowPlayerPossiblePlays(possiblePlays, CurrentPlayer.ToString());
                MakingAPlay(possiblePlays[playerPlay]);
                break;
        }
    }


    private void AsignPlayersPoints()
    {
        CheckForSevenGoldCard();
        CheckForTheMostSevensEarned();
        CheckForTheMostNumberOfCardsEarned();
        CheckForTheMostGoldCardsEarned();
    }
    private void CheckForTheMostSevensEarned()
    {
        var playerOneEarnedCardsCount = Board.PlayerOne.GetNumberOfCardWithSevenEarned();
        var playerTwoEarnedCardsCount = Board.PlayerTwo.GetNumberOfCardWithSevenEarned();
        CheckForNumberOfCardsEarned(playerOneEarnedCardsCount, playerTwoEarnedCardsCount);
    }
    private void CheckForTheMostGoldCardsEarned()
    {
        var playerOneEarnedCardsCount = Board.PlayerOne.GetNumberOfGoldCardsEarned();
        var playerTwoEarnedCardsCount = Board.PlayerTwo.GetNumberOfGoldCardsEarned();
        CheckForNumberOfCardsEarned(playerOneEarnedCardsCount, playerTwoEarnedCardsCount);
    }
    private void CheckForTheMostNumberOfCardsEarned()
    {
        var playerOneEarnedCardsCount = Board.PlayerOne.GetNumberOfCardsEarned();
        var playerTwoEarnedCardsCount = Board.PlayerTwo.GetNumberOfCardsEarned();
        CheckForNumberOfCardsEarned(playerOneEarnedCardsCount, playerTwoEarnedCardsCount);
    }
    private void CheckForNumberOfCardsEarned(int playerOneNumberOfCards, int playerTwoNumberOfCards)
    {
        var differenceInNumberOfCards = playerOneNumberOfCards - playerTwoNumberOfCards;
        switch (differenceInNumberOfCards)
        {
            case 0:
                Board.PlayerOne.AddAPoint();
                Board.PlayerTwo.AddAPoint();
                break;
            case > 0:
                Board.PlayerOne.AddAPoint();
                break;
            case < 0:
                Board.PlayerTwo.AddAPoint();
                break;
        }
    }
    private void CheckForSevenGoldCard()
    {
        if (Board.PlayerOne.CheckForTheSevenGoldCard())
        {
            Board.PlayerOne.AddAPoint();
        }
        else if (Board.PlayerTwo.CheckForTheSevenGoldCard())
        {
            Board.PlayerTwo.AddAPoint();
        }
    }
    private void MakingAPlay(List<Card> cards)
    {
        _lastPlayerThatMakeAPlay = CurrentPlayer;
        foreach (var card in cards)
        {
            Board.RemoveCard(card);
        }
        CurrentPlayer.AddEarnedCards(cards);
        Messages.CardWon(CurrentPlayer.ToString(), Helper.ListOfCardsToString(cards));
        if (Board.CardsOnTable.Count == 0) Escoba();
    }
    private void PlayerGetBoardCards()
    {
        var endTurnEarnedCards = new List<Card>(Board.CardsOnTable);
        foreach (var card in endTurnEarnedCards)
        {
            Board.RemoveCard(card);
        }
        _lastPlayerThatMakeAPlay.AddEarnedCards(endTurnEarnedCards);
    }
    private void Escoba()
    {
        CurrentPlayer.AddAPoint();
        Messages.Escoba(CurrentPlayer.ToString());
    }
}