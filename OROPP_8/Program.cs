using System.IO.Pipes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
 
NamedPipeClientStream _converterStream;
NamedPipeServerStream _resultStream;
int _dataCount = 50;
var Data = new List<Message>(); 
if (args.Length != 0 && args[0] == "listen")
{
    Data = new List<Message>(); 
    _resultStream = new NamedPipeServerStream("Pipe");
    _resultStream.WaitForConnection();
    Console.WriteLine("ID\tName");
    for (int i = 0; i < _dataCount; i++)
    {
        IFormatter formatter = new BinaryFormatter();
        Message clientReceived = (Message)formatter.Deserialize(_resultStream);
        Thread.Sleep(1);
        Data.Add(clientReceived);
        Console.WriteLine(clientReceived.Content);

    }
    _resultStream.Close();
    Console.ReadLine();
}
else
{
    Console.WriteLine("Starting listening process...");
    System.Diagnostics.Process.Start("OROPP_8.exe", "listen");
    Thread.Sleep(5000);
    Data = new List<Message>();
    for (int i = 0; i < _dataCount; i++)
    {
        Message client = new Message();
        client.Content = "Message num " + i.ToString();
        Data.Add(client);
    }
Console.WriteLine("Sending data...");
    Console.WriteLine("ID\tName");
    _converterStream = new NamedPipeClientStream("Pipe");
    _converterStream.Connect();

    foreach (var client in Data)
    {
        Message newClient = new Message() { Content = client.Content};

        Message messageToSend = newClient;
        IFormatter formatter = new BinaryFormatter();
        Console.WriteLine(newClient.Content);
        formatter.Serialize(_converterStream, messageToSend);
    }
    Thread.Sleep(20000);
    if (_converterStream != null)
        _converterStream.Close();
    Console.ReadKey();
}
[Serializable]
public class Message
{
    public string Content { get; set; }
}











