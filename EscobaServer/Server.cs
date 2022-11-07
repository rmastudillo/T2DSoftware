namespace EscobaServer;

public class EscobaGame
{
    
}

public class Player
{
    
}

public class Register
{
    
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
    
    public void ShuffleDeck()
    {
        _cards = _cards.OrderBy(carta => GeneradorNumerosAleatorios.Generar()).ToList();
    }
}

public class Card
{
    public string Pinta { get; set; }
    public int Value { get; set; }
    public string Name { get; set; }

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
}