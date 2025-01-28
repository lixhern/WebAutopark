namespace WebAutopark.Data.Repositories
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Get(T item);
        Task<int> Create(T item);
        Task Update(T item);
        Task Delete(int id);
        Task Delete(T item);
        Task Save();

    }
}
