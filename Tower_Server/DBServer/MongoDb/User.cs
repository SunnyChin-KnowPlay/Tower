using DBServer.MongoDb.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBServer.MongoDb
{
    public class User : BaseEntity
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public Resource Resource { get; set; }

        public User()
        {
          
        }
    }

    public class UserHelper : MongoHelper<User> { }
}
