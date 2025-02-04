namespace WebAutopark.Data.Repositories.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAll(); 
        Task<T> Get(int id);
        Task<int> Create(T item);
        Task Delete(int id);
        Task Delete(T item); // this too
    }
}
