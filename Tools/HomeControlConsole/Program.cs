using msn2.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControlConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UnifiData ud = new UnifiData();
            var c = ud.GetWhoClients();
            Console.WriteLine(c.ToString());
        }
    }
}
