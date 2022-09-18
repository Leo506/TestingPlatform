namespace Server.Definitions.Database.Repositories;

public interface IRepository<T>
{
    // TODO Add wrapper for results
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetAsync(string id);

    Task CreateAsync(T item);

    Task UpdateAsync(T item);

    Task DeleteAsync(T item);
}