using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace labwork
{
    internal class Program
    {
        public const string PREFIX_QUEUE_NAME = "newQueue";
        public const string PREFIX_QUEUE_ACCESS = ".\\Private$\\";

        static void Main(string[] args)
        {
            if (args.Length != 0 && args[0] == "listen") ListeningMode();
            else ControlMode();
        }

        private static void ControlMode()
        {
            bool listen = false;
            while (true)
            {
                string message, title;
                Console.Write("Enter message: ");
                message = Console.ReadLine();
                Console.Write("Enter title for this message: ");
                title = Console.ReadLine();
                if (message.Length > 0)
                {
                    try
                    {
                        if (!MessageQueue.Exists(string.Format("{0}{1}", PREFIX_QUEUE_ACCESS, PREFIX_QUEUE_NAME)))
                        {
                            MessageQueue.Create(string.Format("{0}{1}", PREFIX_QUEUE_ACCESS, PREFIX_QUEUE_NAME));
                        }
                        MessageQueue queue = new MessageQueue(string.Format("{0}{1}", PREFIX_QUEUE_ACCESS, PREFIX_QUEUE_NAME));
                        queue.Send(message, title);
                        if (!listen)
                        {
                            System.Diagnostics.Process.Start("labwork.exe", "listen");
                            listen = true;
                        }
                    }
                    catch (MessageQueueException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
                else Console.WriteLine("What?");
            }
        }

        private static void ListeningMode()
        {
            Console.WriteLine("Monitoring...");
            while (true)
            {
                MessageQueue queue = new MessageQueue(string.Format("{0}{1}", PREFIX_QUEUE_ACCESS, PREFIX_QUEUE_NAME));
                ((XmlMessageFormatter)queue.Formatter).TargetTypeNames = new string[] { "System.String" };
                System.Messaging.Message Mymessage = queue.Receive();
                Console.WriteLine("Message {0}: {1}", Mymessage.Label, Mymessage.Body.ToString());
            }
        }
    }
}
