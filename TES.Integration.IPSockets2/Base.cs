using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TES.Integration.IPSockets2
{
    public class Base
    {
        private IPAddress ipAddress;
        private IPHostEntry ipHostInfo;
        private IPEndPoint remoteEP;
        private Socket socket;
        private Socket openAcceptSocket;

        public Base(string v_IPAddress, int v_Port, int v_Timeout)
        {
            this.p_IPAddress = v_IPAddress;
            this.p_Port = v_Port;
            this.p_Timeout = v_Timeout;
        }

        private bool CheckSocketStatus(SelectMode ModetoTest) =>
            this.socket.Poll(this.p_Timeout, ModetoTest);

        public bool close(ref string r_error)
        {
            try
            {
                if (this.socket.Connected)
                {

                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                }
                if (openAcceptSocket.Connected)
                {
                    openAcceptSocket.Shutdown(SocketShutdown.Both);
                    openAcceptSocket.Close();
                }
                return true;
            }
            catch (SocketException exception)
            {
                r_error = exception.Message;
                return false;
            }
            catch (Exception exception2)
            {
                r_error = exception2.Message;
                return false;
            }
        }

        public bool connect(ref string r_error)
        {
            try
            {
                if (this.p_IPAddress != "")
                {
                    this.ipAddress = IPAddress.Parse(this.p_IPAddress);
                }
                else
                {
                    //this.ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    this.ipHostInfo = Dns.GetHostEntry(p_IPAddress);
                    this.ipAddress = this.ipHostInfo.AddressList[0];
                }
                this.remoteEP = new IPEndPoint(this.ipAddress, this.p_Port);
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(this.remoteEP);
                return true;
            }
            catch (SocketException exception)
            {
                r_error = exception.Message;
                return false;
            }
            catch (Exception exception2)
            {
                r_error = exception2.Message;
                return false;
            }
        }

        public bool Read(ref string r_replymessage, ref string r_error, int v_timeout, bool v_returntrueontimeout)
        {
            byte[] buffer = new byte[0x400];
            StringBuilder builder = new StringBuilder();
            try
            {
                if (v_timeout != 0)
                {
                    this.socket.ReceiveTimeout = v_timeout;
                }
                int count = this.socket.Receive(buffer);
                builder.Append(Encoding.ASCII.GetString(buffer, 0, count));
                r_replymessage = builder.ToString();
                return true;
            }
            catch (SocketException exception)
            {
                if (v_returntrueontimeout)
                {
                    if (exception.ErrorCode == 0x274c)
                    {
                        r_error = exception.Message;
                        return true;
                    }
                    r_error = exception.Message;
                    return false;
                }
                r_error = exception.Message;
                return false;
            }
            catch (Exception exception2)
            {
                r_error = exception2.Message;
                return false;
            }
        }

        public bool ReadAll(ref string r_replymessage, ref string r_error, int v_timeout, bool v_returntrueontimeout, string v_endOfDataTag)
        {
            byte[] buffer = new byte[0x400];
            StringBuilder builder = new StringBuilder();
            //string receivedData = null;

            try
            {
                if (v_timeout != 0)
                {
                    this.socket.ReceiveTimeout = v_timeout;
                }

                while (true)
                {
                    int count = this.socket.Receive(buffer);
                    builder.Append(Encoding.ASCII.GetString(buffer, 0, count));
                    if (builder.ToString().IndexOf(v_endOfDataTag) > -1)
                    {
                        break;
                    }
                    //receivedData += Encoding.ASCII.GetString(buffer, 0, count);
                    //if (receivedData.IndexOf(v_endOfDataTag) > -1)
                    //{
                    //    break;
                    //}
                }
                r_replymessage = builder.ToString();
                //r_replymessage = receivedData;
                return true;
            }
            catch (SocketException exception)
            {
                if (v_returntrueontimeout)
                {
                    if (exception.ErrorCode == 0x274c)
                    {
                        r_error = exception.Message;
                        return true;
                    }
                    r_error = exception.Message;
                    return false;
                }
                r_error = exception.Message;
                return false;
            }
            catch (Exception exception2)
            {
                r_error = exception2.Message;
                return false;
            }
        }

        public bool Write(string v_message, ref string r_error)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(v_message);
                int num = this.socket.Send(bytes);
                return true;
            }
            catch (Exception exception)
            {
                r_error = exception.Message;
                return false;
            }
        }

        public bool open(ref string r_error)
        {
            try
            {
                if (this.p_IPAddress != "")
                {
                    this.ipAddress = IPAddress.Parse(this.p_IPAddress);
                }
                else
                {
                    //this.ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    this.ipHostInfo = Dns.GetHostEntry(p_IPAddress);
                    this.ipAddress = this.ipHostInfo.AddressList[0];
                }
                this.remoteEP = new IPEndPoint(this.ipAddress, this.p_Port);
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Bind(this.remoteEP);
                this.socket.Listen(10);
            }
            catch (Exception e)
            {
                r_error = e.Message;
                return false;
            }
            return true;
        }

        public bool OpenAccept(ref string r_error)
        {
            try
            {
                if (this.p_IPAddress != "")
                {
                    this.ipAddress = IPAddress.Parse(this.p_IPAddress);
                }
                else
                {
                    //this.ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    this.ipHostInfo = Dns.GetHostEntry(p_IPAddress);
                    this.ipAddress = this.ipHostInfo.AddressList[1]; // If you get address incompatible error try using 1 instead of 0 
                }
                this.remoteEP = new IPEndPoint(this.ipAddress, this.p_Port);
                this.openAcceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.openAcceptSocket.Bind(this.remoteEP);
                this.openAcceptSocket.Listen(10);

                this.socket = this.openAcceptSocket.Accept();
            }
            catch (Exception e)
            {
                r_error = e.Message;
                return false;
            }
            return true;
        }

        public bool Accept(ref string r_error)
        {
            try
            {
                if (openAcceptSocket != null)
                {
                    socket = openAcceptSocket.Accept();
                }
                else
                {
                    if (!OpenAccept(ref r_error))
                    {
                        return false;
                    };
                }
            }
            catch (Exception e)
            {
                r_error = e.Message;
                return false;
            }
            return true;
        }

        public string p_IPAddress { get; set; }

        public int p_Port { get; set; }

        public int p_Timeout { get; set; }
    }
}
