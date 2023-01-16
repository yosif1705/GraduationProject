using Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class TcpClient
{
    private readonly Socket _socket;

    public TcpClient(string serverIP, int port)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var endpoint = new IPEndPoint(IPAddress.Parse(serverIP), port);
        _socket.Connect(endpoint);
    }

    public void Close()
    {
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
    }

    public async Task SendCommand(Command command, params string[] args)
    {
        // Construct the command string to send to the server
        var commandString = $"{command}";
        if (args.Length > 0)
        {
            commandString += " " + string.Join(" ", args);
        }

        // Send the command to the server
        var bytesToSend = Encoding.ASCII.GetBytes(commandString);
        await _socket.SendAsync(new ArraySegment<byte>(bytesToSend), SocketFlags.None);
    }

    public async Task<string> ReceiveResponse()
    {
        var buffer = new byte[1024];
        var bytesReceived = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
        return Encoding.ASCII.GetString(buffer, 0, bytesReceived);
    }
}