using DreamEngine.Net;
using DreamEngine.Net.Protocols;
using System.Net.Sockets;
using UnityEngine;

public class Agent : AgentBase
{
    protected override ProtocolBaseManager ProtocolManager
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    public Agent(Socket s, int sendBufferSize = 1048576, int recvBufferSize = 1048576) : base(s, sendBufferSize, recvBufferSize)
    {

    }

}
