using System.Net;

class CommandReceivedEventArgs : EventArgs
{
    public string Command { get; }
    public EndPoint ClientAddress { get; }
    public CommandReceivedEventArgs(string command, EndPoint clientAddress)
    {
        Command = command;
        ClientAddress = clientAddress;
    }
}