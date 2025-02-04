namespace WebAutopark.Data.Repositories.Interfaces
{
    public interface IUpdatable<T>
    {
        Task Update(T item);
    }
}
