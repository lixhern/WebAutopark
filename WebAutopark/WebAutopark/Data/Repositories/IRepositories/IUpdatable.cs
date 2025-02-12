namespace WebAutopark.Data.Repositories.IRepositories
{
    public interface IUpdatable<T>
    {
        Task UpdateAsync(T item);
    }
}
