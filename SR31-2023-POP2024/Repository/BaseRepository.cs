using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using Google.Protobuf.WellKnownTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SR31_2023_POP2024.Repository
{
    public abstract class BaseRepository
    {
        protected string connectionString = "Data Source=localhost;Initial Catalog=POP;Integrated Security=True;Trust Server Certificate=True";

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        protected List<T> ExecuteQuery<T>(string query, Func<SqlDataReader, T> mapFunction)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                using (var reader = command.ExecuteReader())
                {
                    var result = new List<T>();
                    while (reader.Read())
                    {
                        result.Add(mapFunction(reader));
                    }
                    return result;
                }
            }
        }
        public List<T> ExecuteQuery<T>(string query, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    var result = new List<T>();
                    while (reader.Read())
                    {
                        result.Add(mapFunction(reader));
                    }
                    return result;
                }
            }
        }


    }
}