using Calabonga.OperationResults;

namespace Server.Definitions.Database.Repositories;

public interface IRepository<T>
{
    Task<OperationResult<IEnumerable<T>>> GetAllAsync();

    Task<OperationResult<IEnumerable<T>>> GetAllAsync(string userId);

    Task<OperationResult<T>> GetAsync(string id);

    Task<OperationResult<T>> CreateAsync(T item);

    Task<OperationResult<T>> UpdateAsync(T item);

    Task<OperationResult<string>> DeleteAsync(string id);
}