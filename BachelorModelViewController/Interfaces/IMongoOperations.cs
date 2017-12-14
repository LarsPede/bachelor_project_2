using Microsoft.AspNetCore.Http;
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

        // get all documents from a collection from time and forward
        Task<List<BsonDocument>> GetAllFromCollectionFromTime(string collectionName, int fromTime);

        // get all documents from a collection matching an or filter
        Task<List<BsonDocument>> GetAllFromCollectionByOrFilter(string collectionName, IQueryCollection jsonStringFilter);

        // get all documents from a collection matching an and filter
        Task<List<BsonDocument>> GetAllFromCollectionByAndFilter(string collectionName, IQueryCollection jsonStringFilter);

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

        // check if Mongo contains collection
        bool CollectionExists(string collectionName);
    }
}
