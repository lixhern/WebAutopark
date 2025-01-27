using Dapper;
using WebAutopark.Models;

namespace WebAutopark.Data.Repositories
{
    public class ComponentRepository : IRepository<Component>
    {
        private readonly DapperDbContext _dbContext;

        public ComponentRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Component>> GetAll()
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = @"SELECT * FROM Components";
                var components = await connection.QueryAsync<Component>(query);
                return components;
            }
        }

        public async Task<Component> Get(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"SELECT * FROM Components
                    WHERE ComponentId = {id}";
                var component = await connection.QuerySingleAsync<Component>(query);
                return component;
            }
        }

        public Task<Component> Get(Component item) //dell this
        {
            throw new NotImplementedException();
        }

        public async Task Create(Component item)
        {
            using(var connection = _dbContext.GetConnection())
            {
                string query = $@"INSERT INTO Components
                    (Name)
                    @Name";

                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task Update(Component item)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"UPDATE Components
                    SET
                    Name = @Name
                    WHERE ComponentId = @ComponentId";

                await connection.ExecuteAsync(query, item);
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM Components WHERE ComponentId = {id}";

                await connection.ExecuteAsync(query);
            }
        }

        public async Task Delete(Component item) 
        {
            using (var connection = _dbContext.GetConnection())
            {
                string query = $@"DELETE FROM Components WHERE ComponentId = {item.ComponentId}";

                await connection.ExecuteAsync(query);
            }
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

    }
}
