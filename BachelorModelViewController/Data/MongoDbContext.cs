﻿using BachelorModelViewController.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database = null;


        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<BsonDocument> GetMongoCollection (string name)
        {
            return _database.GetCollection<BsonDocument>(name);
        }
    }
}
