using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBServer.MongoDb
{
    public class Database
    {
        private static readonly string connStr = "mongodb://sunny:19861114@127.0.0.1:27017";

        private static readonly string dbName = "test";

        private static IMongoDatabase db = null;

        private static readonly object lockHelper = new object();

        private Database() { }

        public static IMongoDatabase GetDatabase()
        {
            if (db == null)
            {
                lock (lockHelper)
                {
                    if (db == null)
                    {
                        var client = new MongoClient(connStr);
                        db = client.GetDatabase(dbName);
                    }
                }
            }
            return db;
        }
    }

}
