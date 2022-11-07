namespace EscobaServer;

public class Player
{
    private List<Card> _hand = new List<Card>();
    private string Name { get; }
    public List<Card> EarnedCards = new List<Card>();
    public Player(string name)
    {
        Name = name;
    }
    public override string ToString()
    {
        return Name;
    }

    public Card PlayCardFromHand(int indexInHand)
    {
        var cardToBePlayed = _hand[indexInHand];
        _hand.Remove(cardToBePlayed);
        return cardToBePlayed;
    }

    public void DrawCard(Card card)
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
}