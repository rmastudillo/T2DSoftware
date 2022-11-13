using static System.Int32;

namespace EscobaServer;
public class EscobaGame
{
    public Board Board { get; set; }
    private bool Playing = true;
    public Messages Messages = new Messages();
    public Player CurrentPlayer { get; set; }
    private Player LastPlayerThatMakeAPlay { get; set; }

    public EscobaGame(Player player0, Player player1)
    {
        Board = new Board(player0, player1);
        CurrentPlayer = Board.PlayerOne;
    }

    private void StartGame()
    {
        Messages.WelcomeMessage();

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
        while (!ThereIsAWinner())
        {
            Board.Deck.ShuffleDeck();
            while (Board.Deck.NumberOfCards()>0)
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
            ReportEndOfTurn();
            RePopulateDeck();
            SwapPlayerTurn();
        }

        AnnounceWinner();
    }

    private void AnnounceWinner()
    {
        var winnerMessage = new List<string>(){"x Fin de la partida x\n"};
        var playerOnePoints = Board.PlayerOne.GetEarnedPoints();
        var playerTwoPoints = Board.PlayerTwo.GetEarnedPoints();
        var differenceOfPoints = playerOnePoints - playerTwoPoints;
        switch (differenceOfPoints)
        {
            case 0:
                winnerMessage.Add($"Empate con {playerOnePoints} puntos.");
                break;
            case > 0:
                winnerMessage.Add($"Ganó el {Board.PlayerOne.ToString()} con {playerOnePoints} puntos.");
                break;
            case < 0:
                winnerMessage.Add($"Ganó el {Board.PlayerTwo.ToString()} con {playerTwoPoints} puntos.");
                break;
        }
        Messages.EndGameMessage(winnerMessage);
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

    private void ReportEndOfTurn()
    {
        var playerOneEarnedCards = ListOfCardsToString( Board.PlayerOne.EarnedCards);
        var playerTwoEarnedCards = ListOfCardsToString( Board.PlayerTwo.EarnedCards);
        var points = new List<int>() { Board.PlayerOne.GetEarnedPoints(), Board.PlayerTwo.GetEarnedPoints() };
        Messages.EndTurnReport(playerOneEarnedCards, playerTwoEarnedCards, points);
    }

    private void SwapPlayerTurn()
    {
        CurrentPlayer = (CurrentPlayer == Board.PlayerOne) ? Board.PlayerTwo : Board.PlayerOne;
    }
    private void PlayerTurn()
    {
        ShowPlayerOptionToPlay();
        var playerInput = GetPlayerInput(CurrentPlayer._hand.Count);
        PlayACardFromHand(playerInput);
    }

    private void PlayACardFromHand(int playerInput)
    {
        var cardToPlay = CurrentPlayer.PlayCardFromHand(playerInput);
        var possiblePlays= CheckPosiblePlays(cardToPlay).ToList();
        switch (possiblePlays.Count)
        {
            case 0:
                Board.AddCard(cardToPlay) ;
                break;
            case 1:
                MakingAPlay(possiblePlays[0]);
                break;
            case > 1:
                ShowPlayerPossiblePlays(possiblePlays);
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
        CheckForNumberOfCardsEarned(playerOneEarnedCardsCount,playerTwoEarnedCardsCount);
    }
    private void CheckForTheMostGoldCardsEarned()
    {
        var playerOneEarnedCardsCount = Board.PlayerOne.GetNumberOfGoldCardsEarned();
        var playerTwoEarnedCardsCount = Board.PlayerTwo.GetNumberOfGoldCardsEarned();
        CheckForNumberOfCardsEarned(playerOneEarnedCardsCount,playerTwoEarnedCardsCount);
    }

    private void CheckForTheMostNumberOfCardsEarned()
    {
        var playerOneEarnedCardsCount = Board.PlayerOne.GetNumberOfCardsEarned();
        var playerTwoEarnedCardsCount = Board.PlayerTwo.GetNumberOfCardsEarned();
        CheckForNumberOfCardsEarned(playerOneEarnedCardsCount,playerTwoEarnedCardsCount);
    }
    private void CheckForNumberOfCardsEarned(int playerOneNumberOfCards,int playerTwoNumberOfCards)
    {
        var differenceInNumberOfCards = playerOneNumberOfCards -playerTwoNumberOfCards;
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
        else if(Board.PlayerTwo.CheckForTheSevenGoldCard())
        {
            Board.PlayerTwo.AddAPoint();
        }  
    }
    private void MakingAPlay(List<Card> cards)
    {
        LastPlayerThatMakeAPlay = CurrentPlayer;
        foreach (var card in cards)
        {
            Board.RemoveCard(card);
        }
        CurrentPlayer.AddEarnedCards(cards);
        Messages.CardWon(CurrentPlayer.ToString(),ListOfCardsToString(cards));
        if (Board.CardsOnTable.Count == 0)Escoba();
    }

    private void PlayerGetBoardCards()
    {
        var endTurnEarnedCards = new List<Card>(Board.CardsOnTable);
        foreach (var card in endTurnEarnedCards)
        {
            Board.RemoveCard(card);
            
        }
        LastPlayerThatMakeAPlay.AddEarnedCards(endTurnEarnedCards);
    }

    private void Escoba()
    {
        CurrentPlayer.AddAPoint();
        Messages.Escoba(CurrentPlayer.ToString());
    }

    private List<string> ListOfCardsToString(List<Card> possiblePlay)
    {
        return possiblePlay.Select(card => card.ToString()).ToList();
    }

    private List<List<string>> ListOfPlaysToString(List<List<Card>> possiblePlays)
    {
        return possiblePlays.Select(ListOfCardsToString).ToList();
    }
    private void ShowPlayerPossiblePlays(List<List<Card>> possiblePlays)
    {
        LastPlayerThatMakeAPlay = CurrentPlayer;
        Messages.ShowPlays(ListOfPlaysToString(possiblePlays));
        var playerInput = GetPlayerInput(ListOfPlaysToString(possiblePlays).Count);
        MakingAPlay(possiblePlays[playerInput]);
    }

    private void ShowPlayerOptionToPlay()
    {
        var playerName = CurrentPlayer.ToString();
        var hand = CurrentPlayer.PlayerHandToString();
        var cardsOnTable = Board.CardsOnTableToString();
        Messages.MainTurn(playerName,hand,cardsOnTable);
    }

    private int GetPlayerInput(int maxInputValue)
    {
        var inputIsInt = TryParse(Console.ReadLine(), out var playerValidInput);
        while (!inputIsInt || ( playerValidInput<1) || (playerValidInput>maxInputValue))
        {
            Messages.InvalidInput();
            inputIsInt = TryParse(Console.ReadLine(), out  playerValidInput);
        }
        return playerValidInput - 1 ;
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