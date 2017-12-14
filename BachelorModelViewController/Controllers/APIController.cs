using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BachelorModelViewController.Models;
using BachelorModelViewController.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using BachelorModelViewController.Interfaces;

namespace BachelorModelViewController.Controllers
{
    public interface IAPIController
    {

    }

    [Produces("application/json")]
    [Route("api")]
    public class APIController : Controller, IAPIController
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMongoOperations _mongoOperations;

        public APIController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMongoOperations mongoOperations)
        {
            _context = context;
            _mongoOperations = mongoOperations;
            _userManager = userManager;
            _roleManager = roleManager;

        }


        // GET: api/get_mongo_collection/{collectionName}
        [Route("get_mongo_collection/{name}")]
        [HttpGet]
        public IActionResult GetMongoCollection(string name)
        {
            try
            {
                var bson = GetAllFromCollectionInternal(name);

                return Json(bson.Select(x => x.ToJson()));
            } catch (IndexOutOfRangeException e)
            {
                return Json(new { message = e.Message });
            }
        }
        private List<BsonDocument> GetAllFromCollectionInternal(string collectionName)
        {
            if (_mongoOperations.CollectionExists(collectionName))
            {
                return _mongoOperations.GetAllFromCollection(collectionName).Result;
            } else
            {
                throw new IndexOutOfRangeException("The collection you are looking for, doesn't exist");
            }
        }


        // GET: api/get_mongo_collection_from/{collectionName}/{unixTimeInSeconds}
        [Route("get_mongo_collection_from/{name}/{time}")]
        [HttpGet]
        public IActionResult GetAllFromCollectionFrom(string name, int time)
        {
            try
            {
                var bson = GetAllFromCollectionFromInternal(name, time);
                return Json(bson.Select(x => x.ToJson()));
            }
            catch (IndexOutOfRangeException e)
            {
                return Json(new { message = e.Message });
            }
        }
        private List<BsonDocument> GetAllFromCollectionFromInternal(string collectionName, int time)
        {
            if (_mongoOperations.CollectionExists(collectionName))
            {
                return _mongoOperations.GetAllFromCollectionFromTime(collectionName, time).Result;
            }
            else
            {
                throw new IndexOutOfRangeException("The collection you are looking for, doesn't exist");
            }
        }

        // GET: api/get_mongo_collection_filter/{collectionName}?filterByQuery
        [Route("get_mongo_collection_or_filter/{name}")]
        [HttpGet]
        public IActionResult GetAllFromCollectionByOrFilter(string name)
        {
            try
            {
                List<BsonDocument> bson;
                IQueryCollection filter = Request.Query;
                if (filter.Count() > 0)
                {
                    bson = GetAllFromCollectionByFilterInternal(name, filter, false);
                }
                else
                {
                    bson = GetAllFromCollectionInternal(name);
                }
                return Json(bson.Select(x => x.ToJson()));
            }
            catch (IndexOutOfRangeException e)
            {
                return Json(new { message = e.Message });
            }
        }

        // GET: api/get_mongo_collection_filter/{collectionName}?filterByQuery
        [Route("get_mongo_collection_and_filter/{name}")]
        [HttpGet]
        public IActionResult GetAllFromCollectionByAndFilter(string name)
        {
            try
            {
                List<BsonDocument> bson;
                IQueryCollection filter = Request.Query;
                if (filter.Count() > 0)
                {
                    bson = GetAllFromCollectionByFilterInternal(name, filter, true);
                }
                else
                {
                    bson = GetAllFromCollectionInternal(name);
                }
                return Json(bson.Select(x => x.ToJson()));
            }
            catch (IndexOutOfRangeException e)
            {
                return Json(new { message = e.Message });
            }
        }

        private List<BsonDocument> GetAllFromCollectionByFilterInternal(string collectionName, IQueryCollection jsonStringFilter, bool andFilter)
        {
            if (_mongoOperations.CollectionExists(collectionName))
            {
                if(andFilter)
                {
                    return _mongoOperations.GetAllFromCollectionByAndFilter(collectionName, jsonStringFilter).Result;
                } else
                {
                    return _mongoOperations.GetAllFromCollectionByOrFilter(collectionName, jsonStringFilter).Result;
                }
            }
            else
            {
                throw new IndexOutOfRangeException("The collection you are looking for, doesn't exist");
            }
        }


    }
}
