using System.Net.Security;

namespace Escoba;
using System.Net.Sockets;
using System;
using System.Net;


public class Client
{
    private StreamWriter Writer { get; set; }
    private StreamReader Reader { get; set; }
    private TcpClient _clientConnection = new TcpClient();
    private bool _writeMode = false;
    private bool _playing = true;
    
    public Client()
    {
        _clientConnection.Connect(IPAddress.Loopback, 8000);
        Console.Write($"Conectado!\n");
        var networkStream = _clientConnection.GetStream();
        Writer = new StreamWriter(networkStream);
        Reader = new StreamReader(networkStream);
    }

    private void ShowServerMessages(TextReader reader)
    {
        var response = reader.ReadLine();
        while (response!="" && !response.Contains("Code:"))
        {
            Console.WriteLine(response);
            response = reader.ReadLine();
        }
        switch (response)
        {
            case "Code:input":
                _writeMode = true;
                break;
            case "Code:end":
                _playing = false;
                break;
            case "Code:correct":
                Console.WriteLine("El mensaje fue recibido correctamente");
                _writeMode = false;
                break;
        }
   
    }

    private void SendMessage(string message)
    {
  
        Console.WriteLine($"Enviando{message}");
        Writer.WriteLineAsync(message);
        Writer.Flush();
        Console.WriteLine($"-{message}- Enviado");
        //ShowServerMessages(Reader);
    }
    public void StartConnection()
    {
        Console.WriteLine("Conexión establecida correctamente\nIniciando el juego\n\n");
        Writer.WriteLine("Code:sucess");
        Writer.Flush();
        while (_playing)
        {
            ShowServerMessages(Reader);
            if (_writeMode)
            {
                var msg = Console.ReadLine();
                if (!string.IsNullOrEmpty(msg))
                {
                    SendMessage($"{msg}");
                    _writeMode = false;
                }
            }
        }
        Writer.WriteLine("Salir");
        Writer.Flush();
        _clientConnection.Close();
    }
}