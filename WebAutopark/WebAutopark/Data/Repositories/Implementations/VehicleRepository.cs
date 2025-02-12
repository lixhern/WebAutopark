using Dapper;
using WebAutopark.Data.Repositories.IRepositories;
using WebAutopark.Models;

namespace WebAutopark.Data.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private DapperDbContext _dbContext;

        public VehicleRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = @"SELECT *  FROM Vehicles";
                var vehicles = await connection.QueryAsync<Vehicle>(query);
                return vehicles;
            }
        }

        public async Task<Vehicle> GetAsync(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $"SELECT * FROM Vehicles WHERE VehicleId = {id}";

                var vehicle = await connection.QuerySingleOrDefaultAsync<Vehicle>(query);

                return vehicle;
            }
        }


        public async Task<Vehicle> GetByRegistrationNumberAsync(string regNumber)
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = $@"
                    SELECT * FROM Vehicles WHERE RegistrationNumber = '{regNumber}'
                    ";

                var vehicle = await connection.QuerySingleOrDefaultAsync<Vehicle>(query);

                return vehicle;
            }
        }

        public async Task<int> CreateAsync(Vehicle item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"INSERT INTO Vehicles
                    (vehicleTypeId, model, registrationNumber, weight, year, mileage, color, fuelConsumption)
                    OUTPUT INSERTED.VehicleId
                    VALUES
                    (@VehicleTypeId, @Model, @RegistrationNumber, @Weight, @Year, @Mileage, @Color, @FuelConsumption)";

                var result = await connection.QuerySingleAsync<int>(query, item);
                return result;
            }
        }

        public async Task UpdateAsync(Vehicle item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"UPDATE Vehicles
                    SET
                    VehicleTypeId = @VehicleTypeId,
                    Model = @Model,
                    RegistrationNumber = @RegistrationNumber,
                    Weight = @Weight,
                    Year = @Year,
                    Mileage = @Mileage,
                    Color = @Color,
                    FuelConsumption = @FuelConsumption
                    WHERE VehicleId = @VehicleId";

                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM Vehicles WHERE VehicleId = {id}";

                await connection.ExecuteAsync(query);
            }
        }

        public async Task DeleteAsync(Vehicle item)
        {
            using (var connection = _dbContext.GetConnection()) 
            {
                string query = $@"DELETE FROM Vehicles 
                    WHERE 
                    VehicleId = {item.VehicleId}";

                await connection.ExecuteAsync(query);
            }
        }


        public Task Save()
        {
            throw new NotImplementedException();
        }


    }
}
