namespace EscobaServer;

public class Board
{
    public Player PlayerOne { set; get; }
    public Player PlayerTwo { set; get; }
    public List<Card> CardsOnTable = new List<Card>();
    public Deck Deck { set; get; }

    public Board(Player playerOne, Player playerTwo)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        Deck = new Deck();
    }
    
    public List<string> CardsOnTableToString()
    {
        return CardsOnTable.Select(card => card.ToString()).ToList();
    }

    public void AddCard(Card card)
    {
        CardsOnTable.Add(card);
    }
    public void RemoveCard(Card card)
    {
        CardsOnTable.Remove(card);
    }
}