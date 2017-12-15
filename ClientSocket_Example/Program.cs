using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using TES.Integration.IPSockets;

namespace ClientSocket_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorText = "";
            string request = "-tr10 -am102 -rf99999991 -x";

            IPHostEntry localIPHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIPAddress = localIPHostInfo.AddressList[1]; // If you get address incompatible error try using 1 instead of 0

            Base socketHandler = new Base(localIPAddress.ToString(), 30503, 0);
            try
            {
                if (socketHandler.connect(ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Connected to socket", DateTime.Now.ToString()));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(), errorText));
                }

                if (socketHandler.Write(request, ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Sending transaction: {1}", DateTime.Now.ToString(), request));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(), errorText));
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
