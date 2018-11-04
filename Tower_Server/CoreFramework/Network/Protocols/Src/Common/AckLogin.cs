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
	public partial class AckLogin : Protocol<AckLogin>
	{
		public delegate bool ReceivedHandle(AckLogin protocol);

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
        /// 
        /// </summary>
		public UserInfo UserInfo
		{
			get
			{
				return m_UserInfo;
			}
			set
			{
				m_UserInfo = value;
			}
		}
		private UserInfo m_UserInfo = new UserInfo();

		private static uint16 s_ProtocolType = 0x0004;
		
		public AckLogin()
		{

		}

		public AckLogin(AgentBase agent = null) : base(agent)
		{
			
		}

		public override uint16 GetProtocolType()
		{
			return s_ProtocolType;
		}

		public override string GetProtocolName()
        {
            return "AckLogin";
        }

		private static AckLogin s_AckLogin;
		public static AckLogin Instance
		{
			get
			{
				if (s_AckLogin == null)
				{
					s_AckLogin = new AckLogin();
				}
				return s_AckLogin;
			}
		}

		protected override byte[] EncodePlaceholder()
        {
			bitcodes.SetAll(false);
			if (UserInfo.IsVaild())
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
			if (UserInfo.IsVaild())
				return true;
            return false;
        }

		public override void Reset()
		{
			m_UserInfo.Reset();
		}

		public override byte[] Encode(byte[] buffer, ref int index)
        {
            buffer = base.Encode(buffer, ref index);
			if (UserInfo.IsVaild())
				Encode(buffer, ref index, m_UserInfo);
			return buffer;
        }

		public override void Decode(MemoryStream ms)
        {
            base.Decode(ms);
			if (bitcodes[0])
				Decode(ms, ref m_UserInfo);
        }

		public AckLogin Clone()
		{
			AckLogin obj = new AckLogin();
			obj.UserInfo = this.UserInfo.Clone();
			return obj;
		}

		public static AckLogin operator +(AckLogin s1, ReceivedHandle handle)
        {
            s1.RegisterListener(handle);
            return s1;
        } 

        public static AckLogin operator -(AckLogin s1, ReceivedHandle handle)
        {
            s1.UnregisterListener(handle);
            return s1;
        }

		public static AckLogin operator -(AckLogin s1, object target)
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
				isContinue = pkg.m_Handle.Invoke(t as AckLogin);
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
			return new AckLogin(m_Agent);
		}

		public override bool HasReceived()
		{
			return m_Receiveds.Count > 0;
		}
	}
}