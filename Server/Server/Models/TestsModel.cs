﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

public class TestsModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    public List<Question> Questions = new List<Question>();
}