namespace DreamEngine.Net.Protocols
{
	public class ProtocolManager : ProtocolBaseManager
	{
		public ProtocolManager(AgentBase agent = null)
		{
			Common.ReqConnect Common_ReqConnect = new Common.ReqConnect(agent);
			AddProtocol(Common_ReqConnect.GetProtocolType(), Common_ReqConnect);
			Common.AckConnect Common_AckConnect = new Common.AckConnect(agent);
			AddProtocol(Common_AckConnect.GetProtocolType(), Common_AckConnect);
			Common.ReqLogin Common_ReqLogin = new Common.ReqLogin(agent);
			AddProtocol(Common_ReqLogin.GetProtocolType(), Common_ReqLogin);
			Common.AckLogin Common_AckLogin = new Common.AckLogin(agent);
			AddProtocol(Common_AckLogin.GetProtocolType(), Common_AckLogin);
			Server.ServerInfoReport Server_ServerInfoReport = new Server.ServerInfoReport(agent);
			AddProtocol(Server_ServerInfoReport.GetProtocolType(), Server_ServerInfoReport);
			Server.ReqJoinToCenterServer Server_ReqJoinToCenterServer = new Server.ReqJoinToCenterServer(agent);
			AddProtocol(Server_ReqJoinToCenterServer.GetProtocolType(), Server_ReqJoinToCenterServer);
			Server.ReqQuitFromCenterServer Server_ReqQuitFromCenterServer = new Server.ReqQuitFromCenterServer(agent);
			AddProtocol(Server_ReqQuitFromCenterServer.GetProtocolType(), Server_ReqQuitFromCenterServer);
			Server.NtfServerInfoReportProxy Server_NtfServerInfoReportProxy = new Server.NtfServerInfoReportProxy(agent);
			AddProtocol(Server_NtfServerInfoReportProxy.GetProtocolType(), Server_NtfServerInfoReportProxy);
		}
	}
}
