using WebAutopark.Models;

namespace WebAutopark.Data.Repositories.IRepositories
{
    public interface IVehicleTypeRepository : IRepository<VehicleType>, IUpdatable<VehicleType>
    {
    }
}
