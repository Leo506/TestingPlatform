namespace Server.Definitions.Database.Repositories;

public interface IRepository<T>
{
    // TODO Add wrapper for results
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> Get(string id);

    Task Create(T item);

    Task Update(T item);

    Task Delete(T item);
}