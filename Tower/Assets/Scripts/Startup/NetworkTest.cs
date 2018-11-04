using DreamEngine.Net.Protocols.Common;
using DreamEngine.Net.Protocols.Server;
using DreamEngine.Utilities.Debugger;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    private Socket socket = null;

    private Agent agent = null;

    // Use this for initialization
    void Start()
    {
        Debugger.LogEvent += OnLog;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6868);
        socket.Connect(iep);

        agent = new Agent(socket);
        agent.IsLogger = true;
        agent.Start();

        var ackLogin = agent.FindProtocol<AckLogin>();
        ackLogin += ProcessAckLogin;

        var req = agent.PopProtocol<ReqLogin>();
        req.Account = "123";
        req.Password = "123";

        agent.Send(req);
    }

    // Update is called once per frame
    void Update()
    {
        if (null != agent)
        {
            agent.Update();
        }
    }

    private void OnDestroy()
    {
        agent.Dispose();
        agent = null;
    }

    private bool ProcessAckLogin(AckLogin ack)
    {
        int d = 1;
        return true;
    }

    private void OnLog(object log)
    {
        UnityEngine.Debug.Log(log);
    }
}
