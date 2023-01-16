using System.Net.Sockets;
using System.Net;
using Client;

namespace Server
{
    class Server
    {
        private TcpListener _listener;
        private bool _isRunning;
        private List<ClientHandler> _clients;
        private Database _database;
        private Thread _connectionThread;
        private DbActions _dbActions;

        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _clients = new List<ClientHandler>();
            _database = new Database("localhost", "music_streaming_app", "serverApp", "ParolaServer");
            _connectionThread = new Thread(ListenForClients);
            _dbActions = new DbActions(_database);
        }

        public void Start()
        {
            _isRunning = true;
            _listener.Start();
            _database.Open();
            _connectionThread.Start();
        }

        public void Close()
        {
            _isRunning = false;
            _listener.Stop();
            _database.Close();
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                var client = _listener.AcceptTcpClient();
                var clientHandler = new ClientHandler(client);
                clientHandler.CommandReceived += OnCommandReceived;
                clientHandler.Start();
                _clients.Add(clientHandler);
            }
        }

        public async Task<CommandStatus> HandleCommand(string commandString)
        {
            CommandStatus status;
            var parts = commandString.Split(' ');
            if (!Enum.TryParse(parts[0], out UserCommand command))
            {
                status = CommandStatus.FAILED;
            }
            var args = parts.Skip(1).ToArray();
            
            switch (command)
            {
                case UserCommand.LOGIN:
                    if (args.Length != 2)
                    {
                        status = CommandStatus.FAILED;
                        break;
                    } 
                    try
                    {
                        var result = await HandleLogin(args);
                        status = result ? CommandStatus.SUCCESSFUL : CommandStatus.FAILED;
                        break;
                    }
                    catch (Exception)
                    {
                        status = CommandStatus.FAILED;
                        break;
                    }
                case UserCommand.REGISTER_USER:
                    if (args.Length < 3)
                    {
                        status = CommandStatus.FAILED;
                        break;
                    }
                    try
                    {
                        var registered = await HandleRegistry(args);
                        status = registered ? CommandStatus.SUCCESSFUL : CommandStatus.FAILED;
                        break;
                    }
                    catch (Exception)
                    {
                        status = CommandStatus.FAILED;
                        break;
                    }
                default:
                    status = CommandStatus.FAILED;
                    break;
            }
            return status;
        }
        private async Task<bool> HandleLogin(string[] args)
        {
            return await _dbActions.Login(args[0], args[1]);
        }
        private async Task<bool> HandleRegistry(string[] args)
        {
            return await _dbActions.Register(args[0], args[1], args[2]);
        }

        private async void OnCommandReceived(object sender, CommandReceivedEventArgs e)
        {
            var command = e.Command;
            var clientAddress = e.ClientAddress;
            Console.WriteLine("Client {0} command was {1}", clientAddress, await HandleCommand(command));
        }
    }
}