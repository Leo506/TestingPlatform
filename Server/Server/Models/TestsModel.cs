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

    public override bool Equals(object? obj)
    {
        if (obj is not TestsModel testsModel)
            return false;

        if (Questions.Count != testsModel.Questions.Count)
            return false;
        
        for (var i = 0; i < Questions.Count; i++)
        {
            if (!Questions[i].Equals(testsModel.Questions[i]))
                return false;
        }

        return true;
    }
}