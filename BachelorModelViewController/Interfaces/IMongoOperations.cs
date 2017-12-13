using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Interfaces
{
    public interface IMongoOperations
    {

        // get all documents from a collection
        Task<List<BsonDocument>> GetAllFromCollection(string collectionName);

        // get single document
        Task<IActionResult> GetFromCollection(string collectionName, string id);

        // add new document
        Task<IActionResult> AddToCollection(string collectionName, BsonDocument document);

        // remove a single document
        Task<IActionResult> RemoveFromCollection(string collectionName, string id);

        // update just a single document
        Task<IActionResult> UpdateInCollection(string collectionName, string id, string body);

        // should be used with high cautious
        Task<IActionResult> RemoveAllFromCollection(string collectionName);
    }
}
