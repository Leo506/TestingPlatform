using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

public class TestsModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string Id { get; set; }
    
    [BsonIgnoreIfNull]
    public string UserId { get; set; }
    
    public List<Question> Questions { get; set; } = new();
}