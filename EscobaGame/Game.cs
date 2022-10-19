namespace Backend;
using static System.Text.Json.JsonSerializer;
/*const string cardsFolder = @"src";
const string imageName = "cards_all.json";
const string deckFolder = @"decks";
var allCardsPath = Path.Combine (cardsFolder,imageName) ;
var jsonString = File.ReadAllText(path);
var cardsDict = new Dictionary<string, Card>();
var cards = Deserialize<List<Card>>(jsonString);*/
public class Game
{
    public List<Card> Deck { get;}
    public Player PlayerOne { get; set;}
    public Player PlayerTwo { get; set;}
    
    public Game(string cardsPath)
    {
        var jsonString = File.ReadAllText(cardsPath);
        Deck = Deserialize<List<Card>>(jsonString) ?? 
                    throw new InvalidOperationException("No se cargaron correctamente las cartas");
        PlayerOne = new Player();
        PlayerTwo = new Player();
        var hola = new Dictionary<string, int>();
        hola.Add("playcard", 2);
        var feo = Serialize(hola);
        var lindo = Deserialize<Dictionary<string,int>>(feo);
        var player = new Player();
        var move = new Move(player,hola);
        var eem = Serialize(move);
        Console.WriteLine(eem);
        Console.WriteLine(hola);
        var lindos = Deserialize<Move>(eem);
        Console.WriteLine(lindos.playerMove["playcard"]);
    }

    
}

[Serializable]
public class Move
{
    public Player player { get; set; }
    public Dictionary<string, int> playerMove { get; set; }

    public Move(Player player, Dictionary<string, int> playerMove)
    {
        this.player = player;
        this.playerMove = playerMove;
    }
}