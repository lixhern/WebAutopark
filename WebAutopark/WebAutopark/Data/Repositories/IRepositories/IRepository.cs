namespace WebAutopark.Data.Repositories.IRepositories
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<int> CreateAsync(T item);
        Task DeleteAsync(int id);
        Task DeleteAsync(T item); // this too
    }
}
