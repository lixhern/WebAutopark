using Dapper;
using WebAutopark.Data.Repositories.IRepositories;
using WebAutopark.Models;

namespace WebAutopark.Data.Repositories.Implementations
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private DapperDbContext _dbContext;

        public VehicleTypeRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<VehicleType>> GetAllAsync()
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = @"SELECT * FROM VehicleTypes";
                var vehTypes = await connection.QueryAsync<VehicleType>(query);
                return vehTypes;
            }
        }

        public async Task<VehicleType> GetAsync(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = @$"SELECT * FROM VehicleTypes
                    WHERE VehicleTypeId = {id}";
                var vehType = await connection.QuerySingleOrDefaultAsync<VehicleType>(query);
                return vehType;
            }
        }

        public async Task<int> CreateAsync(VehicleType item) 
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = @"INSERT INTO VehicleTypes
                    (Name, TaxCoefficient)
                    OUTPUT INSERTED.VehicleTypeId
                    VALUES
                    (@Name, @TaxCoefficient)";

                var result = await connection.QuerySingleAsync<int>(query, item);
                return result;
            }
        }

        public async Task UpdateAsync(VehicleType item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"UPDATE VehicleTypes
                    SET
                    Name = @Name,
                    TaxCoefficient = @TaxCoefficient
                    WHERE VehicleTypeId = @VehicleTypeId";

                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM VehicleTypes
                    WHERE VehicleTypeId = {id}";

                await connection.ExecuteAsync(query);
            }
        }

        public async Task DeleteAsync(VehicleType item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM VehicleTypes
                    WHERE VehicleTypeId = {item.VehicleTypeId}";

                await connection.ExecuteAsync(query);
            }
        }

        public async Task Save()
        {
            throw new NotImplementedException();
        }

    }
}
