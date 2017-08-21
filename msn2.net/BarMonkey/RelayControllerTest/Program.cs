using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RelayControllerTest.ServiceReference;

namespace RelayControllerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            RelayControllerClient client = new RelayControllerClient();

            Console.WriteLine("Type a relay to send per line in the format:");
            Console.WriteLine("   <SendGroup> <RelayNumber> <Seconds>");
            Console.WriteLine("");
            Console.WriteLine("Type 'go' to send the command, 'exit' when done.");
            Console.WriteLine("");

            List<BatchItem> currentList = new List<BatchItem>();

            while (true)
            {
                Console.Write("> ");
                string currentBuffer = Console.ReadLine();

                if (string.Equals(currentBuffer, "exit") == true)
                {
                    break;
                }
                else if (string.Equals(currentBuffer, "clear") == true)
                {
                    currentList.Clear();
                }
                else if (string.Equals(currentBuffer, "go") == true || string.Equals(currentBuffer, "") == true)
                {
                    if (currentList.Count > 0)
                    {
                        Console.WriteLine("sending...");
                        try
                        {
                            client.SendBatch(currentList.ToArray());
                            Console.WriteLine("sent.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error sending: " + ex.ToString());
                        }
                        Console.WriteLine("Type 'clear' to clear this drink or enter to repeat.");
                    }
                    else
                    {
                        Console.WriteLine("no items to send!");
                    }
                }
                else
                {
                    string[] commands = currentBuffer.Split(' ');
                    if (commands.Length != 3)
                    {
                        Console.WriteLine("invalid format");
                    }
                    else
                    {
                        try
                        {
                            BatchItem item = new BatchItem();
                            item.Group = int.Parse(commands[0]);
                            item.RelayNumber = int.Parse(commands[1]);
                            item.Seconds = double.Parse(commands[2]);
                            currentList.Add(item);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("Unexpected format: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
