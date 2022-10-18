// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
/*const int PORT_NO = 5000;
const string SERVER_IP = "127.0.0.1";
IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(SERVER_IP);
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, PORT_NO);
using Socket listener = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);*/

IPHostEntry host = Dns.GetHostEntry("localhost");
IPAddress ipAddress = host.AddressList[0];
IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);

// Create a Socket that will use Tcp protocol
using Socket listener = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

listener.Bind(ipEndPoint);
listener.Listen(100);
var sockets = new List<Socket>();

while (true)
{
    try
    {
        var handler = await listener.AcceptAsync();
        sockets.Add(handler);
        var buffer = new byte[1_024];
        var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        var eom = "<|EOM|>";
        if (response.IndexOf(eom) > -1 /* is end of message */)
        {
            Console.WriteLine(
                $"Socket server received message: \"{response.Replace(eom, "")}\"");

            var ackMessage = $"Socket server received message: \"{response.Replace(eom, "")} <|ACK|>";
            var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
            await handler.SendAsync(echoBytes, 0);
                Console.WriteLine(
                    $"Socket server sent acknowledgment: \"{ackMessage}\"");
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            /*break;*/
        }
    }
    catch (ArgumentNullException ane)
    {
        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
    }
    catch (SocketException se)
    {
        Console.WriteLine("SocketException : {0}", se.ToString());
    }
    catch (Exception e)
    {
        Console.WriteLine("Unexpected exception : {0}", e.ToString());
    }

    // Receive message.
    // Sample output:
    //    Socket server received message: "Hi friends 👋!"
    //    Socket server sent acknowledgment: "<|ACK|>"
}