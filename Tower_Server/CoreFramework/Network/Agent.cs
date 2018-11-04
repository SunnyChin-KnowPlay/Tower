using DreamEngine.Net;
using DreamEngine.Net.Protocols;
using DreamEngine.Net.Protocols.System;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CoreFramework.Network
{
    public class Agent : AgentBase
    {
        private ProtocolManager protocolManager = null;

        public override ProtocolBaseManager ProtocolManager
        {
            get { return protocolManager; }
        }

        public Agent(Socket s, int sendBufferSize = 4096, int recvBufferSize = 4096) : base(s, sendBufferSize, recvBufferSize)
        {
            protocolManager = new ProtocolManager();

        }

    }
}
