using CoreFramework.Network;
using Dream.Server.CoreFramework.Utilities;
using System.Net;
using System.Configuration;
using System.Net.Sockets;
using System;

namespace DBServer.Network
{
    public class AgentManager : NetworkManager<AgentManager>
    {

        public AgentManager()
        {

        }

        private void AgentAdded(Agent agent)
        {
            var dict = agent.ProtocolManager.Protocols;
        }

        public override bool Update()
        {
            bool ret = base.Update();
            if (ret)
            {

            }
            return ret;
        }


    }
}
