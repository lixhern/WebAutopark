using Dapper;
using WebAutopark.Data.Repositories.Interfaces;
using WebAutopark.Models;

namespace WebAutopark.Data.Repositories.Implementations
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly DapperDbContext _dbContext;

        public ComponentRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WebAutopark.Models.Component>> GetAll()
        {
            using var connection = _dbContext.GetConnection();
            string query = "SELECT * FROM Components";
            return await connection.QueryAsync<Component>(query);
        }

        public async Task<WebAutopark.Models.Component> Get(int id)
        {
            using var connection = _dbContext.GetConnection();
            string query = "SELECT * FROM Components WHERE ComponentId = @Id";
            return await connection.QuerySingleOrDefaultAsync<Component>(query, new { Id = id });
        }

        public async Task<int> Create(WebAutopark.Models.Component item)
        {
            using var connection = _dbContext.GetConnection();
            string query = @"INSERT INTO Components (Name) 
                             OUTPUT INSERTED.ComponentId 
                             VALUES (@Name)";
            return await connection.QuerySingleAsync<int>(query, item);
        }

        public async Task Delete(int id)
        {
            using var connection = _dbContext.GetConnection();
            string query = "DELETE FROM Components WHERE ComponentId = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task Delete(WebAutopark.Models.Component item)
        {
            await Delete(item.ComponentId);
        }
    }
}
