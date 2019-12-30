using dMyBlogAPI.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Services
{
    public class InteractionService
    {

        private readonly IMongoCollection<Interaction> _interactions;
        IMongoDatabase _db;
        public InteractionService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("PostStoreDb"));
            this._db = client.GetDatabase("dMyBlogCollection");
            _interactions = this._db.GetCollection<Interaction>("Interactions");
        }

        public InteractionTypeResult Get(string postId, string userId)
        {
            InteractionTypeResult res = _interactions.AsQueryable().Where(i => i.postId == postId).GroupBy(g => new { g.postId }).Select(n => new InteractionTypeResult
            {
                postId = n.Key.postId,
                LikeCount = n.Count(c => c.type == 1),
                UnlikeCount = n.Count(c => c.type == 0)
            }).FirstOrDefault();
            if (res == null)
                res = new InteractionTypeResult();
            if ((res.LikeCount != 0 || res.UnlikeCount != 0) && !string.IsNullOrEmpty(userId))
            {

                Interaction userInteraction = _interactions.AsQueryable().Where(i => i.postId == postId && i.userId == userId).FirstOrDefault();
                if (userInteraction != null && !string.IsNullOrEmpty(userInteraction.id))
                {
                    res.isUserLike = userInteraction.type == 1;
                    res.isUserUnlike = userInteraction.type == 0;
                    res.userInteractionId = userInteraction.id;
                }

            }
            return res;
        }

        public Interaction Get(string id)
        {
            return _interactions.Find<Interaction>(i => i.id == id).FirstOrDefault();
        }

        public Interaction Create(Interaction interaction)
        {
            _interactions.InsertOne(interaction);
            return interaction;
        }

        public void Update(string id, Interaction interactionIn)
        {
            _interactions.ReplaceOne(i => i.id == id, interactionIn);
        }

        public void Remove(Interaction interactionIn)
        {
            _interactions.DeleteOne(i => i.id == interactionIn.id);
        }

        public void Remove(string id)
        {
            _interactions.DeleteOne(i => i.id == id);
        }
    }
}