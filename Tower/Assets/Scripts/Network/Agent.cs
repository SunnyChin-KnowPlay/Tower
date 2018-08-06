using DreamEngine.Net;
using DreamEngine.Net.Protocols;
using System.Net.Sockets;
using UnityEngine;

public class Agent : AgentBase
{
    public ProtocolBaseManager protocolManager;

    protected override ProtocolBaseManager ProtocolManager
    {
        get
        {
            return protocolManager;
        }
    }

    public Agent(Socket s, int sendBufferSize = 2048, int recvBufferSize = 2048) : base(s, sendBufferSize, recvBufferSize)
    {
        protocolManager = new ProtocolManager();
    }

}
