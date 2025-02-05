using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace WebAutopark.Data
{
    public class DapperDbContext : IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public DapperDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
