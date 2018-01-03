using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES.Integration.IPSockets2;
using System.Net;

namespace Select_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //open a socket for reading
            IPHostEntry localIPHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIPAddress = localIPHostInfo.AddressList[1]; // If you get address incompatible error try using 1 instead of 0

            Base w = new Base(localIPAddress.ToString(), 30503, 0);

            string errortext = "";
            string replymessage = "";

            w.OpenReceivingSocket(ref errortext);

            Console.WriteLine("Socket Open");

            //Test status here. TRUE as the socket is open
            bool writeResult = w.OpenAcceptWritable();
            Console.WriteLine("Write: " + writeResult.ToString());

            //Test status here. FALSE as nothing has been written to the socket
            bool result = w.OpenAcceptReadable();
            Console.WriteLine(result.ToString());

            Base writer = new Base(localIPAddress.ToString(), 30503, 0);
            writer.connect(ref errortext);
            writer.Write("123456789-x", ref errortext);

            //Test status here. TRUE as something has been written to the socket
            result = w.OpenAcceptReadable();
            Console.WriteLine(result.ToString());

            w.Accept(ref errortext);
            w.ReadAll(ref replymessage, ref errortext, 0, false, "-x");
            Console.WriteLine(replymessage);

            //Test status here. TRUE as the socket is open
            writeResult = w.OpenAcceptWritable();
            Console.WriteLine("Write: " + writeResult.ToString());

            //Test status here. FALSE as something has been read off the socket to the socket
            result = w.OpenAcceptReadable();
            Console.WriteLine(result.ToString());

            w.close(ref errortext);
            
            //Test status here. FALSE as the socket is closed
            writeResult = w.OpenAcceptWritable();
            Console.WriteLine("Write: " + writeResult.ToString());

            Console.ReadLine();


        }
    }
}
