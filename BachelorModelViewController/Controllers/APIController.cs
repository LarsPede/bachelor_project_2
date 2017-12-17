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
using MongoDB.Bson.Serialization;

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


        // GET: api/get_all_channel_data/{channelName}/{userToken}
        [Route("get_all_channel_data/{name}/{token}")]
        [HttpGet]
        public IActionResult GetMongoCollection(string name, string token = null)
        {

            try
            {
                UserAuthenticatedToChannel(token, name);
                var bson = GetAllFromCollectionInternal(name);

                return Json(bson.Select(x => BsonSerializer.Deserialize<Object>(x)));
            } catch (Exception e)
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
                throw new IndexOutOfRangeException("The supplier hasn't made a first upload. The data-collection you are looking for does not exist.");
            }
        }
        
        // GET: api/get_mongo_collection_from/{collectionName}/{unixTimeInSeconds}/{token}
        [Route("get_mongo_collection_from/{name}/{time}/{token}")]
        [HttpGet]
        public IActionResult GetAllFromCollectionFrom(string name, int time, string token = null)
        {
            try
            {
                UserAuthenticatedToChannel(token, name);
                var bson = GetAllFromCollectionFromInternal(name, time);
                return Json(bson.Select(x => BsonSerializer.Deserialize<Object>(x)));
            }
            catch (Exception e)
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
                throw new IndexOutOfRangeException("The supplier hasn't made a first upload. The data-collection you are looking for does not exist.");
            }
        }

        // GET: api/get_mongo_collection_filter/{collectionName}/{token}?filterByQuery
        [Route("get_mongo_collection_or_filter/{name}/{token}")]
        [HttpGet]
        public IActionResult GetAllFromCollectionByOrFilter(string name, string token = null)
        {
            try
            {
                UserAuthenticatedToChannel(token, name);
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
                return Json(bson.Select(x => BsonSerializer.Deserialize<Object>(x)));
            }
            catch (Exception e)
            {
                return Json(new { message = e.Message });
            }
        }

        // GET: api/get_mongo_collection_filter/{collectionName}/{token}?filterByQuery
        [Route("get_mongo_collection_and_filter/{name}/{token}")]
        [HttpGet]
        public IActionResult GetAllFromCollectionByAndFilter(string name, string token = null)
        {
            try
            {
                UserAuthenticatedToChannel(token, name);
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
                return Json(bson.Select(x => BsonSerializer.Deserialize<Object>(x)));
            }
            catch (Exception e)
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
                throw new IndexOutOfRangeException("The supplier hasn't made a first upload. The data-collection you are looking for does not exist.");
            }
        }

        // POST: api/post_to_channel/{channelName}/{userToken}
        [Route("post_to_channel/{name}/{token}")]
        [HttpPost]
        public IActionResult PostSingleEntry(string name, string token, [FromBody] dynamic value)
        {
            var start = "here";
            try
            {
                UserPostAuthenticatedToChannel(token, name);
                
                if (value == null)
                {
                    return BadRequest();
                }
                BsonDocument bsonItem = value.ToBsonDocument;
                if (PostSingleEntryInternal(name, bsonItem))
                {
                    return Json(new { success = true });
                }
                return Json(new { error = true });
            }
            catch (Exception e)
            {
                return Json(new { message = e.Message });
            }
        }
        private bool PostSingleEntryInternal(string collectionName, BsonDocument item)
        {
            if (_mongoOperations.CollectionExists(collectionName))
            {
                _mongoOperations.AddToCollection(collectionName, item);
                return true;
            }
            else
            {
                throw new IndexOutOfRangeException("The supplier hasn't made a first upload. The data-collection you are looking for does not exist.");
            }
        }

        private void UserAuthenticatedToChannel(string token, string channelName)
        {
            try
            {
                var channel = _context.Channels.Where(x => x.Name == channelName).First();
                var accessRestriction = _context.AccessRestrictions.Where(x => x.Id == channel.AccessRestrictionId).First();
                if (accessRestriction.GroupRestricted == true || accessRestriction.UserRestricted == true)
                {
                    var user = _context.Users.Where(x => x.Token == Guid.Parse(token)).First();
                    if (accessRestriction.GroupRestricted == true)
                    {
                        var association = _context.Associations.Where(x => x.UserId == user.Id && x.GroupId == channel.GroupId && x.Role != null).FirstOrDefault();
                        if (association == null)
                        {
                            throw new Exception();
                        }
                        if (accessRestriction.AccessLevel == _roleManager.FindByNameAsync("Administrator").Result
                            && _roleManager.FindByNameAsync("Administrator").Result != association.Role)
                        {
                            throw new Exception();
                        }
                        if (accessRestriction.AccessLevel == _roleManager.FindByNameAsync("Supplier").Result
                            && (_roleManager.FindByNameAsync("Administrator").Result != association.Role
                                || _roleManager.FindByNameAsync("Supplier").Result != association.Role))
                        {
                            throw new Exception();
                        }
                    }

                }
            }
            catch (InvalidOperationException)
            {
                throw new KeyNotFoundException("You are either trying to access a channel that doesn't exist, or your user-token is wrong.");
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("The channel has not been created correctly. Ask info@cupid.cupid for assistance.");
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this channel.");
            }
        }
        private void UserPostAuthenticatedToChannel(string token, string channelName)
        {
            try
            {
                var channel = _context.Channels.Where(x => x.Name == channelName).First();
                var user = _context.Users.Where(x => x.Token == Guid.Parse(token)).First();
                if (channel.UserId != null)
                {
                    if (channel.UserId != user.Id)
                    {
                        throw new UnauthorizedAccessException();
                    }
                } else
                {
                    var association = _context.Associations.Where(x => x.UserId == user.Id && x.GroupId == channel.GroupId).First();
                    if (association.RoleId != _roleManager.FindByNameAsync("Administrator").Result.Id && association.RoleId != _roleManager.FindByNameAsync("Supplier").Result.Id)
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw new KeyNotFoundException("You are either trying to access a channel that doesn't exist, or your user-token is wrong.");
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("The channel has not been created correctly. Ask info@cupid.cupid for assistance.");
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this channel.");
            }
        }

    }
}
