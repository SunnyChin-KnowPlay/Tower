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
	public partial class ServerInfoReport : Protocol<ServerInfoReport>
	{
		public delegate bool ReceivedHandle(ServerInfoReport protocol);

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
        private byte[] placeholder = new uint8[2];
        private BitArray bitcodes = new BitArray(16, false);
		
		/// <summary>
        ///  服务器id
        /// </summary>
		public uint64 Target_id
		{
			get
			{
				return m_Target_id;
			}
			set
			{
				m_Target_id = value;
			}
		}
		private uint64 m_Target_id = (uint64)0;
		/// <summary>
        /// 
        /// </summary>
		public uint64 Server_id
		{
			get
			{
				return m_Server_id;
			}
			set
			{
				m_Server_id = value;
			}
		}
		private uint64 m_Server_id = (uint64)0;
		/// <summary>
        /// 
        /// </summary>
		public string Server_name 
		{
			get
			{
				return m_Server_name ;
			}
			set
			{
				m_Server_name  = value;
			}
		}
		private string m_Server_name  = "";
		/// <summary>
        /// 
        /// </summary>
		public string Server_ip 
		{
			get
			{
				return m_Server_ip ;
			}
			set
			{
				m_Server_ip  = value;
			}
		}
		private string m_Server_ip  = "";
		/// <summary>
        /// 
        /// </summary>
		public int32 Server_port 
		{
			get
			{
				return m_Server_port ;
			}
			set
			{
				m_Server_port  = value;
			}
		}
		private int32 m_Server_port  = (int32)0;
		/// <summary>
        /// 
        /// </summary>
		public int32 Server_max_online 
		{
			get
			{
				return m_Server_max_online ;
			}
			set
			{
				m_Server_max_online  = value;
			}
		}
		private int32 m_Server_max_online  = (int32)0;
		/// <summary>
        /// 
        /// </summary>
		public int32 Server_cur_count 
		{
			get
			{
				return m_Server_cur_count ;
			}
			set
			{
				m_Server_cur_count  = value;
			}
		}
		private int32 m_Server_cur_count  = (int32)0;
		/// <summary>
        /// 
        /// </summary>
		public ServerState Server_state 
		{
			get
			{
				return (ServerState)m_Server_state ;
			}
			set
			{
				m_Server_state  = (int)value;
			}
		}
		private int m_Server_state  = 0;
		/// <summary>
        /// 
        /// </summary>
		public int32 Server_type 
		{
			get
			{
				return m_Server_type ;
			}
			set
			{
				m_Server_type  = value;
			}
		}
		private int32 m_Server_type  = (int32)0;

		private static uint16 s_ProtocolType = 0x0F01;
		
		public ServerInfoReport()
		{

		}

		public ServerInfoReport(AgentBase agent = null) : base(agent)
		{
			
		}

		public override uint16 GetProtocolType()
		{
			return s_ProtocolType;
		}

		public override string GetProtocolName()
        {
            return "ServerInfoReport";
        }

		private static ServerInfoReport s_ServerInfoReport;
		public static ServerInfoReport Instance
		{
			get
			{
				if (s_ServerInfoReport == null)
				{
					s_ServerInfoReport = new ServerInfoReport();
				}
				return s_ServerInfoReport;
			}
		}

		protected override byte[] EncodePlaceholder()
        {
			bitcodes.SetAll(false);
			if (Target_id != (uint64)0)
				bitcodes[0] = true;
			if (Server_id != (uint64)0)
				bitcodes[1] = true;
			if (Server_name  != "")
				bitcodes[2] = true;
			if (Server_ip  != "")
				bitcodes[3] = true;
			if (Server_port  != (int32)0)
				bitcodes[4] = true;
			if (Server_max_online  != (int32)0)
				bitcodes[5] = true;
			if (Server_cur_count  != (int32)0)
				bitcodes[6] = true;
			if (Server_state  != 0)
				bitcodes[7] = true;
			if (Server_type  != (int32)0)
				bitcodes[8] = true;
            this.ConvertPlaceholder(bitcodes, placeholder);
            return placeholder;
        }

		protected override void DecodePlaceholder(MemoryStream ms)
        {
            if (!IsEnough(ms, 2))
            {
                Debugger.Log("MemoryStream Error");
                return;
            }
            ms.Read(placeholder, 0, 2);
            ConvertToBitArray(bitcodes, placeholder);
        }

		public override bool IsVaild()
        {
			if (Target_id != (uint64)0)
				return true;
			if (Server_id != (uint64)0)
				return true;
			if (Server_name  != "")
				return true;
			if (Server_ip  != "")
				return true;
			if (Server_port  != (int32)0)
				return true;
			if (Server_max_online  != (int32)0)
				return true;
			if (Server_cur_count  != (int32)0)
				return true;
			if (Server_state  != 0)
				return true;
			if (Server_type  != (int32)0)
				return true;
            return false;
        }

		public override void Reset()
		{
			m_Target_id = (uint64)0;
			m_Server_id = (uint64)0;
			m_Server_name  = "";
			m_Server_ip  = "";
			m_Server_port  = (int32)0;
			m_Server_max_online  = (int32)0;
			m_Server_cur_count  = (int32)0;
			m_Server_state  = 0;
			m_Server_type  = (int32)0;
		}

		public override byte[] Encode(byte[] buffer, ref int index)
        {
            buffer = base.Encode(buffer, ref index);
			if (Target_id != (uint64)0)
				Encode(buffer, ref index, m_Target_id);
			if (Server_id != (uint64)0)
				Encode(buffer, ref index, m_Server_id);
			if (Server_name  != "")
				Encode(buffer, ref index, m_Server_name );
			if (Server_ip  != "")
				Encode(buffer, ref index, m_Server_ip );
			if (Server_port  != (int32)0)
				Encode(buffer, ref index, m_Server_port );
			if (Server_max_online  != (int32)0)
				Encode(buffer, ref index, m_Server_max_online );
			if (Server_cur_count  != (int32)0)
				Encode(buffer, ref index, m_Server_cur_count );
			if (Server_state  != 0)
				Encode(buffer, ref index, m_Server_state );
			if (Server_type  != (int32)0)
				Encode(buffer, ref index, m_Server_type );
			return buffer;
        }

		public override void Decode(MemoryStream ms)
        {
            base.Decode(ms);
			if (bitcodes[0])
				Decode(ms, ref m_Target_id);
			if (bitcodes[1])
				Decode(ms, ref m_Server_id);
			if (bitcodes[2])
				Decode(ms, ref m_Server_name );
			if (bitcodes[3])
				Decode(ms, ref m_Server_ip );
			if (bitcodes[4])
				Decode(ms, ref m_Server_port );
			if (bitcodes[5])
				Decode(ms, ref m_Server_max_online );
			if (bitcodes[6])
				Decode(ms, ref m_Server_cur_count );
			if (bitcodes[7])
				Decode(ms, ref m_Server_state );
			if (bitcodes[8])
				Decode(ms, ref m_Server_type );
        }

		public ServerInfoReport Clone()
		{
			ServerInfoReport obj = new ServerInfoReport();
			obj.Target_id = this.Target_id;
			obj.Server_id = this.Server_id;
			obj.Server_name  = this.Server_name ;
			obj.Server_ip  = this.Server_ip ;
			obj.Server_port  = this.Server_port ;
			obj.Server_max_online  = this.Server_max_online ;
			obj.Server_cur_count  = this.Server_cur_count ;
			obj.Server_state  = this.Server_state ;
			obj.Server_type  = this.Server_type ;
			return obj;
		}

		public static ServerInfoReport operator +(ServerInfoReport s1, ReceivedHandle handle)
        {
            s1.RegisterListener(handle);
            return s1;
        } 

        public static ServerInfoReport operator -(ServerInfoReport s1, ReceivedHandle handle)
        {
            s1.UnregisterListener(handle);
            return s1;
        }

		public static ServerInfoReport operator -(ServerInfoReport s1, object target)
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
				isContinue = pkg.m_Handle.Invoke(t as ServerInfoReport);
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
			return new ServerInfoReport(m_Agent);
		}

		public override bool HasReceived()
		{
			return m_Receiveds.Count > 0;
		}
	}
}