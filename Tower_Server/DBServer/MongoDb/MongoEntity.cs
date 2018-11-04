using MongoDB.Bson;
using System;

namespace DBServer.MongoDb
{
    /// <summary>
    /// 基础实体
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 最近的一次更新时间
        /// </summary>
        public string UpdateTime { get; set; }

    }
}
