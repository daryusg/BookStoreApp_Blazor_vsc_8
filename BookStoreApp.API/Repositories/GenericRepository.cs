using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class //cip...64
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper; //cip...65
    private readonly DbSet<T> _db;

    public GenericRepository(BookStoreDbContext context, IMapper mapper) //cip...65
    {
        _context = context;
        this._mapper = mapper;
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
        _context.Remove(entity);
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

    public async Task<VirtualiseResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParams) where TResult : class  //cip...65
    {
        var totalSize = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>()
            .Skip(queryParams.StartIndex)
            .Take(queryParams.PageSize)
            //.Select(e => (TResult)(object)e) // This cast assumes T and TResult are compatible copilot
            .ProjectTo<TResult>(_mapper.ConfigurationProvider) // Requires AutoMapper
            .ToListAsync();

        //Console.WriteLine($"Returning {items.Count} authors, TotalSize = {totalSize}"); //cip...72 chatgpt

        return new VirtualiseResponse<TResult>
        {
            Items = items,
            TotalSize = totalSize
        };
    }
}