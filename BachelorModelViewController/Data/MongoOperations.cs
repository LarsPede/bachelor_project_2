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

        public async Task<List<BsonDocument>> GetAllFromCollection(string collectionName)
        {
            var documents = await _context.GetMongoCollection(collectionName).Find(_ => true).ToListAsync();
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
