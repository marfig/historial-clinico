using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public abstract class SqlHelperService
    {
        public async Task ExecuteNonQueryAsync(string sp_name, string conn_str, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(conn_str))
            {
                using (SqlCommand cmd = new SqlCommand(sp_name, con))
                {
                    cmd.CommandType = commandType;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<T>> ExecuteReaderToListAsync<T>(string sp_name, string conn_str, CommandType commandType, params SqlParameter[] parameters)
        {
            List<T> list = new List<T>();

            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                using (SqlCommand cmd = new SqlCommand(sp_name, conn))
                {
                    cmd.CommandType = commandType;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    conn.Open();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var props = typeof(T).GetProperties();

                                var newItem = Activator.CreateInstance<T>();

                                foreach (var prop in props)
                                {
                                    if (reader[prop.Name] != DBNull.Value)
                                    {
                                        var targetType = IsNullableType(prop.PropertyType) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;

                                        var propertyVal = Convert.ChangeType(reader[prop.Name], targetType);

                                        prop.SetValue(newItem, propertyVal);
                                    }
                                }

                                list.Add(newItem);
                            }
                        }
                    }
                }
            }

            return list;
        }

        public async Task<T> ExecuteReaderToSingleObjectAsync<T>(string sp_name, string conn_str, CommandType commandType, params SqlParameter[] parameters)
        {
            var newItem = Activator.CreateInstance<T>();

            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                using (SqlCommand cmd = new SqlCommand(sp_name, conn))
                {
                    cmd.CommandType = commandType;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    conn.Open();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var props = typeof(T).GetProperties();

                                foreach (var prop in props)
                                {
                                    if (reader[prop.Name] != DBNull.Value)
                                    {
                                        var targetType = IsNullableType(prop.PropertyType) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;

                                        var propertyVal = Convert.ChangeType(reader[prop.Name], targetType);

                                        prop.SetValue(newItem, propertyVal);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return newItem;
        }

        private bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}