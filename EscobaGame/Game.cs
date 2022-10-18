namespace Escoba;
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
    }

    
}