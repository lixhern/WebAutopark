using WebAutopark.Models;

namespace WebAutopark.Data.Repositories.IRepositories
{
    public interface IVehicleRepository : IRepository<Vehicle>, IUpdatable<Vehicle>
    {
        Task<Vehicle> GetByRegistrationNumberAsync(string regNumber);

    }
}
