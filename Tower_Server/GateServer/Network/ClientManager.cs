using CoreFramework.Network;
using DreamEngine.Net.Protocols.System;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace GateServer.Network
{
    /// <summary>
    /// 客户端代理管理器
    /// </summary>
    public class ClientManager : NetworkManager<ClientManager>
    {
        /// <summary>
        /// 客户端代理队列
        /// </summary>
        private Dictionary<UInt64, ClientAgent> clientAgents = null;

        /// <summary>
        /// 当前最新的代理ID
        /// </summary>
        private UInt64 currentClientAgentId = 0;

        public ClientManager()
        {
            clientAgents = new Dictionary<ulong, ClientAgent>();
        }

        #region Override
        /// <summary>
        /// 通过socket来创建一个客户端代理，并且加入断线的各种监听回调
        /// </summary>
        /// <param name="s">已accept或connected的socket</param>
        /// <returns>代理</returns>
        protected override Agent CreateAgent(Socket s)
        {
            ClientAgent a = new ClientAgent(s);
            if (null != a)
            {
                a.Start();

                InterruptionNoti interruptionNoti = a.FindProtocol<InterruptionNoti>();
                interruptionNoti += ProcessInterruption;
                DisconnectedNoti disconnectedNoti = a.FindProtocol<DisconnectedNoti>();
                disconnectedNoti += ProcessDisconnected;
                BreakupNoti breakupNoti = a.FindProtocol<BreakupNoti>();
                breakupNoti += ProcessBreakupNoti;
            }
            return a;
        }

        /// <summary>
        /// 提取一个最新的客户端代理ID
        /// </summary>
        /// <returns></returns>
        private UInt64 PickClientAgentId()
        {
            if (currentClientAgentId + 1 >= UInt64.MaxValue)
                currentClientAgentId = 0;

            return ++currentClientAgentId;
        }

        public override void AddAgent(Agent agent)
        {
            base.AddAgent(agent);


            var clientAgent = agent as ClientAgent;
            if (!clientAgents.ContainsKey(clientAgent.AgentId))
            {
                clientAgents.Add(clientAgent.AgentId, clientAgent);
            }
        }

        public override void RemoveAgent(Agent agent)
        {
            base.RemoveAgent(agent);

            ClientAgent clientAgent = agent as ClientAgent;
            if (clientAgents.ContainsKey(clientAgent.AgentId))
            {
                clientAgents.Remove(clientAgent.AgentId);
            }
        }
        #endregion
    }
}
