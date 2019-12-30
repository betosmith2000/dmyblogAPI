using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Models
{
    public class Interaction
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("postId")]
        public string postId { get; set; }

        [BsonElement("userId")]
        public string userId { get; set; }

        [BsonElement("type")]
        public int type { get; set; }

      
    }

    public class InteractionTypeResult {
        public string userInteractionId { get; set; }
        public string postId { get; set; }
        
        public bool isUserLike { get; set; }
        public bool isUserUnlike { get; set; }
        public Int64 LikeCount { get; set; }
        public Int64 UnlikeCount { get; set; }
    }
}
