using MongoDB.Bson;
using MongoDB.Driver;
using Server.Definitions.Database.Settings;
using Server.Models;

namespace Server.Definitions.Database.Repositories;

public class TestsRepository : IRepository<TestsModel>
{
    private readonly IMongoCollection<TestsModel> _collection;

    public TestsRepository(IMongoClient client, MongoSettings settings)
    {
        _collection = client.GetDatabase(settings.Database).GetCollection<TestsModel>(settings.Collection);
    }

    public async Task<IEnumerable<TestsModel>> GetAllAsync()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }

    public Task<TestsModel> Get(string id)
    {
        return Task.FromResult(_collection.AsQueryable().FirstOrDefault(x => x.Id == id));
    }

    public async Task Create(TestsModel item)
    {
        await _collection.InsertOneAsync(item);
    }

    public async Task Update(TestsModel item)
    {
        await _collection.ReplaceOneAsync(x => x.Id == item.Id, item);
    }

    public async Task Delete(TestsModel item)
    {
        await _collection.DeleteOneAsync(x => x.Id == item.Id);
    }
}