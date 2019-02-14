using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BugTracker.Repositories.Models
{
    public class User : IModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
    }
}
