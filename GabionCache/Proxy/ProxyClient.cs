using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Heijden.DNS;
using GabionCache.Storage;
using TransportType = System.Net.TransportType;

namespace GabionCache.Proxy
{
    public class ProxyClient
    {
        public ReverseProxy Server { get; set; }
        public Socket Connection { get; set; }

        private byte[] Buffer;
        private String Message;
        private StringDictionary Header;
        private String HttpVersion;
        private String RequestedPath;
        private String HttpRequestType;

        private StorageFile TargetFile;

        public ProxyClient(Socket connection, ReverseProxy server)
        {
            Connection = connection;

            Buffer = new byte[server.BufferSize];
            Message = "";
            Header = new StringDictionary();

            Server = server;
        }

        public void Start()
        {
            Connection.BeginReceive(Buffer, 0, Server.BufferSize, SocketFlags.None, ReceiveHook, Connection);

            return;
        }

        private void ReceiveHook(IAsyncResult result)
        {
            int bufferLen = 0;

            try
            {
                bufferLen = Connection.EndReceive(result);
            }
            catch (Exception e)
            {
                // Connection lost

                return;
            }

            Message += Encoding.ASCII.GetString(Buffer, 0, bufferLen);

            if (ValidMessage(Message))
            {
                StorageFile file = new StorageFile();
                

                
            }
            else
            {
                
            }

            return;
        }

        private void ErrorSentHook(IAsyncResult ar)
        {
            try
            {
                Connection.EndSend(ar);
            }
            catch(Exception e)
            {
                
            }
            
            //Dispose();

            return;
        }

        private bool ValidMessage(String message)
        {
            bool success = false;

            int index = message.IndexOf("\r\n\r\n", StringComparison.Ordinal);

            if (index != -1)
            {
                Header = ParseMessage(message);

                if (HttpRequestType.ToUpper().Equals("POST"))
                {
                    int contentLen;

                    if (Int32.TryParse((String) Header["Content-Length"], out contentLen))
                    {
                        success = message.Length >= index + 6 + contentLen;
                    }
                    else
                    {
                        SendError();
                        success = true;
                    }
                }
                else
                {
                    success = true;
                }
            }

            return success;
        }

        private void SendError()
        {
            String html = "HTTP/1.1 400 Bad Request\r\nConnection: close\r\nContent-Type: text/html\r\n\r\n<html><head><title>400 Bad Request</title></head><body><span style=\"font-size:18px\">400 Bad Request</span></body></html>";

            try
            {
                Connection.BeginSend(Encoding.ASCII.GetBytes(html), 0, html.Length, SocketFlags.None, ErrorSentHook, Connection);
            }
            catch
            {
                //Dispose();
            }

            return;
        }

        private StringDictionary ParseMessage(string Message)
        {
            // TODO: THIS

            StringDictionary headerInfo = new StringDictionary();
            string[] lines = Message.Replace("\r\n", "\n").Split('\n');

            //Extract requested URL
            if (lines.Length > 0)
            {
                // Parse the Http Request Type
                int requestTypeLen = lines[0].IndexOf(' ');

                if (requestTypeLen > 0)
                {
                    HttpRequestType = lines[0].Substring(0, requestTypeLen);
                    lines[0] = lines[0].Substring(requestTypeLen).Trim();
                }

                // Parse the Http Version and the Requested Path
               

                // Remove http:// if present
               
            }

            

            return null;
        }

        private void ProcessMessage(string message)
        {
            // TODO: THIS

            // Parse header
            Header = ParseMessage(message);

            if (Header == null || !Header.ContainsKey("Host"))
            {
                SendError();

                return;
            }

           
           

            //
            try
            {
                // Check if file exists on disk or in memory
                if ((TargetFile = GetFile(RequestedPath)) != null)
                {
                    // File exists

                }
                else
                {
                    // File doesn't exist
                    IPEndPoint DestinationEndPoint = new IPEndPoint(IPAddress.Parse(DnsLookup(Host)), Port);

                    DestinationSocket = new Socket(DestinationEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    if (Header.ContainsKey("Proxy-Connection") && Header["Proxy-Connection"].ToLower().Equals("keep-alive"))
                        DestinationSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

                    DestinationSocket.BeginConnect(DestinationEndPoint, new AsyncCallback(this.OnConnected), DestinationSocket);
                }
            }
            catch
            {
                SendError();
                return;
            }
        }

        private String DnsLookup(String name)
        {
            Resolver resolver = new Resolver();
            resolver.DnsServer = "8.8.8.8";
            resolver.Recursion = true;
            resolver.Retries = 3;
            resolver.TimeOut = 1;
            resolver.TransportType = Heijden.DNS.TransportType.Udp;

            Response response = resolver.Query(name, QType.A);
            
            if (response.header.ANCOUNT > 0)
            {
                return response.Answers[0].RECORD.ToString();
            }

            return null;
        }

        public StorageFile GetFile(String path)
        {
            // check memory
            StorageFile file = CacheApp.Memory.GetFile(path);

            if (file == null)
            {
                // check disk
                file = new StorageFile(1);

                if (CacheApp.Disk.GetFile(path, file))
                {
                    if (CacheApp.Memory.AddFile(path, file))
                    {
                        CacheApp.Memory.AddFileGarbage(file);
                    }
                }
                else
                {
                    file = null;
                }
            }

            return file;
        }
    }
}
