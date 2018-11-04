using CoreFramework.Network;
using System.Net.Sockets;

namespace GateServer.Network
{
    public class ClientAgent : Agent
    {
        protected ulong agentId = 0;
        /// <summary>
        /// 唯一ID
        /// </summary>
        public ulong AgentId
        {
            get { return agentId; }
            set { agentId = value; }
        }

        public ClientAgent(Socket s, int sendBufferSize = 4096, int recvBufferSize = 4096)
            : base(s, sendBufferSize, recvBufferSize)
        {
           
        }

        
    }
}
