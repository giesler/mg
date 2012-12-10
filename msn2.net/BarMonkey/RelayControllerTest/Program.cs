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

            BatchItem[] batch = new BatchItem[] {  
                                new BatchItem { Group = 1, RelayNumber = 3, Seconds = 2},
                                new BatchItem { Group = 1, RelayNumber = 4, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 5, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 8, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 10, Seconds = 4},
                                new BatchItem { Group = 2, RelayNumber = 12, Seconds = 5},
                                new BatchItem { Group = 2, RelayNumber = 14, Seconds = 8},
                                new BatchItem { Group = 2, RelayNumber = 16, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 18, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 20, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 22, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 24, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 26, Seconds = 5},
                                new BatchItem { Group = 2, RelayNumber = 28, Seconds = 4},
                                new BatchItem { Group = 2, RelayNumber = 30, Seconds = 8},
                                new BatchItem { Group = 2, RelayNumber = 17, Seconds = 2},
                                new BatchItem { Group = 2, RelayNumber = 15, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 13, Seconds = 4},
                                new BatchItem { Group = 2, RelayNumber = 25, Seconds = 8},
                                new BatchItem { Group = 2, RelayNumber = 27, Seconds = 4},
                                new BatchItem { Group = 2, RelayNumber = 29, Seconds = 3},
                                new BatchItem { Group = 2, RelayNumber = 9, Seconds = 6},
                                new BatchItem { Group = 2, RelayNumber = 11, Seconds = 4},
                                new BatchItem { Group = 2, RelayNumber = 31, Seconds = 7}
                                };

            client.SendBatch(batch);

            Console.Read();
        }


    }
}
