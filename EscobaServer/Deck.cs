namespace EscobaServer;

public class Deck
{
    private List<Card> _cards = GenerateDeck();
    private static List<Card> GenerateDeck()
    {
        var newdeck = new List<Card>();
        var validPintas = new List<string>{"Oro","Espada","Bastos","Copa"};
        foreach (var pinta in validPintas)
        {
            for (var value = 1; value <= 10; value++)
            {
                var card = new Card(pinta, value);
                newdeck.Add(card);
            }
        }
        return newdeck;
    }

    public int NumberOfCards()
    {
        return _cards.Count;
    }
    
    public void ShuffleDeck()
    {
        _cards = _cards.OrderBy(carta => GeneradorNumerosAleatorios.Generar()).ToList();
    }

    public Card GetTopCard()
    {
        var card = _cards.Last();
        _cards.Remove(card);
        return card;
    }
    public void AddCards(List<Card> cards)
    {
        _cards.AddRange(cards);
    }
}