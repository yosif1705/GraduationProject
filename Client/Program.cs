using Client;
using System.IO;

class Program
{
    private static TcpClient _client;

    static void Main()
    {
        Console.WriteLine("CLIENT");
        _client = new TcpClient("127.0.0.1", 64532);
        new Thread(SendCommands).Start();
    }

    private static async void SendCommands()
    {
        while (true)
        {
            var input = Console.ReadLine().Split(" ");
            Command.TryParse(input[0],out Command command);
            string[] args = input.Skip(1).ToArray();
            await _client.SendCommand(command, args);
        }
    }
}