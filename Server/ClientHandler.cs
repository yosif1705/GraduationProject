using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

class ClientHandler
{
    public event EventHandler<CommandReceivedEventArgs> CommandReceived;

    private TcpClient _client;
    private bool _isRunning;
    private Thread _receiveThread;

    public ClientHandler(TcpClient client)
    {
        _client = client;
        _receiveThread = new Thread(ReceiveCommands);
    }

    public void Start()
    {
        _isRunning = true;
        _receiveThread.Start();
    }

    public void Stop()
    {
        _isRunning = false;
        _client.Close();
    }

    private void ReceiveCommands()
    {
        while (_isRunning)
        {
            try
            {
                var buffer = new byte[1024];
                var bytesReceived = _client.GetStream().Read(buffer, 0, buffer.Length);
                var command = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                RaiseCommandReceivedEvent(command, _client.Client.RemoteEndPoint);
            }
            catch (Exception)
            {
                Stop();
            }
        }
    }

    private void RaiseCommandReceivedEvent(string command, EndPoint clientAddress)
    {
        CommandReceived?.Invoke(this, new CommandReceivedEventArgs(command, clientAddress));
    }
}