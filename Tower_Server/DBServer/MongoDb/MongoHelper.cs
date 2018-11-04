using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DBServer.MongoDb
{
    public class MongoHelper<T> where T : BaseEntity
    {
        private IMongoDatabase database = null;

        private IMongoCollection<T> collection = null;

        public MongoHelper()
        {
            database = Database.GetDatabase();
            collection = database.GetCollection<T>(typeof(T).Name);
        }

        public T Insert(T entity)
        {
            var flag = Guid.NewGuid();
            entity.GetType().GetProperty("Id").SetValue(entity, flag);
            entity.State = "y";

            var dts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            entity.CreateTime = dts;
            entity.UpdateTime = dts;

            collection.InsertOneAsync(entity);
            return entity;
        }

        /// <summary>
        /// 查询并拿出一个
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>你想要的东西</returns>
        public T QueryOne(string id)
        {
            var guid = Guid.Parse(id);

            var list = collection.Find(a => a.Id == guid).ToList();
            if (null == list || list.Count < 1)
                return null;

            return list[0];
        }

        /// <summary>
        /// 查询并提取所有的
        /// </summary>
        /// <returns></returns>
        public List<T> QueryAll()
        {
            return collection.Find(a => a.State.Equals("y")).ToList();
        }


        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="filter">筛选器</param>
        /// <param name="options">选项</param>
        /// <returns></returns>
        public List<T> Find(FilterDefinition<T> filter, FindOptions options = null)
        {
            var res = this.collection.Find(filter, options);
            return res.ToList();
        }

        /// <summary>
        /// 读取表中所有的
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> FindAll(FindOptions options = null)
        {
            BsonDocument doc = new BsonDocument();
            return this.Find(doc, options);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public ReplaceOneResult Update(T entity)
        {
            var old = collection.Find(e => e.Id.Equals(entity.Id)).ToList()[0];

            foreach (var prop in entity.GetType().GetProperties())
            {
                var newValue = prop.GetValue(entity);
                var oldValue = old.GetType().GetProperty(prop.Name).GetValue(old);
                if (newValue != null)
                {
                    old.GetType().GetProperty(prop.Name).SetValue(old, newValue);
                    //if (!newValue.ToString().Equals(oldValue.ToString()))
                    //{

                    //}
                }
            }
            old.State = "y";
            old.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            ReplaceOneResult result = collection.ReplaceOne(filter, old);
            return result;
        }

        public void Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            collection.DeleteOneAsync(filter);
        }


    }


}
