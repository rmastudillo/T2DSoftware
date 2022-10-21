namespace Backend;

public class Player
{
    private List<Card> Hand { get; set;}
    public string Name { get; set; }

    public Player(string playerName)
    {
        this.Name = playerName;
    }
}