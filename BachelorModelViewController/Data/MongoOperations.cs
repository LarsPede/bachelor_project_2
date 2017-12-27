using System;
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

        public Task<bool> CreateCollection(string collectionName)
        {
            try
            {
                if (_context.CollectionExists(collectionName).Result)
                {
                    throw new MongoException("This channel already exists. You have to change the channel-name.");
                }
                _context.CreateDatabase(collectionName);
                return new Task<bool>(() => true);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public Task<bool> AddToCollection(string collectionName, BsonDocument document)
        {
            try
            {
                _context.GetMongoCollection(collectionName).InsertOne(document);
                return new Task<bool>(() => true);
            }
            catch(Exception e)
            {
                throw(e);
            }
        }

        public Task<int> AddMultipleToCollection(string collectionName, IEnumerable<BsonDocument> documents)
        {
            try
            {
                var collection = _context.GetMongoCollection(collectionName);

                collection.InsertMany(documents);
                return new Task<int>(() => documents.Count());
            }
            catch (Exception e)
            {
                throw;
            }
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

        public async Task<List<BsonDocument>> GetAllFromCollectionByAndFilter(string collectionName, IQueryCollection queryStringFilters)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            foreach (var queryStringFilter in queryStringFilters)
            {
                IFormatProvider provider = CultureInfo.CurrentCulture;

                string key = queryStringFilter.Key.Replace("__gt", "");
                key = key.Replace("__lt", "");

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

        public async Task<List<BsonDocument>> GetAllFromCollectionByOrFilter(string collectionName, IQueryCollection queryStringFilters)
        {
            var notPossible = new ObjectId(0, 0, 0, 0);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", notPossible);
            foreach (var queryStringFilter in queryStringFilters)
            {
                IFormatProvider provider = CultureInfo.CurrentCulture;

                string key = queryStringFilter.Key.Replace("__gt", "");
                key = key.Replace("__lt", "");

                int value;
                if (int.TryParse(queryStringFilter.Value, NumberStyles.Integer, provider, out value))
                {
                    if (queryStringFilter.Key.Contains("__gt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Gt(key, value);
                        filter = filter | tempFilter;
                    }
                    else if (queryStringFilter.Key.Contains("__lt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Lt(key, value);
                        filter = filter | tempFilter;
                    }
                    else
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Eq(key, value);
                        filter = filter | tempFilter;
                    }
                }
                else
                {
                    if (queryStringFilter.Key.Contains("__gt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Gt(key, queryStringFilter.Value.ToString());
                        filter = filter | tempFilter;
                    }
                    else if (queryStringFilter.Key.Contains("__lt"))
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Lt(key, queryStringFilter.Value.ToString());
                        filter = filter | tempFilter;
                    }
                    else
                    {
                        var tempFilter = Builders<BsonDocument>.Filter.Eq(key, queryStringFilter.Value.ToString());
                        filter = filter | tempFilter;
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

        public bool RemoveAllFromCollection(string collectionName)
        {
            try
            {
                _context.GetMongoCollection(collectionName).DeleteMany(_ => true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<IActionResult> RemoveFromCollection(string collectionName, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateInCollection(string collectionName, string id, string body)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCollection(string collectionName)
        {
            try
            {
                RemoveAllFromCollection(collectionName);
                _context.DeleteCollection(collectionName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
