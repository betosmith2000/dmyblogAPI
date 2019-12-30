using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Models
{
    public class ShareModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public string idSever { get; set; }

        [BsonElement("id")]
        public string id { get; set; }

        [BsonElement("source")]
        public string source { get; set; }

        [BsonElement("target")]
        public string target { get; set; }

        [BsonElement("fileName")]
        public string fileName { get; set; }

        [BsonElement("isNotified")]
        public bool isNotified { get; set; }

        [BsonElement("valid")]
        public bool valid { get; set; }

        [BsonElement("date")]
        public string date { get; set; }

        [BsonElement("name")]
        public string name { get; set; }


    }
}
