using CoreFramework.Thread;
using CoreFramework.Utilities;
using Dream.Server.CoreFramework.Utilities;
using DreamEngine.Net.Protocols.System;
using DreamEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CoreFramework.Network
{
    public class NetworkManager<T> : Singleton<T>,
        IAgentProcessor,
        IControl
        where T : NetworkManager<T>, new()
    {
        protected List<Agent> agents = null;

        /// <summary>
        /// 代理改变
        /// </summary>
        /// <param name="agent"></param>
        public delegate void AgentChangeHandle(Agent agent);

        /// <summary>
        /// 有新的代理被激活并加入
        /// </summary>
        public event AgentChangeHandle AgentWillAdd;
        /// <summary>
        /// 一个网络代理关闭并移除
        /// </summary>
        public event AgentChangeHandle AgentDidRemoved;

        public NetworkManager()
        {
            agents = new List<Agent>();


        }

        #region Connect
        /// <summary>
        /// 连接远端服务器
        /// </summary>
        /// <param name="remoteEp">远端地址</param>
        /// <param name="connectedAgent">已连接成功的代理，如果为空则说明失败</param>
        /// <returns></returns>
        public virtual bool Connect(EndPoint remoteEp, out Agent connectedAgent)
        {
            connectedAgent = null;

            if (null == remoteEp)
                return false;

            Logger.Log("AgentManager Connect Start", "开始尝试连接，远端IP和端口" + remoteEp.ToString());
            bool ret = true;
            var connectSocket = new Socket(remoteEp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                connectSocket.Connect(remoteEp);

                connectedAgent = CreateAgent(connectSocket);
                if (connectedAgent != null)
                {
                    IPEndPoint iep = remoteEp as IPEndPoint;

                    var connectedNoti = connectedAgent.PopProtocol<ConnectedNoti>();
                    connectedNoti.Result = true;
                    connectedNoti.ServerInfo.Ip.Clear();
                    connectedNoti.ServerInfo.Ip.AddRange(iep.Address.GetAddressBytes());
                    connectedNoti.ServerInfo.Port = iep.Port;

                    connectedAgent.PushProtocol(connectedNoti);
                    this.AddAgent(connectedAgent);
                }
            }
            catch (System.Exception e)
            {
                Logger.LogError("AgentManager Connect Err", "连接远端服务器错误 错误信息为:" + e);
                ret = false;
            }

            return ret;
        }
        #endregion

        #region Listen
        /// <summary>
        /// 监听本地端口 准备接受其他服务器连接
        /// </summary>
        /// <param name="localEP"></param>
        /// <returns></returns>
        public virtual bool Listen(EndPoint localEP, ref Socket listenSocket)
        {
            if (null == localEP)
                return false;

            if (null != listenSocket)
                return false;

            Logger.Log("AgentManager Listen Start", "开始监听本地IP和端口" + localEP.ToString());
            bool ret = true;
            listenSocket = new Socket(localEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(localEP);
                listenSocket.Listen(10);

                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(OnAccept);

                if (!listenSocket.AcceptAsync(e))
                {
                    OnAccept(listenSocket, e);
                }
            }
            catch (System.Exception e)
            {
                Logger.LogError("AgentManager Listen Err", "绑定本地IP和端口错误 错误信息为:" + e);
                ret = false;
            }

            return ret;
        }
        #endregion

        #region Network Callback
        protected virtual void OnAccept(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Accept)
            {
                if (e.SocketError == SocketError.Success)
                {
                    Socket acceptSocket = e.AcceptSocket;

                    lock (agents)
                    {
                        Agent agent = CreateAgent(acceptSocket);
                        if (agent != null)
                        {
                            this.AddAgent(agent);
                        }
                    }
                }
                else
                {

                }

                Socket socket = sender as Socket;
                e.AcceptSocket = null;
                if (!socket.AcceptAsync(e))
                {
                    OnAccept(socket, e);
                }
            }
        }
        #endregion

        #region Processor
        /// <summary>
        /// 处理所有的代理迭代，回收所有需要清理的代理
        /// </summary>
        protected virtual void ProcessAgents()
        {
            if (System.Threading.Monitor.TryEnter(agents))
            {
                try
                {
                    bool hasnotNeedRemove = true;

                    for (int i = 0; i < agents.Count; i++)
                    {
                        var agent = agents[i];
                        if (null != agent)
                        {
                            agent.Update();
                            hasnotNeedRemove &= (agent.IsConnection);
                        }
                    }

                    if (!hasnotNeedRemove)
                    {

                        for (int i = agents.Count - 1; i >= 0; i--)
                        {
                            var agent = agents[i];
                            if (null == agent || !agent.IsConnection)
                            {
                                this.RemoveAgent(agent);
                                agent.Dispose();
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    System.Threading.Monitor.Exit(agents);
                }
            }
        }
        #endregion

        #region Protocol Processor
        protected virtual bool ProcessBreakupNoti(BreakupNoti noti)
        {

            return true;
        }

        protected virtual bool ProcessInterruption(InterruptionNoti noti)
        {
            return true;
        }

        protected virtual bool ProcessDisconnected(DisconnectedNoti noti)
        {

            return true;
        }
        #endregion

        #region Utility
        public virtual void AddAgent(Agent agent)
        {
            if (null != AgentWillAdd)
            {
                AgentWillAdd.Invoke(agent);
            }

            if (!this.agents.Contains(agent))
            {
                this.agents.Add(agent);
            }
        }

        public virtual void RemoveAgent(Agent agent)
        {
            if (this.agents.Contains(agent))
            {
                agents.Remove(agent);
            }

            if (null != AgentDidRemoved)
            {
                AgentDidRemoved.Invoke(agent);
            }
        }
        /// <summary>
        /// 通过socket来创建一个代理，并且加入断线的各种监听回调
        /// </summary>
        /// <param name="s">已accept或connected的socket</param>
        /// <returns>代理</returns>
        protected virtual Agent CreateAgent(Socket s)
        {
            Agent a = new Agent(s);
            if (a != null)
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

        public virtual bool Start()
        {
            return true;
        }

        public virtual bool Shut()
        {
            return true;
        }

        public virtual bool Update()
        {
            this.ProcessAgents();

            return true;
        }
        #endregion
    }
}
