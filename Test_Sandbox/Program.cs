using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TES.Integration.IPSockets;

namespace Test_Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorText = "";
            string request = "-tr10 -am102 -rf99999991 -x";
            string response = "";
            string receiptText = "";

            //Base b = new Base("127.0.0.1", 30500, 0);
            Base b = new Base("192.168.1.212", 30500, 0);
            try
            {
                if (b.connect(ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Connected to transaction port", DateTime.Now.ToString()));
                    Console.WriteLine();

                    //open the receipt port
                    //Base r = new Base("127.0.0.1", 30503, 0);
                    Base r = new Base("192.168.1.212", 30503, 0);
                    if (r.open(ref errorText))
                    {
                        Console.WriteLine(String.Format("{0}: Opened receipt port", DateTime.Now.ToString()));
                    }
                    else
                    {
                        Console.WriteLine(String.Format("{0}: Failed to open to receipt port {1}", DateTime.Now.ToString(),errorText));
                    }

                    Console.WriteLine();

                    if (b.Write(request, ref errorText))
                    {
                        Console.WriteLine(String.Format("{0}: Sending transaction: {1}", DateTime.Now.ToString(),request));

                        //Read the receipt port
                        //Base w = new Base("192.168.1.154", 30503, 0);
                        //if (!w.connect(ref errorText))
                        //{
                        //    Console.WriteLine(String.Format("{0}: failed to connect to recept socket: {1}", DateTime.Now.ToString(), errorText));
                        //}
                        
                        //bool gotReceipt = false;
                        //int counter = 1;

                        //while (gotReceipt=false || counter <30)
                        //{
                        //    counter += 1;
                        //    gotReceipt = r.Read(ref receiptText, ref errorText, 0, true);
                        //    Console.WriteLine(string.Format("Try {0}:{1}:{2}", counter.ToString(),receiptText,errorText));
                        //    Thread.Sleep(1000);
                        //}

                        //if (gotReceipt)
                        //{
                        //    Console.WriteLine(String.Format("{0}: Got receipt: {1}", DateTime.Now.ToString(), receiptText));
                        //}
                        //else
                        //{
                        //    Console.WriteLine(String.Format("{0}: Failed to get receipt", DateTime.Now.ToString()));
                        //}
                        
                        //Read the response
                        if (b.Read(ref response,ref errorText,0,true))
                        {
                            Console.WriteLine(String.Format("{0}: Received from transaction request {1}", DateTime.Now.ToString(), response));
                        }
                        else
                        {
                            Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(), errorText));
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(), errorText));
                    }
                    
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
