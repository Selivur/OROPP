using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;

if (args.Length != 0 && args[0] == "server")
{
    Console.WriteLine("Listening Mode: ");
    const int MMF_VIEW_SIZE = 1024; 
    BinaryFormatter formatter = new BinaryFormatter();
    byte[] buffer = new byte[MMF_VIEW_SIZE];
    Message message1;
    while (true)
    {
        MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("mFile");
        MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, MMF_VIEW_SIZE);
        if (mmvStream.CanRead)
        {
            mmvStream.Read(buffer, 0, MMF_VIEW_SIZE);
            try
            {
                message1 = (Message)formatter.Deserialize(new MemoryStream(buffer));
                Console.WriteLine($"Received message: {message1.content}");
            }
            catch
            {
                Console.WriteLine("Nothing found");
            }
        }
        Thread.Sleep(1000);
    }
}
else
{
    Console.WriteLine("Preparing...");
    const int MMF_MAX_SIZE = 1024;  
    const int MMF_VIEW_SIZE = 1024; 
    MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("mFile", MMF_MAX_SIZE, MemoryMappedFileAccess.ReadWrite);
    MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, MMF_VIEW_SIZE);
    Console.WriteLine("Starting listening process...");
    System.Diagnostics.Process.Start("OROPP_10.exe", "server");
    Thread.Sleep(2000);
    while (true)
    {     
        Message message1 = new Message();
        Console.Write("Enter message:");
        message1.content = Console.ReadLine();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(mmvStream, message1);
        mmvStream.Seek(0, SeekOrigin.Begin); 
    }
}
[Serializable]
class Message
{
    public string content;
}