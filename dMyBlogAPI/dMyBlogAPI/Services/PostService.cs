using dMyBlogAPI.Helpers;
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

    //create index for search
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;
        IMongoDatabase _db;
        public PostService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("PostStoreDb"));
            this._db = client.GetDatabase("dMyBlogCollection");
            _posts = this._db.GetCollection<Post>("Posts");
        }

        public Paginate<Post> Get(PaginateParams p, string searchTerm)
        {
            Paginate<Post> res = new Paginate<Post>();
            res.PageSize = p.PageSize;
            res.PageNumber = p.PageNumber;
            int skip = p.PageNumber * p.PageSize;
            var builder = Builders<Post>.Filter;
            if (string.IsNullOrEmpty(searchTerm))
            {
                res.TotalRegs = _posts.CountDocuments(f => f.status == "2" );
                res.Data = _posts.AsQueryable().Where(f => f.status == "2" ).OrderByDescending(o=> o.date).Skip(skip).Take(p.PageSize).ToList();
            }
            else
            {
                searchTerm = searchTerm.ToLower();
                res.TotalRegs = _posts.CountDocuments(f => f.status == "2" && (f.title.ToLower().Contains(searchTerm)|| f.author.ToLower().Contains(searchTerm)));
                res.Data = _posts.AsQueryable().Where(f => f.status == "2" && (f.title.ToLower().Contains(searchTerm) || f.author.ToLower().Contains(searchTerm))).OrderByDescending(o=> o.date).Skip(skip).Take(p.PageSize).ToList();
            }
            
            return res;
        }


        public PostStatistics GetStatistics()
        {
            PostStatistics res= new PostStatistics();
            var builder = Builders<Post>.Filter;
            
                res.TotalPublicPosts = _posts.CountDocuments(f => f.status == "2");
                res.TotalUsers = _posts.AsQueryable().Select(e=> e.author).Distinct().Count();
            
            return res;
        }

        public Post Get(string id)
        {
            return _posts.Find<Post>(p => p.id == id).FirstOrDefault();
        }

        public Post Create(Post post)
        {
            _posts.InsertOne(post);
            return post;
        }

        public void Update(string id, Post postIn)
        {
            _posts.ReplaceOne(post => post.id == id, postIn);
        }

        public void Remove(Post postIn)
        {
            _posts.DeleteOne(book => book.id == postIn.id);
        }

        public void Remove(string id)
        {
            _posts.DeleteOne(post => post.id == id);
        }
    }
}