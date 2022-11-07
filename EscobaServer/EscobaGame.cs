namespace EscobaServer;

public class EscobaGame
{
    public Player Player0 { set; get; }
    public Player Player1 { set; get; }

    public EscobaGame()
    {
        Console.WriteLine("Saludos");
    }
}

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

    private int NumberOfCards()
    {
        return _cards.Count;
    }
    
    public void ShuffleDeck()
    {
        _cards = _cards.OrderBy(carta => GeneradorNumerosAleatorios.Generar()).ToList();
    }

    public Card DrawCard()
    {
        var card = _cards.Last();
        _cards.Remove(card);
        return card;
    }
    public void AddCards(List<Card> cards)
    {
        _cards.AddRange(_cards);
    }
}

public class Card
{
    public string Pinta { get; set; }
    public int Value { get; set; }
    private string Name { get; set; }

    public Card(string pinta, int value)
    {
        Pinta = pinta;
        Value = value;
        Name = value switch
        {
            8 => $"Jota_{pinta}",
            9 => $"Caballo_{pinta}",
            10 => $"Rey_{pinta}",
            _ => $"{value}_{pinta}"
        };
    }
    public override string ToString()
    {
        return Name;
    }
}