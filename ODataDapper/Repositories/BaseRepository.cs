using Dapper;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ODataDapper.Repositories
{
    /// <summary>
    /// The basic repository class for dapper database connections.
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Queries the first or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL to be queried.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Returns result of query.
        /// </returns>
        protected T QueryFirstOrDefault<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }

        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL to be queried.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Returns results of query.
        /// </returns>
        protected async Task<List<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<T>(sql, parameters)).ToList();
            }
        }

        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL to be executed.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Returns result of sql execution.
        /// </returns>
        protected int Execute(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }

        // Other Helpers...

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>
        /// Returns the created connection.
        /// </returns>
        private IDbConnection CreateConnection()
        {
            //Gets the connection string
            string connectionString = ConfigurationManager.ConnectionStrings["SimpleDB"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            // Properly initializing the connection.
            return connection;
        }
    }
}