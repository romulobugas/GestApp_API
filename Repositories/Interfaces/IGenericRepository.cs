public interface IGenericRepository
{
    Task AddAsync<T>(T entity) where T : class;    
    Task UpdateAsync<T>(T entity) where T : class;
    Task DeleteAsync<T>(int id) where T : class;
    Task<bool> SaveChangesAsync();
}
