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
	public partial class UserInfo : Info
    {
		/// <summary>
        /// 占位符
        /// </summary>
        private byte[] placeholder = new uint8[1];
        private BitArray bitcodes = new BitArray(8, false);

		/// <summary>
        ///  唯一ID
        /// </summary>
		public Guid Uuid
		{
			get
			{
				return m_Uuid;
			}
			set
			{
				m_Uuid = value;
			}
		}
		private Guid m_Uuid = Guid.Empty;

		/// <summary>
        ///  用户名
        /// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}
		private string m_Name = "";


		protected override byte[] EncodePlaceholder()
        {
			bitcodes.SetAll(false);

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

		public override byte[] Encode(byte[] buffer, ref int index)
        {
            buffer = base.Encode(buffer, ref index);
			if (bitcodes[0])
				Encode(buffer, ref index, m_Uuid);
			if (bitcodes[1])
				Encode(buffer, ref index, m_Name);
			return buffer;
        }

		public override void Decode(MemoryStream ms)
        {
            base.Decode(ms);
			if (bitcodes[0])
				Decode(ms, ref m_Uuid);
			if (bitcodes[1])
				Decode(ms, ref m_Name);
        }

		public override bool IsVaild()
        {
            bitcodes.SetAll(false);
			if (Uuid != Guid.Empty)
				bitcodes[0] = true;
			if (Name != "")
				bitcodes[1] = true;
            for (int i = 0; i < bitcodes.Length; i++)
            {
                if (bitcodes[i])
                    return true;
            }

            return false;
        }

		public void Reset()
		{
			m_Uuid = Guid.Empty;
			m_Name = "";
		}

		public UserInfo Clone()
		{
			UserInfo obj = new UserInfo();
			obj.Uuid = this.Uuid;
			obj.Name = this.Name;
			return obj;
		}

		public override string GetProtocolName()
        {
            return "UserInfo";
        }
	}
}