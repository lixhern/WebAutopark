using WebAutopark.Models;

namespace WebAutopark.Data.Repositories.Interfaces
{
    public interface IVehicleTypeRepository : IRepository<VehicleType>, IUpdatable<VehicleType>
    {
    }
}
