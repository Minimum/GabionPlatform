using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using GabionCache.Settings;
using GabionCache.Storage;

namespace GabionCache.Proxy
{
    public class ReverseProxy
    {
        public int BufferSize { get; set; }

        protected int Port;
        protected IPAddress Address;
        protected Socket ServerSocket;
        protected bool State = false;

        public ReverseProxy(IPAddress address, ProxySettings settings)
        {
            Address = address;

            Port = settings.ListenPort;
            BufferSize = settings.BufferSize;
        }

        public bool Start()
        {
            if (!State)
            {
                try
                {
                    ServerSocket = new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    ServerSocket.Bind(new IPEndPoint(Address, Port));
                    ServerSocket.Listen(128);
                    ServerSocket.BeginAccept(OnAccept, ServerSocket);

                    State = true;
                }
                catch (Exception e)
                {
                    // TODO: Send bug info to UI
                    
                }
            }

            return State;
        }

        public bool Shutdown()
        {
            if (State)
            {
                try
                {
                    ServerSocket.Close();
                }
                catch (Exception e)
                {
                    // TODO: Send bug info to UI
                    
                }

                State = false;
            }

            return !State;
        }

        public void OnAccept(IAsyncResult result)
        {
            try
            {
                Socket clientSocket = ServerSocket.EndAccept(result);

                if (clientSocket != null)
                {
                    ProxyClient client = new ProxyClient(clientSocket);

                    client.Start();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
