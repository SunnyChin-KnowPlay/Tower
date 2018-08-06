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

namespace DreamEngine.Net.Protocols.Common
{
	public partial class LoginReq : Protocol<LoginReq>
	{
		public delegate bool ReceivedHandle(LoginReq protocol);

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
        /// 
        /// </summary>
		public string Account
		{
			get
			{
				return m_Account;
			}
			set
			{
				m_Account = value;
			}
		}
		private string m_Account = "";
		/// <summary>
        /// 
        /// </summary>
		public string Password
		{
			get
			{
				return m_Password;
			}
			set
			{
				m_Password = value;
			}
		}
		private string m_Password = "";
		/// <summary>
        /// 
        /// </summary>
		public string C1
		{
			get
			{
				return m_C1;
			}
			set
			{
				m_C1 = value;
			}
		}
		private string m_C1 = "";
		/// <summary>
        /// 
        /// </summary>
		public string C2
		{
			get
			{
				return m_C2;
			}
			set
			{
				m_C2 = value;
			}
		}
		private string m_C2 = "";
		/// <summary>
        /// 
        /// </summary>
		public string C3
		{
			get
			{
				return m_C3;
			}
			set
			{
				m_C3 = value;
			}
		}
		private string m_C3 = "";
		/// <summary>
        /// 
        /// </summary>
		public string C4
		{
			get
			{
				return m_C4;
			}
			set
			{
				m_C4 = value;
			}
		}
		private string m_C4 = "";
		/// <summary>
        /// 
        /// </summary>
		public string C5
		{
			get
			{
				return m_C5;
			}
			set
			{
				m_C5 = value;
			}
		}
		private string m_C5 = "";
		/// <summary>
        /// 
        /// </summary>
		public string C6
		{
			get
			{
				return m_C6;
			}
			set
			{
				m_C6 = value;
			}
		}
		private string m_C6 = "";
		/// <summary>
        /// 
        /// </summary>
		public string C7
		{
			get
			{
				return m_C7;
			}
			set
			{
				m_C7 = value;
			}
		}
		private string m_C7 = "";

		private static uint16 s_ProtocolType = 0x0003;
		
		public LoginReq()
		{

		}

		public LoginReq(AgentBase agent = null) : base(agent)
		{
			
		}

		public override uint16 GetProtocolType()
		{
			return s_ProtocolType;
		}

		public override string GetProtocolName()
        {
            return "LoginReq";
        }

		private static LoginReq s_LoginReq;
		public static LoginReq Instance
		{
			get
			{
				if (s_LoginReq == null)
				{
					s_LoginReq = new LoginReq();
				}
				return s_LoginReq;
			}
		}

		protected override byte[] EncodePlaceholder()
        {
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
            bitcodes.SetAll(false);
			if (Account != "")
				bitcodes[0] = true;
			if (Password != "")
				bitcodes[1] = true;
			if (C1 != "")
				bitcodes[2] = true;
			if (C2 != "")
				bitcodes[3] = true;
			if (C3 != "")
				bitcodes[4] = true;
			if (C4 != "")
				bitcodes[5] = true;
			if (C5 != "")
				bitcodes[6] = true;
			if (C6 != "")
				bitcodes[7] = true;
			if (C7 != "")
				bitcodes[8] = true;
            for (int i = 0; i < bitcodes.Length; i++)
            {
                if (bitcodes[i])
                    return true;
            }

            return false;
        }

		public override void Reset()
		{
			m_Account = "";
			m_Password = "";
			m_C1 = "";
			m_C2 = "";
			m_C3 = "";
			m_C4 = "";
			m_C5 = "";
			m_C6 = "";
			m_C7 = "";
		}

		public override byte[] Encode(byte[] buffer, ref int index)
        {
            buffer = base.Encode(buffer, ref index);
			if (bitcodes[0])
				Encode(buffer, ref index, m_Account);
			if (bitcodes[1])
				Encode(buffer, ref index, m_Password);
			if (bitcodes[2])
				Encode(buffer, ref index, m_C1);
			if (bitcodes[3])
				Encode(buffer, ref index, m_C2);
			if (bitcodes[4])
				Encode(buffer, ref index, m_C3);
			if (bitcodes[5])
				Encode(buffer, ref index, m_C4);
			if (bitcodes[6])
				Encode(buffer, ref index, m_C5);
			if (bitcodes[7])
				Encode(buffer, ref index, m_C6);
			if (bitcodes[8])
				Encode(buffer, ref index, m_C7);
			return buffer;
        }

		public override void Decode(MemoryStream ms)
        {
            base.Decode(ms);
			if (bitcodes[0])
				Decode(ms, ref m_Account);
			if (bitcodes[1])
				Decode(ms, ref m_Password);
			if (bitcodes[2])
				Decode(ms, ref m_C1);
			if (bitcodes[3])
				Decode(ms, ref m_C2);
			if (bitcodes[4])
				Decode(ms, ref m_C3);
			if (bitcodes[5])
				Decode(ms, ref m_C4);
			if (bitcodes[6])
				Decode(ms, ref m_C5);
			if (bitcodes[7])
				Decode(ms, ref m_C6);
			if (bitcodes[8])
				Decode(ms, ref m_C7);
        }

		public LoginReq Clone()
		{
			LoginReq obj = new LoginReq();
			obj.Account = this.Account;
			obj.Password = this.Password;
			obj.C1 = this.C1;
			obj.C2 = this.C2;
			obj.C3 = this.C3;
			obj.C4 = this.C4;
			obj.C5 = this.C5;
			obj.C6 = this.C6;
			obj.C7 = this.C7;
			return obj;
		}

		public static LoginReq operator +(LoginReq s1, ReceivedHandle handle)
        {
            s1.RegisterListener(handle);
            return s1;
        } 

        public static LoginReq operator -(LoginReq s1, ReceivedHandle handle)
        {
            s1.UnregisterListener(handle);
            return s1;
        }

		public static LoginReq operator -(LoginReq s1, object target)
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
				isContinue = pkg.m_Handle.Invoke(t as LoginReq);
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
			return new LoginReq(m_Agent);
		}

		public override bool HasReceived()
		{
			return m_Receiveds.Count > 0;
		}
	}
}