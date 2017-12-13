using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TES.Integration.IPSockets;

namespace Test_Sandbox_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorText = "";

            Base r = new Base("192.168.1.212", 30503, 0);
            try
            {
                if (r.open(ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Opened receipt port", DateTime.Now.ToString()));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}: Failed to open to receipt port {1}", DateTime.Now.ToString(), errorText));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(String.Format("{0}: End of Transaction.", DateTime.Now.ToString()));
            Console.Read();
        }
    }
}
