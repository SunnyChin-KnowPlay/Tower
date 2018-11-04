using System;
using System.Collections.Generic;
using System.Text;

namespace DBServer.MongoDb.Common
{
    /// <summary>
    /// 资源
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// 金币
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// 钻石
        /// </summary>
        public int Diamond { get; set; }

        public Resource()
        {

        }

        public Resource(int gold, int diamond)
        {
            Gold = gold;
            Diamond = diamond;
        }

        
    }
}
