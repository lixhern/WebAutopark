using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace WebAutopark.Data
{
    public class DapperDbContext
    {
        private readonly string _connectionString;
        private IDbConnection _connection;
        //IDBTRansaction

        public DapperDbContext(string connectionString)
        {
            _connectionString = connectionString;
            //_connection.Open();
        }

        public IDbConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
            /*if( _connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }

            return _connection;*/
        }


    }
}
