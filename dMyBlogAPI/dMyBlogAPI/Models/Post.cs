using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("title")]
        public string title { get; set; }

        [BsonElement("date")]
        public string date { get; set; }

        [BsonElement("excerpt")]
        public string excerpt { get; set; }

        [BsonElement("author")]
        public string author { get; set; }

        [BsonElement("postFileName")]
        public string postFileName { get; set; }

        [BsonElement("imageFileName")]
        public string imageFileName { get; set; }


        [BsonElement("status")]
        public string status  { get; set; }


        [BsonElement("encrypt")]
        public bool encrypt { get; set; }

        [BsonElement("shareCode")]
        public string shareCode { get; set; }


        [BsonIgnore]
        public InteractionTypeResult interactions { get; set; }

    }
}
