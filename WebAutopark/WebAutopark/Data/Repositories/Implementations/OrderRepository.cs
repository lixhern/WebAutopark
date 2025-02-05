using Dapper;
using WebAutopark.Data.Repositories.Interfaces;
using WebAutopark.Models;
using WebAutopark.ViewModel;

namespace WebAutopark.Data.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperDbContext _dbContext;

        public OrderRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = "SELECT * FROM Orders";
                return await connection.QueryAsync<Order>(query);
            }
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllInDetails()
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = @"
                    SELECT oi.OrderId, v.Model, o.Date, c.Name, oi.Quantity FROM OrderItems oi
                    JOIN Orders o ON oi.OrderId = o.OrderId
                    Join Vehicles v ON o.VehicleId = v.VehicleId
                    JOIN Components c ON oi.ComponentId = c.ComponentId";
                var orders = await connection.QueryAsync<OrderViewModel>(query);
                return orders;
            }
        }

        public async Task<IEnumerable<OrderViewModel>> GetInDetails(int id)
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = $@"
                    SELECT oi.OrderId, v.Model, o.Date, c.Name, oi.Quantity FROM OrderItems oi
                    JOIN Orders o ON oi.OrderId = o.OrderId
                    Join Vehicles v ON o.VehicleId = v.VehicleId
                    JOIN Components c ON oi.ComponentId = c.ComponentId
                    WHERE oi.OrderId = {id}
                    ";

                var order = await connection.QueryAsync<OrderViewModel>(query);
                return order;
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

        public async Task<int> Create(Order item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"INSERT INTO ORDERS
                    (VehicleId, Date)
                    OUTPUT INSERTED.OrderId
                    VALUES
                    (@VehicleId, @Date)";
                var result = await connection.QuerySingleAsync<int>(query, item);
                return result;
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
