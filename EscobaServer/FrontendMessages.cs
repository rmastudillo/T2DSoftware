namespace EscobaServer;
public class Messages
{
    public List<Client> Clients = new();
    public void SetClients(Client firstClient,Client secondClient)
    {
        Clients.Add(firstClient);
        Clients.Add(secondClient);
    }


    public void SendMessage( StreamWriter writer,string message)
    {
        writer.WriteLine(message);
        writer.Flush();
    }
    private  void ListMessagePrinter(IEnumerable<string> listMessage, string spliter="")
    {
        var consolidateMessage = string.Join(spliter, listMessage);
        Console.Write(consolidateMessage);
    }

    private void SendSpecificMessageToClient(IEnumerable<string> listMessage, string clientName, string spliter = "")
    {
        var waitingMessage = $"Espera a que el jugador {clientName} haga su jugada\n";
        var consolidateMessage = string.Join(spliter, listMessage);
        if (Clients.Count < 2) return;
        switch (clientName)
        {
            case "Jugador 0":
                SendMessage(Clients[0].ClientWriter, consolidateMessage);
                SendMessage(Clients[1].ClientWriter, waitingMessage);
                break;
            case "Jugador 1":
                SendMessage(Clients[0].ClientWriter, waitingMessage);
                SendMessage(Clients[1].ClientWriter, consolidateMessage);
                break;
            case "Ambos":
            {
                SendMessage(Clients[0].ClientWriter, consolidateMessage);
                SendMessage(Clients[1].ClientWriter, consolidateMessage);
                break;
            }
        }
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
        SendSpecificMessageToClient(message, "Ambos");
    }
    public void EndGameMessage(List<string> message)
    {
        ListMessagePrinter(message);
        SendSpecificMessageToClient(message, "Ambos");
    }

    public void MainMenu()
    {
        var mainMenuMessage = new List<string>(new[]
        {
            "¿Que quieres hacer?\n",
            "[1] Jugar en local\n",
            "[2] Jugar mediante sockets\n"
        });
        ListMessagePrinter(mainMenuMessage);
        InputMessage(2);
    }
    

    public void DealingCards(string player)
    {
        var message = new List<string>(new[]
        {
            $"\n>>> Reparte el {player}! <<<\n",
        });
        ListMessagePrinter(message);
        SendSpecificMessageToClient(message, "Ambos");
    }

    public void Escoba(string playeName)
    {
        var message = new List<string>(new[]
        {
            $"ESCOBA ! **************************************************  JUGADOR {playeName}\n",
        });
        ListMessagePrinter(message);
        SendSpecificMessageToClient(message, "Ambos");
    }

    public void EndTurnReport(List<string> cardsWonPlayer1, List<string> cardsWonPlayer2, List<int> points)
    {
        
        var cards1 = string.Join(", ", cardsWonPlayer1);
        var cards2 = string.Join(", ", cardsWonPlayer2);
        var message = new List<string>(new[]
        {
            "-----------------------------------\n",
            "Cartas ganadas en esta ronda\n",
            $"    Jugador 0: {cards1}\n",
            $"    Jugador 1: {cards2}\n",
            "-----------------------------------\n",
            "Total puntos ganados\n",
            $"    Jugador 0: {points[0]}\n",
            $"    Jugador 1: {points[1]}\n",
            "-----------------------------------\n"
        });
        ListMessagePrinter(message);
        SendSpecificMessageToClient(message, "Ambos");
    }

    private void AddOptionToPlay(int numerOfOption,List<string> play)
    {
        play[0] = $"({numerOfOption+1}) " + play[0];
        play[^1] += "\n";
    }
    public void ShowPlays(List<List<string>> listOfPlays)
    {

        for (var numberOfPlay = 0; numberOfPlay < listOfPlays.Count; numberOfPlay++)
        {
            AddOptionToPlay(numberOfPlay,listOfPlays[numberOfPlay]);
            ListMessagePrinter(listOfPlays[numberOfPlay],", ");
        }
        Console.WriteLine("¿Que jugada desea usar?");
        InputMessage(listOfPlays.Count);
    }
    public void CardWon(string playername, List<string> cardsWon)
    {
        var cards = string.Join(", ", cardsWon);
        var message = new List<string>(new[]
        {
            $"{playername} se lleva las siguientes cartas: {cards}\n"
        });
        ListMessagePrinter(message);
        SendSpecificMessageToClient(message, "Ambos");
    }

    private void InputMessage(int maxOption)
    {
        var mainMenuMessage = new List<string>(new[]
        {
            $"Ingresa un número entre 1 y {maxOption}:\n"
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
            $"\n* Juega {playerName} *\n",
            "Mesa actual: "
        });
        ListMessagePrinter(mainMenuMessage);
        SendSpecificMessageToClient(mainMenuMessage, "Ambos");
        var boardCopy = AddIndexToListString(currentBoard);
        var listMessage = boardCopy.ToList();
        ListMessagePrinter(listMessage, ", ");
        SendSpecificMessageToClient(listMessage, "Ambos",", ");
        Console.Write("\nMano jugador: ");
        var playerHandCopy = AddIndexToListString(playerHand);
        ListMessagePrinter(playerHandCopy, ", ");
        Console.WriteLine("\n¿Qué carta quieres bajar?");
        InputMessage(playerHand.Count);
    }

    public void InvalidInput()
    {
        ListMessagePrinter(new []{"Error: Input inválido, porfavor selecciona una opción válida:\n"});
    }
}