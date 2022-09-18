using Calabonga.OperationResults;
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

    public async Task<OperationResult<IEnumerable<TestsModel>>> GetAllAsync()
    {
        var result = OperationResult.CreateResult<IEnumerable<TestsModel>>();

        try
        {
            result.Result = await _collection.Find(new BsonDocument()).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e); // TODO Add logger
            result.AddError(e);
        }

        return result;
    }

    public async Task<OperationResult<TestsModel>> GetAsync(string id)
    {
        var result = OperationResult.CreateResult<TestsModel>();
        try
        {
            result.Result = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result.AddError(e);
        }

        return result;
    }

    public async Task<OperationResult<TestsModel>> CreateAsync(TestsModel item)
    {
        var result = OperationResult.CreateResult<TestsModel>();

        try
        {
            await _collection.InsertOneAsync(item);
            result.Result = item;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result.AddError(e);
        }

        return result;
    }

    public async Task<OperationResult<TestsModel>> UpdateAsync(TestsModel item)
    {
        var result = OperationResult.CreateResult<TestsModel>();

        try
        {
            await _collection.ReplaceOneAsync(x => x.Id == item.Id, item);
            result.Result = item;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result.AddError(e);
        }

        return result;
    }

    public async Task<OperationResult<string>> DeleteAsync(string id)
    {
        var result = OperationResult.CreateResult<string>();

        try
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
            result.Result = id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result.AddError(e);
        }

        return result;
    }
}