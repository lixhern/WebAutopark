using WebAutopark.Models;

namespace WebAutopark.Data.Repositories.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle>, IUpdatable<Vehicle>
    {
        Task<Vehicle> GetByRegNumber(string regNumber);
        
    }
}
