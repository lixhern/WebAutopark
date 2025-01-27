using Dapper;
using WebAutopark.Models;

namespace WebAutopark.Data.Repositories
{
    public class VehicleTypeRepository : IRepository<VehicleType>
    {
        private DapperDbContext _dbContext;

        public VehicleTypeRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<VehicleType>> GetAll()
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = @"SELECT * FROM VehicleTypes";
                var vehTypes = await connection.QueryAsync<VehicleType>(query);
                return vehTypes;
            }
        }

        public async Task<VehicleType> Get(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = @$"SELECT * FROM VehicleTypes
                    WHERE VehicleTypeId = {id}";
                var vehType = await connection.QuerySingleOrDefaultAsync<VehicleType>(query);
                return vehType;
            }
        }

        public async Task<VehicleType> Get(VehicleType item) //mb
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = @$"SELECT * FROM VehicleTypes
                    WHERE VehicleTypeId = {item.VehicleTypeId}";
                var vehType = await connection.QuerySingleOrDefaultAsync<VehicleType>(query);
                return vehType;
            }
        }

        public async Task Create(VehicleType item) //return id
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = @"INSERT INTO VehicleTypes
                    (Name, TaxCoefficient)
                    VALUES
                    (@Name, @TaxCoefficient)";

                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task Update(VehicleType item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"UPDATE VehicleTypes
                    SET
                    Name = @Name,
                    TaxCoefficient = @TaxCoefficient
                    WHERE VehicleTypeId = @VehicleTypeId AND Name = @Name AND TaxCoefficient = @TaxCoefficient";

                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task Delete(int id)
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM VehicleTypes
                    WHERE VehicleTypeId = {id}";

                await connection.ExecuteAsync(query);
            }
        }
        //stay item !!!!!
        public async Task Delete(VehicleType item) //maybe should delete this method, stay only by ID
        {
            using(var connection = _dbContext.GetConnection())
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
