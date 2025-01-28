using Dapper;
using WebAutopark.Models;
//можно попробовать сделать rollback

namespace WebAutopark.Data.Repositories
{
    public class VehicleRepository : IRepository<Vehicle>
    {
        private DapperDbContext _dbContext;

        public VehicleRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = @"SELECT *  FROM Vehicles"; //join vehType mbmbmbmmbmb
                var vehicles = await connection.QueryAsync<Vehicle>(query);
                return vehicles;
            }
        }

        public async Task<Vehicle> Get(int id)
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = $"SELECT * FROM Vehicles WHERE VehicleId = {id}";
                
                var vehicle = (await connection.QuerySingleOrDefaultAsync<Vehicle>(query));
                
                return vehicle;
            }
        }

        public async Task<Vehicle> Get(Vehicle item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $"SELECT * FROM Vehicles WHERE Model = {item.Model} AND RegistrationNumber = {item.RegistrationNumber}";
                
                var vehicle = (await connection.QuerySingleOrDefaultAsync<Vehicle>(query));
                
                return vehicle;
            }
        }


        public async Task<int> Create(Vehicle item)
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

        public async Task Update(Vehicle item)
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

        public async Task Delete(int id)
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM Vehicles WHERE VehicleId = {id}";

                await connection.ExecuteAsync(query);
            }
        }

        public async Task Delete(Vehicle item)
        {
            using(var connection = _dbContext.GetConnection()) //search by id mbmb
            {
                string query = $@"DELETE FROM Vehicles 
                    WHERE 
                    Model = {item.Model}
                    AND 
                    RegistrationNumber = {item.RegistrationNumber}";

                await connection.ExecuteAsync(query);
            }
        }


        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
