using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES.Integration.IPSockets;

namespace Test_Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorText = "";
            Base b = new Base("987877898", 23, 5);
            try
            {
                if (b.connect(ref errorText))
                {
                    Console.WriteLine("Connected.....");
                }
                else
                {
                    Console.WriteLine(errorText);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
        }
    }
}
