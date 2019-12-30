using dMyBlogAPI.Helpers;
using dMyBlogAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Services
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_AllowOrigins")]
    public class CommentService
    {

        private readonly IMongoCollection<Comment> _comments;
        IMongoDatabase _db;
        public CommentService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("PostStoreDb"));
            this._db = client.GetDatabase("dMyBlogCollection");
            _comments = this._db.GetCollection<Comment>("Comments");
        }

        public Paginate<Comment> Get(PaginateParams p, string postId)
        {
            Paginate<Comment> res = new Paginate<Comment>();
            res.PageSize = p.PageSize;
            res.PageNumber = p.PageNumber;
            int skip = p.PageNumber * p.PageSize;
            var builder = Builders<Comment>.Filter;

            
            res.TotalRegs = _comments.CountDocuments(f => f.postId == postId);
            res.Data = _comments.AsQueryable().Where(f => f.postId == postId).Skip(skip).Take(p.PageSize).ToList();
            return res;
        }


        public Comment Get(string id)
        {
            return _comments.Find<Comment>(i => i.id == id).FirstOrDefault();
        }

        public Comment Create(Comment comment)
        {
            _comments.InsertOne(comment);
            return comment;
        }

        public void Update(string id, Comment commentIn)
        {
            _comments.ReplaceOne(i => i.id == id, commentIn);
        }

        public void Remove(Comment commentIn)
        {
            _comments.DeleteOne(i => i.id == commentIn.id);
        }

        public void Remove(string id)
        {
            _comments.DeleteOne(i => i.id == id);
        }
    }
}