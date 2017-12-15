using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using TES.Integration.IPSockets2;

namespace Test_Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            string errorText = "";
            //string requestCPPayment = "-tr10 -am102 -rf99999991 -x";
            //string requestCPRefund = "-tr58 -am102 -rf99999991 -x";
            //string requestCNPPayment = "-tr09 -am102 -rf99999991 -x";
            string request = "-tr10 -am102 -rf99999991 -x";
            string response = "";
            string receiptText = "";
            string ipAddress = "192.168.1.212";
            string endOfDataTag = "-x";

            int transactionPort = 30500;
            int receiptPort = 30503;

            //string xmlString = "hello -rptc\"<receipt><copy> *CUSTOMER RECEIPT *</copy><header2> Southbank Centre Limited</header2><psn> 03 </psn></receipt>\"";
            //saveReceiptsToXML(xmlString);
            //return;

            //Base b = new Base("127.0.0.1", 30500, 0);
            Base transactionSocket = new Base(ipAddress, transactionPort, 0);
            try
            {
                if (transactionSocket.connect(ref errorText))
                {
                    Console.WriteLine(String.Format("{0}: Connected to transaction port", DateTime.Now.ToString()));
                    Console.WriteLine();

                    if (transactionSocket.Write(request, ref errorText))
                    {
                        Console.WriteLine(String.Format("{0}: Sending transaction: {1}", DateTime.Now.ToString(),request));

                        //Open & Listen to the receipt port
                        Base receiptSocket = new Base("", receiptPort, 0);
                        if (receiptSocket.OpenAccept(ref errorText))
                        {
                            Console.WriteLine(String.Format("{0}: Opened Listen receipt port", DateTime.Now.ToString()));

                            bool gotReceipt = false;
                            gotReceipt = receiptSocket.ReadAll(ref receiptText, ref errorText, 30000, true, endOfDataTag);

                            if (gotReceipt)
                            {
                                Console.WriteLine(String.Format("{0}: Got receipt: {1}", DateTime.Now.ToString(), receiptText));
                                saveReceiptsToXML(receiptText);
                            }
                            else
                            {
                                Console.WriteLine(String.Format("{0}: Failed to get receipt", DateTime.Now.ToString()));
                            }

                            // Get Second Receipt - Start
                            //if (receiptSocket.Accept(ref errorText))
                            //{
                            //    Console.WriteLine(String.Format("{0}: Accepting from receipt port", DateTime.Now.ToString()));
                            //}
                            //else
                            //{
                            //    Console.WriteLine(String.Format("{0}: Failed to accept from receipt port {1}", DateTime.Now.ToString(), errorText));
                            //}

                            //gotReceipt = false;
                            //gotReceipt = receiptSocket.ReadAll(ref receiptText, ref errorText, 30000, true);

                            //if (gotReceipt)
                            //{
                            //    Console.WriteLine(String.Format("{0}: Got receipt: {1}", DateTime.Now.ToString(), receiptText));
                            //    saveReceiptsToXML(receiptText);
                            //}
                            //else
                            //{
                            //    Console.WriteLine(String.Format("{0}: Failed to get receipt", DateTime.Now.ToString()));
                            //}
                        }
                        else
                        {
                            Console.WriteLine(String.Format("{0}: Failed to openListen to receipt port {1}", DateTime.Now.ToString(), errorText));
                        }

                        //Read the response
                        if (transactionSocket.ReadAll(ref response,ref errorText,0,true, endOfDataTag))
                        {
                            Console.WriteLine(String.Format("{0}: Received from transaction request {1}", DateTime.Now.ToString(), response));
                        }
                        else
                        {
                            Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(), errorText));
                        }

                        if (receiptSocket.close(ref errorText))
                        {
                            Console.WriteLine(String.Format("{0}: Receipt socket closed", DateTime.Now.ToString()));
                        }
                        else
                        {
                            Console.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString(),errorText));
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

        static void saveReceiptsToXML(string receiptText)
        {
            // Write Receipts To XML and File
            //XmlDocument custXmlFile = new XmlDocument();
            //custXmlFile.LoadXml(getReceiptXMLFromReceiptText(receiptText, "-rptc\"", "\""));
            //custXmlFile.Save("C:\\TES\\IPSocketTesting\\test.xml");

            XmlDocument custXmlFile = new XmlDocument();
            custXmlFile.LoadXml(getReceiptXMLFromReceiptText(receiptText, "-rptc\"", "\" -rptm"));
            custXmlFile.Save("C:\\TES\\IPSocketTesting\\CustReceipt.xml");

            XmlDocument vendXmlFile = new XmlDocument();
            vendXmlFile.LoadXml(getReceiptXMLFromReceiptText(receiptText, "-rptm\"", "\" -x"));
            vendXmlFile.Save("C:\\TES\\IPSocketTesting\\VendReceipt.xml");
        }

        static string getReceiptXMLFromReceiptText(string receiptText, string startString, string endString)
        {
            int startIndex = receiptText.IndexOf(startString) + startString.Length;
            int endIndex = receiptText.LastIndexOf(endString);
            int length = endIndex - startIndex;

            string xmlString = receiptText.Substring(startIndex, length);

            xmlString = "<receipt>" + xmlString + "</receipt>"; // needs a root element

            return xmlString;
        }
    }
}
