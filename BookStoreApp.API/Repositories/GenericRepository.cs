using BookStoreApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class //cip...64
{
    private readonly BookStoreDbContext _context;
    private readonly DbSet<T> _db;

    public GenericRepository(BookStoreDbContext context)
    {
        _context = context;
        _db = _context.Set<T>();
    }

    public async Task<T> GetAsync(int? id)
    {
        if (id == null)
        {
            return null;
        }
        return await _db.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _db.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _db.AddAsync(entity);
        await SaveAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        // _db.Attach(entity); copilot
        // _context.Entry(entity).State = EntityState.Modified;
        _context.Update(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        _db.Remove(entity);
        await SaveAsync();
    }

    private async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }
}