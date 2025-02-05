using WebAutopark.Models;
using WebAutopark.ViewModel;

namespace WebAutopark.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<OrderViewModel>> GetAllInDetails();
        Task<IEnumerable<OrderViewModel>> GetInDetails(int id);
    }
}
    