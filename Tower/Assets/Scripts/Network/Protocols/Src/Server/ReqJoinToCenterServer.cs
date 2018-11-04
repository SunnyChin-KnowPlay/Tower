using DreamEngine.Utilities.Debugger;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using uint8 = System.Byte;
using int16 = System.Int16;
using uint16 = System.UInt16;
using int32 = System.Int32;
using uint32 = System.UInt32;
using int64 = System.Int64;
using uint64 = System.UInt64;

namespace DreamEngine.Net.Protocols.Server
{
	public partial class ReqJoinToCenterServer : Protocol<ReqJoinToCenterServer>
	{
		public delegate bool ReceivedHandle(ReqJoinToCenterServer protocol);

		struct ReceivedPkg
		{
			public bool m_IsAutoRemove;
			public ReceivedHandle m_Handle;
		}
		private List<ReceivedPkg> m_Receiveds = new List<ReceivedPkg>();
		private List<ReceivedHandle> m_PreRemoveReceiveds = new List<ReceivedHandle>();
		private List<object> m_PreRemoveReceivedTargets = new List<object>();
		private List<ReceivedPkg> m_PreAddReceiveds = new List<ReceivedPkg>();

		/// <summary>
        /// 占位符
        /// </summary>
        private byte[] placeholder = new uint8[1];
        private BitArray bitcodes = new BitArray(8, false);
		
		/// <summary>
        ///  此时服务器ID为0
        /// </summary>
		public ServerInfoReport Server
		{
			get
			{
				return m_Server;
			}
			set
			{
				m_Server = value;
			}
		}
		private ServerInfoReport m_Server = new ServerInfoReport();

		private static uint16 s_ProtocolType = 0x0F03;
		
		public ReqJoinToCenterServer()
		{

		}

		public ReqJoinToCenterServer(AgentBase agent = null) : base(agent)
		{
			
		}

		public override uint16 GetProtocolType()
		{
			return s_ProtocolType;
		}

		public override string GetProtocolName()
        {
            return "ReqJoinToCenterServer";
        }

		private static ReqJoinToCenterServer s_ReqJoinToCenterServer;
		public static ReqJoinToCenterServer Instance
		{
			get
			{
				if (s_ReqJoinToCenterServer == null)
				{
					s_ReqJoinToCenterServer = new ReqJoinToCenterServer();
				}
				return s_ReqJoinToCenterServer;
			}
		}

		protected override byte[] EncodePlaceholder()
        {
			bitcodes.SetAll(false);
			if (Server.IsVaild())
				bitcodes[0] = true;
            this.ConvertPlaceholder(bitcodes, placeholder);
            return placeholder;
        }

		protected override void DecodePlaceholder(MemoryStream ms)
        {
            if (!IsEnough(ms, 1))
            {
                Debugger.Log("MemoryStream Error");
                return;
            }
            ms.Read(placeholder, 0, 1);
            ConvertToBitArray(bitcodes, placeholder);
        }

		public override bool IsVaild()
        {
			if (Server.IsVaild())
				return true;
            return false;
        }

		public override void Reset()
		{
			m_Server.Reset();
		}

		public override byte[] Encode(byte[] buffer, ref int index)
        {
            buffer = base.Encode(buffer, ref index);
			if (Server.IsVaild())
				Encode(buffer, ref index, m_Server);
			return buffer;
        }

		public override void Decode(MemoryStream ms)
        {
            base.Decode(ms);
			if (bitcodes[0])
				Decode(ms, ref m_Server);
        }

		public ReqJoinToCenterServer Clone()
		{
			ReqJoinToCenterServer obj = new ReqJoinToCenterServer();
			obj.Server = this.Server.Clone();
			return obj;
		}

		public static ReqJoinToCenterServer operator +(ReqJoinToCenterServer s1, ReceivedHandle handle)
        {
            s1.RegisterListener(handle);
            return s1;
        } 

        public static ReqJoinToCenterServer operator -(ReqJoinToCenterServer s1, ReceivedHandle handle)
        {
            s1.UnregisterListener(handle);
            return s1;
        }

		public static ReqJoinToCenterServer operator -(ReqJoinToCenterServer s1, object target)
        {
            s1.UnregisterListener(target);
            return s1;
        }

		public void RegisterListener(ReceivedHandle handle, bool isAutoRemove = false)
		{
			if (m_IsDispatching)
            {
                for (int i = 0; i < m_PreAddReceiveds.Count; i++)
                {
                    var p = m_PreAddReceiveds[i];
                    if (p.m_Handle == handle)
                        return;
                }
            }
            else
            {
                for (int i = 0; i < m_Receiveds.Count; i++)
                {
                    var p = m_Receiveds[i];
                    if (p.m_Handle == handle)
                        return;
                }
            }

			ReceivedPkg pkg = new ReceivedPkg();
			pkg.m_IsAutoRemove = isAutoRemove;
			pkg.m_Handle = handle;
			if (m_IsDispatching)
			{
				m_PreAddReceiveds.Add(pkg);
			}
			else
			{
				m_Receiveds.Add(pkg);
			}
		}

		public void UnregisterListener(ReceivedHandle handle)
		{
			if (m_IsDispatching)
			{
				m_PreRemoveReceiveds.Add(handle);
			}
			else
			{
				foreach (ReceivedPkg pkg in m_Receiveds)
				{
					if (pkg.m_Handle == handle)
					{
						m_Receiveds.Remove(pkg);
						break;
					}
				}
			}
		}

		public void UnregisterListener(object target)
        {
            if (m_IsDispatching)
            {
                m_PreRemoveReceivedTargets.Add(target);
            }
            else
            {
                for (int i = m_Receiveds.Count - 1; i >= 0; i--)
                {
                    var pkg = m_Receiveds[i];
                    if (pkg.m_Handle.Target == target)
                    {
                        m_Receiveds.RemoveAt(i);
                    }
                }
            }
        }

		public override void Dispatch(IProtocol t)
		{
			m_IsDispatching = true;

			bool isContinue = true;
			foreach (ReceivedPkg pkg in m_Receiveds)
			{
				isContinue = pkg.m_Handle.Invoke(t as ReqJoinToCenterServer);
				if (pkg.m_IsAutoRemove)
				{
					UnregisterListener(pkg.m_Handle);
				}
				if (isContinue == false)
					break;
			}

			foreach (ReceivedHandle handle in m_PreRemoveReceiveds)
			{
				foreach (ReceivedPkg pkg in m_Receiveds)
				{
					if (pkg.m_Handle == handle)
					{
						m_Receiveds.Remove(pkg);
						break;
					}
				}
			}

			foreach (var target in m_PreRemoveReceivedTargets)
            {
                for (int i = m_Receiveds.Count - 1; i >= 0; i--)
                {
                    var pkg = m_Receiveds[i];
                    if (pkg.m_Handle.Target == target)
                    {
                        m_Receiveds.RemoveAt(i);
                    }
                }
			}

			foreach (ReceivedPkg pkg in m_PreAddReceiveds)
			{
				m_Receiveds.Add(pkg);
			}

			m_PreRemoveReceiveds.Clear();
            m_PreRemoveReceivedTargets.Clear();
            m_PreAddReceiveds.Clear();

			m_IsDispatching = false;
		}

		public void Request()
		{

		}

		public override IProtocol Make()
		{
			return new ReqJoinToCenterServer(m_Agent);
		}

		public override bool HasReceived()
		{
			return m_Receiveds.Count > 0;
		}
	}
}