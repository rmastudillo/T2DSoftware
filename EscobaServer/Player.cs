namespace EscobaServer;

public class Player
{
    public List<Card> _hand = new List<Card>();
    private int Id { get; }
    public List<Card> EarnedCards = new List<Card>();
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
}