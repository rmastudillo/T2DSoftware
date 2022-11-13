using Escoba;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Backend;
const string imageName = "cards.json";
const string cardsFolder = @"src";
var cardsPath = Path.Combine(cardsFolder, imageName);

var newGame = new Game(cardsPath);

TcpClient client = new TcpClient();
client.Connect(IPAddress.Loopback, 8000);

NetworkStream ns = client.GetStream();
StreamWriter writer = new StreamWriter(ns);
StreamReader reader = new StreamReader(ns);
writer.WriteLine("Hola!");
writer.Flush();
string response = reader.ReadLine();
Console.WriteLine("El servidor dice: " + response);
client.Close(); // cerramos la conexi ́on
/*
// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using System.Text;
using System.Net;
using Escoba;

const int PORT_NO = 5000;
const string SERVER_IP = "127.0.0.1";

IPHostEntry host = Dns.GetHostEntry("localhost");
IPAddress ipAddress = host.AddressList[0];
IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);

byte[] bytes = new byte[1024];

using Socket client = new(
ipEndPoint.AddressFamily, 
SocketType.Stream, 
ProtocolType.Tcp);
client.Connect(ipEndPoint);  
while (true)
{
    
    // Send message.
    var msg = Console.ReadLine();
    var message = $"{msg}Hi friends 👋!<|EOM|>";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    _ = await client.SendAsync(messageBytes, SocketFlags.None);
    Console.WriteLine($"Socket client sent message: \"{message}\"");

    // Receive ack.
    var buffer = new byte[1_024];
    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
    var response = Encoding.UTF8.GetString(buffer, 0, received);

    Console.WriteLine(
        $"Socket client received acknowledgment: \"{response}\"");
    /*client.Shutdown(SocketShutdown.Both);
    client.Close();#1#
        // Sample output:
    //     Socket client sent message: "Hi friends 👋!<|EOM|>"
    //     Socket client received acknowledgment: "<|ACK|>"
}
*/
