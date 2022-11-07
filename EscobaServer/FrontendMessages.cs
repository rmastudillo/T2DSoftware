using System.Diagnostics.CodeAnalysis;

namespace FrontendMessages;
public class Messages
{
    private static void ListMessagePrinter(IEnumerable<string> listMessage, string spliter = "")
    {
        var consolidateMessage = string.Join(spliter, listMessage);
        Console.Write(consolidateMessage);
    }
    public void WelcomeMessage()
    {
        var message = new List<string>(new[]
            {
                "########################################\n",
                "#   Bienvenido al juego de la escoba   #\n",
                "########################################\n"
            });
        ListMessagePrinter(message);
    }

    public void MainMenu()
    {
        var mainMenuMessage = new List<string>(new[]
        {
            "¿Que quieres hacer?\n",
            "[1] Salir\n",
            "[2] Nuevo juego\n"
        });
        ListMessagePrinter(mainMenuMessage);
        InputMessage(2);
    }

    public void ShuffleCards(int playerId)
    {
        var message = new List<string>(new[]
        {
            $"Baraja el maso el jugador {playerId}!\n",
        });
        ListMessagePrinter(message);
    }

    private void InputMessage(int maxOption)
    {
        var mainMenuMessage = new List<string>(new[]
        {
            $"Ingresa un número entre 1 y {maxOption}:"
        });
        ListMessagePrinter(mainMenuMessage);
    }

    private static IEnumerable<string> AddIndexToListString(IReadOnlyCollection<string> listStringWithoutIndex)
    {
        var listCopy = new List<string>(listStringWithoutIndex);
        foreach (var card in listStringWithoutIndex.Select((value, index) => new { index, value }))
        {
            listCopy[card.index] = $"({card.index + 1}) " + card.value;
        }

        return listCopy;
    }
    public void MainTurn(string playerName, List<string> playerHand, List<string> currentBoard)
    {
        var mainMenuMessage = new List<string>(new[]
        {
            $"Juega {playerName}\n",
            "Mesa actual: "
        });
        ListMessagePrinter(mainMenuMessage);
        var boardCopy = AddIndexToListString(currentBoard);
        ListMessagePrinter(boardCopy, ", ");
        Console.Write("\n");
        Console.WriteLine("Mano jugador: ");
        var playerHandCopy = AddIndexToListString(playerHand);
        ListMessagePrinter(playerHandCopy, ", ");
        Console.WriteLine("¿Qué carta quieres bajar?");
        InputMessage(playerHand.Count);
    }
}