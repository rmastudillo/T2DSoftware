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