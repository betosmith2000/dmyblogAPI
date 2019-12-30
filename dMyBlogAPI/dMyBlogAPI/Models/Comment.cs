using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Models
{
    public class Comment
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("postId")]
        public string postId { get; set; }

        [BsonElement("userId")]
        public string userId { get; set; }

        [BsonElement("fileName")]
        public string fileName { get; set; }

        [BsonElement("date")]
        public DateTime date { get; set; }

        [BsonElement("comments")]
        public ICollection<Comment> Comments { get; set; }

        [BsonElement("rootId")]
        public string rootId { get; set; }

        [BsonElement("parentId")]
        public string parentId { get; set; }

    }
}
