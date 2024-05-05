using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class DatabaseHandler
    {
        public static void InsertAdmin(string connectionString, string name)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string queryString = "Insert Into Admin values(@name);";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@name", name);
                connection.Open();
                command.ExecuteNonQuery();
            }

        }
        public static void InsertCustomer(string connectionString, string name, string passport, string nationality)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string queryString = "Insert Into Customer values(@name,@passport,@nationality);";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@passport", passport);
                command.Parameters.AddWithValue("@nationality", nationality);
                connection.Open();
                command.ExecuteNonQuery();
            }

        }
        public static IEnumerable<string[]> ReadTable(string connectionString, string tableName)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string queryString = $"SELECT * FROM {tableName};";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string[] values = new string[reader.FieldCount];
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = reader[i].ToString()??"";
                        }
                        yield return values;
                    }
                }
            }

        }
        public static IEnumerable<Tuple<int,string,string,string>> ReadCustomers(string connectionString)
        {
            using(SqlConnection connection = new(connectionString))
            {
                string queryString = "SELECT * FROM Customer;";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new(reader.GetInt32(0),reader.GetString(1),reader.GetString(2),reader.GetString(3));
                    }
                }
            }
        }
    }
}
