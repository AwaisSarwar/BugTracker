using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BugTracker.Repositories.Models
{
    public class Bug : IModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Severity Severity{ get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReportedOn { get; set; }
        public Status Status { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ClosedOn { get; set; }
        public string ReportedBy { get; set; }
        public string AssignedTo { get; set; }
    }
}
