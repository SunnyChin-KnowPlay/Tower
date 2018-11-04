using DreamEngine.Net;
using DreamEngine.Net.Protocols;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Agent : AgentBase
{
    public ProtocolBaseManager protocolManager;

    public override ProtocolBaseManager ProtocolManager
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

    protected override void ProcessReceived(MessagePackage p)
    {
        int length = p.Length;
        var buffer = p.Buffer;

        try
        {
            m_IsProcessing = true;
            int index = 0;

            byte[] buff = new byte[2];
            Array.Copy(buffer, index, buff, 0, 2);

            UInt16 protocolType = BitConverter.ToUInt16(buff, 0);
            protocolType = (UInt16)IPAddress.NetworkToHostOrder((Int16)protocolType);

            if (ProtocolManager.Protocols.ContainsKey(protocolType))
            {
                Info protocol = ProtocolManager.Protocols[protocolType];

                IProtocol baseProtocol = protocol as IProtocol;

                Log(string.Format("Recv Prot: Prot type is:{0:x4}; Name is:{1};  Length is:{2}", baseProtocol.GetProtocolType(), baseProtocol.GetProtocolName(), length));

                Info popedProtocol = ProtocolManager.PopProtocol(baseProtocol.GetProtocolType());
                IProtocol protocolInterface = popedProtocol as IProtocol;

                lock (m_MemoryStream)
                {
                    if (m_MemoryStream.Capacity < length)
                    {
                        m_MemoryStream = new MemoryStream(length);
                    }

                    m_MemoryStream.Seek(0, SeekOrigin.Begin);
                    m_MemoryStream.Write(buffer, index, length);
                    m_MemoryStream.Seek(0, SeekOrigin.Begin);

                    popedProtocol.Decode(m_MemoryStream);
                }

                protocolInterface.SetAgent(this);
                protocolInterface.SetRemoteEndPoint(p.m_RemoteEndPoint);
                protocolInterface.SetLocalEndPoint(p.m_LocalEndPoint);


                if (baseProtocol.HasReceived())
                {
                    baseProtocol.Dispatch(protocolInterface);
                }
                ProtocolManager.PushProtocol(protocolInterface.GetProtocolType(), protocolInterface);
            }

        }
        finally
        {
            m_IsProcessing = false;
        }
    }

}
