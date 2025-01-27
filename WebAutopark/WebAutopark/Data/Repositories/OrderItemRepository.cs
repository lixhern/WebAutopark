using Dapper;
using WebAutopark.Models;

namespace WebAutopark.Data.Repositories
{
    public class OrderItemRepository : IRepository<OrderItem>
    {
        private readonly DapperDbContext _dbContext;

        public OrderItemRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderItem>> GetAll()
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = "SELECT * FROM OrderItems";
                var orderItems = await connection.QueryAsync<OrderItem>(query); 
                return orderItems;
            }
        }

        public async Task<OrderItem> Get(int id)
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = $@"SELECT * FROM OrderItems 
                    WHERE OrderItemId = {id}";
                var orderItem = await connection.QuerySingleAsync<OrderItem>(query);
                return orderItem;
            }
        }

        public async Task<OrderItem> Get(OrderItem item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"SELECT * FROM OrderItems 
                    WHERE OrderId = {item.OrderId}
                    AND ComponentId = {item.ComponentId}";
                var orderItem = await connection.QuerySingleAsync<OrderItem>(query);
                return orderItem;
            }
        }

        public async Task Create(OrderItem item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"INSERT INTO OrderItems
                    (OrderId, ComponentId, Quantity)
                    VALUES
                    @OrderId,
                    @ComponentId,
                    @Quantity";

                await connection.ExecuteAsync(query);
            }
        }

        public async Task Update(OrderItem item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"UPDATE OrderITems
                    OrderId = @OrderId,
                    ComponentId = @ComponentId,
                    Quantity = @Quantity";

                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM OrderItems WHERE OrderItemId = {id}";

                await connection.ExecuteAsync(query);
            }
        }

        public async Task Delete(OrderItem item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM OrderItems 
                    WHERE 
                    OrderId = {item.OrderId},
                    ComponentId = {item.ComponentId}";

                await connection.ExecuteAsync(query);
            }
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }


    }
}
