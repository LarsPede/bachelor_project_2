﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorModelViewController.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using BachelorModelViewController.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace BachelorModelViewController.Data
{
    public class MongoOperations : IMongoOperations
    {
        private readonly MongoDbContext _context = null;

        public MongoOperations (IOptions<MongoSettings> settings)
        {
            _context = new MongoDbContext(settings);
        }

        public Task<IActionResult> AddToCollection(string collectionName, BsonDocument document)
        {
            throw new NotImplementedException();
        }

        public bool CollectionExists(string collectionName)
        {
            return _context.CollectionExists(collectionName).Result;
        }

        public async Task<List<BsonDocument>> GetAllFromCollection(string collectionName)
        {
            var documents = await _context.GetMongoCollection(collectionName).Find(_ => true).ToListAsync();
            return documents;
        }

        public async Task<List<BsonDocument>> GetAllFromCollectionByFilter(string collectionName, IQueryCollection queryStringFilters)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            foreach (var queryStringFilter in queryStringFilters)
            {
                IFormatProvider provider = CultureInfo.CurrentCulture;

                string key = queryStringFilter.Key.Replace("__gt", "");
                key = queryStringFilter.Key.Replace("__lt", "");

                int value;
                if (int.TryParse(queryStringFilter.Value, NumberStyles.Integer, provider, out value))
                {
                    if (queryStringFilter.Key.Contains("__gt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Gt(key, value);
                        filter = filter & tempFilter;
                    }
                    else if (queryStringFilter.Key.Contains("__lt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Lt(key, value);
                        filter = filter & tempFilter;
                    }
                    else
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Eq(key, value);
                        filter = filter & tempFilter;
                    }
                } else
                {
                    if (queryStringFilter.Key.Contains("__gt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Gt(key, queryStringFilter.Value.ToString());
                        filter = filter & tempFilter;
                    }
                    else if (queryStringFilter.Key.Contains("__lt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Lt(key, queryStringFilter.Value.ToString());
                        filter = filter & tempFilter;
                    }
                    else
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Eq(key, queryStringFilter.Value.ToString());
                        filter = filter & tempFilter;
                    }
                }

            }
            var documents = await _context.GetMongoCollection(collectionName).Find(filter).ToListAsync();
            return documents;
        }

        public async Task<List<BsonDocument>> GetAllFromCollectionFromTime(string collectionName, int fromTime)
        {
            var since = new ObjectId(fromTime, 0, 0, 0);
            var filter = Builders<BsonDocument>.Filter.Gt("_id", since);
            var documents = await _context.GetMongoCollection(collectionName).Find(filter).ToListAsync();
            return documents;
        }

        public Task<IActionResult> GetFromCollection(string collectionName, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> RemoveAllFromCollection(string collectionName)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> RemoveFromCollection(string collectionName, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateInCollection(string collectionName, string id, string body)
        {
            throw new NotImplementedException();
        }
    }
}
