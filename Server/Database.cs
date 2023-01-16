using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Database
    {
        private MySqlConnection _connection;
        
        public Database(string server, string databaseName, string userName, string password)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                { "Server", server },
                { "Database", databaseName },
                { "UID", userName },
                { "password", password }
            };
            _connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
        }

        public void Open()
        {
               
            _connection.Open();
            
        }

        public void Close()
        {
            _connection.Close();
        }

        public async Task<List<Dictionary<string, object>>> ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var result = new List<Dictionary<string, object>>();
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        result.Add(row);
                    }
                    return result;
                }
            }
        }
        public async Task<int> ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                return await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<object> ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.Select(p => new MySqlParameter(p.Key, p.Value)).ToArray());
                }

                return await command.ExecuteScalarAsync();
            }
        }
        public async Task<DataTable> ExecuteDataTable(string query, Dictionary<string, object> parameters = null)
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.Select(p => new MySqlParameter(p.Key, p.Value)).ToArray());
                }

                var dataTable = new DataTable();
                dataTable.Load(await command.ExecuteReaderAsync());
                return dataTable;
            }
        }
    }

 }
