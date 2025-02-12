using WebAutopark.Models;
using WebAutopark.ViewModel;

namespace WebAutopark.Data.Repositories.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<OrderViewModel>> GetInDetailsAsync(int id);
    }
}
