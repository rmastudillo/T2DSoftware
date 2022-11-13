namespace EscobaServer;

public class Player
{
    public List<Card> _hand = new List<Card>();
    private int Id { get; }
    public List<Card> EarnedCards = new List<Card>();
    private int _earnedPoints = 0;
    public Player(int id)
    {
        Id = id;
    }
    public override string ToString()
    {
        return $"Player {Id}";
    }

    public Card PlayCardFromHand(int indexInHand)
    {
        var cardToBePlayed = _hand[indexInHand];
        _hand.Remove(cardToBePlayed);
        return cardToBePlayed;
    }

    public void GetCard(Card card)
    {
        _hand.Add(card);
    }

    public void AddEarnedCards(List<Card> cards)
    {
        EarnedCards.AddRange(cards);
    }

    public int GetNumberOfCardsEarned()
    {
        return EarnedCards.Count;
    }
    public int GetNumberOfCardWithSevenEarned()
    {
        var counterOfSeven = EarnedCards.Count(card => card.Value == 7);
        return counterOfSeven;
    }
    public int GetNumberOfGoldCardsEarned()
    {
        var counterOfGoldCards = EarnedCards.Count(card => card.Pinta == "Oro");
        return counterOfGoldCards;
    }

    public bool CheckForTheSevenGoldCard()
    {
        var counterOfGoldCards = EarnedCards.Where(card => card.Value==7 && card.Pinta=="Oro");
        return counterOfGoldCards.Any();
    }
    public List<Card> ReturnCardsToDeck()
    {
        var cardsToDeck = new List<Card>();
        cardsToDeck.AddRange(EarnedCards);
        EarnedCards.Clear();
        return cardsToDeck;
    }

    public List<string> PlayerHandToString()
    {
        return _hand.Select(card => card.ToString()).ToList();
    }

    public int GetEarnedPoints()
    {
        return _earnedPoints;
    }
    public void AddAPoint()
    {
        _earnedPoints++;
    }
}