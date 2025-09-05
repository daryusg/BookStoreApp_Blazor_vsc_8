namespace BookStoreApp.API.Repositories;

public interface IGenericRepository<T> where T : class //cip...64
{
    Task<T> GetAsync(int? id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
