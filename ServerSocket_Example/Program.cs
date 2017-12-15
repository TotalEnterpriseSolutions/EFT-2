using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using TES.Integration.IPSockets2;

namespace ServerSocket_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorText = "";

            IPHostEntry localIPHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIPAddress = localIPHostInfo.AddressList[1]; // If you get address incompatible error try using 1 instead of 0

            Base w = new Base(localIPAddress.ToString(), 30503, 0);
            try
            {
                bool gotResponse = false;
                string responseText = "";
                
                if (w.OpenAccept(ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Opened socket", DateTime.Now.ToString()));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}: Failed to open socket: {1}", DateTime.Now.ToString(), errorText));
                }

                gotResponse = w.Read(ref responseText, ref errorText, 30000, true);
                Console.WriteLine(string.Format("Read {0}:{1}", responseText, errorText));              

                if (gotResponse)
                {
                    Console.WriteLine(String.Format("{0}: Got response: {1}", DateTime.Now.ToString(), responseText));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}: Failed to get response", DateTime.Now.ToString()));
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