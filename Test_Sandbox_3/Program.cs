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

            Base w = new Base("192.168.1.212", 30503, 0);
            try
            {
                //if (w.open(ref errorText))
                //{
                //    Console.WriteLine(String.Format("{0}: Opened receipt port", DateTime.Now.ToString()));
                //}
                //else
                //{
                //    Console.WriteLine(String.Format("{0}: Failed to open to receipt port {1}", DateTime.Now.ToString(), errorText));
                //}

                //Base w = new Base("192.168.1.212", 30503, 0);
                //if (w.connect(ref errorText))
                //{
                //    Console.WriteLine(String.Format("{0}: Connected to receipt port", DateTime.Now.ToString()));
                //}
                //else
                //{
                //    Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(), errorText));
                //}

                bool gotReceipt = false;
                //int counter = 1;
                string receiptText = "";
                //TESJH Added Accept
                if (w.openListen(ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Opened Listen receipt port", DateTime.Now.ToString()));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}: Failed to openListen to receipt port {1}", DateTime.Now.ToString(), errorText));
                }
                //TESJH Added Accept

                gotReceipt = w.Read(ref receiptText, ref errorText, 30000, true);
                Console.WriteLine(string.Format("Read {0}:{1}", receiptText, errorText));              

                if (gotReceipt)
                {
                    Console.WriteLine(String.Format("{0}: Got receipt: {1}", DateTime.Now.ToString(), receiptText));
                }
                else
                {
                    Console.WriteLine(String.Format("{0}: Failed to get receipt", DateTime.Now.ToString()));
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