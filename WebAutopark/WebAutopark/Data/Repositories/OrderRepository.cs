using Dapper;
using WebAutopark.Models;

namespace WebAutopark.Data.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly DapperDbContext _dbContext;

        public OrderRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IEnumerable<Order>> GetAll()
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = "SELECT * FROM Orders";
                var orders = connection.QueryAsync<Order>(query);
                return orders;
            }
        }

        public async Task<Order> Get(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"SELECT * FROM Orders
                    WHERE OrderId = {id}";
                var order = await connection.QuerySingleAsync<Order>(query);
                return order;
            }
        }

        public async Task<Order> Get(Order item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"SELECT * FROM Orders
                    WHERE OrderId = {item.OrderId}";
                var order = await connection.QuerySingleAsync<Order>(query);
                return order;
            }
        }

        public async Task Create(Order item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"INSERT INTO ORDERS
                    (VehicleId, Date)
                    VALUES
                    @VehicleId,
                    @Date";
                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task Update(Order item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"UPDATE Orders
                    VehicleId = @VehicleId,
                    Date = @Date";
                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string quary = $@"DELETE FROM Orders
                    WHERE OrderId = {id}";

                await connection.ExecuteAsync(quary);
            }
        }

        public async Task Delete(Order item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string quary = $@"DELETE FROM Orders
                    WHERE OrderId = {item.OrderId}";

                await connection.ExecuteAsync(quary);
            }
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

    }
}
