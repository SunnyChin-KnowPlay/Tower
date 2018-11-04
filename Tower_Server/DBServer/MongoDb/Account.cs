using System;
using System.Collections.Generic;
using System.Text;

namespace DBServer.MongoDb
{
    public class Account : BaseEntity
    {
        /// <summary>
        /// 账号名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    public class AccountHelper : MongoHelper<Account> { }
}
