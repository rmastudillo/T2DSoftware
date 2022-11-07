namespace EscobaServer;

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