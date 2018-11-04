using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CoreFramework.Network;
using CoreFramework.Utilities;
using DBServer.Network;
using DreamEngine.Net.Protocols.Common;
using DreamEngine.Net.Protocols.Server;
using DreamEngine.Net.Protocols.System;

namespace DBServer.Logic
{
    public class Controller : IControl
    {
        /// <summary>
        /// 代理管理器
        /// </summary>
        private AgentManager agentManager = null;

        public Controller()
        {
            agentManager = AgentManager.Instance;
            agentManager.AgentWillAdd += OnAddAgent;
        }

        public virtual bool Shut()
        {
            return true;
        }

        public virtual bool Start()
        {
            if (null != agentManager)
            {

                IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6868);
                Socket socket = null;
                agentManager.Listen(iep, ref socket);


            }

            return true;
        }

        public virtual bool Update()
        {
            if (null != agentManager)
            {
                agentManager.Update();
            }
            return true;
        }

        #region Agent
        private void OnAddAgent(Agent agent)
        {
            var protocol = agent.FindProtocol<ReqLogin>();
            if (null != protocol)
            {
                protocol += ProcessReqLogin;
            }

            var breakupNoti = agent.FindProtocol<BreakupNoti>();
            breakupNoti += ProcessBreakupNoti;

        }
        #endregion

        #region Process Protocol
        private bool ProcessReqLogin(ReqLogin req)
        {
            int d = 1;

            AckLogin ackLogin = req.Agent.PopProtocol<AckLogin>();
            ackLogin.UserInfo.Uuid = Guid.NewGuid();

            req.Agent.Send(ackLogin);
            return true;
        }

        private bool ProcessBreakupNoti(BreakupNoti noti)
        {
            int d = 1;
            return true;
        }
        #endregion
    }
}
