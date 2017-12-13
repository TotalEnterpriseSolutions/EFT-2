using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TES.Integration.IPSockets;

namespace Test_Sandbox_4
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorText = "";
            string request = "-tr10 -am102 -rf99999991 -x";

            //Base w = new Base("192.168.1.212", 30503, 0);
            try
            {
                //if (w.open(ref errorText))
                ////{
                ////    Console.WriteLine(String.Format("{0}: Opened receipt port", DateTime.Now.ToString()));
                ////}
                ////else
                ////{
                ////    Console.WriteLine(String.Format("{0}: Failed to open to receipt port {1}", DateTime.Now.ToString(), errorText));
                ////}

                Base w = new Base("192.168.1.212", 30503, 0);
                if (w.connect(ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Connected to receipt port", DateTime.Now.ToString()));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(), errorText));
                }

                if (w.Write(request, ref errorText))
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
