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
			Common.LoginReq Common_LoginReq = new Common.LoginReq(agent);
			AddProtocol(Common_LoginReq.GetProtocolType(), Common_LoginReq);
			Common.LoginRes Common_LoginRes = new Common.LoginRes(agent);
			AddProtocol(Common_LoginRes.GetProtocolType(), Common_LoginRes);
		}
	}
}
