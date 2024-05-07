using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class DatabaseHandler
    {
        public static void ChangeCustomerNameById(string connectionString,string newName,int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string queryString = "Update Customer Set Name = @Name where ID = @Id";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", newName);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public static void ChangeFlightTakeOffDateByFnum(string connectionString,DateTime newTime, string fnum)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string queryString = "Update Flight Set TakeOffDate = @newTime where Fnum = @fnum";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@fnum", fnum);
            command.Parameters.AddWithValue("@newTime", newTime);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public static void DeleteAircraftById(string connectionString, int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string queryString = "DELETE FROM Aircraft WHERE ID = @Id;";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public static void InsertInfant(string connectionString,string name,int companionId,DateTime birthDate,string fnum)
        {
            using SqlConnection connection = new(connectionString);
            string queryString = "Insert Into Infant values(@name,@companionId,@birthDate,@fnum);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@companionId", companionId);
            command.Parameters.AddWithValue("@birthDate", birthDate);
            command.Parameters.AddWithValue("@fnum", fnum);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public static void InsertCustomer(string connectionString,string name,string passport, string nationality)
        {
            using SqlConnection connection = new(connectionString);
            string queryString = "Insert Into Customer values(@name,@passport,@nationality);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@passport", passport);
            command.Parameters.AddWithValue("@nationality", nationality);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public static void InsertAdmin(string connectionString, string name)
        {
            using SqlConnection connection = new(connectionString);
            string queryString = "Insert Into Admin values(@name);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            connection.Open();
            command.ExecuteNonQuery();

        }
        public static void DeleteCustomerById(string connectionString,int Id) 
        {
            using SqlConnection connection = new(connectionString);
            string queryString = "Delete from Customer where ID=@Id";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@Id", Id);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public static IEnumerable<string[]> ReadTable(string connectionString, string tableName)
        {
            using SqlConnection connection = new(connectionString);
            string queryString = $"SELECT * FROM {tableName};";
            SqlCommand command = new(queryString, connection);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string[] values = new string[reader.FieldCount];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = reader[i].ToString() ?? "";
                }
                yield return values;
            }
        }
        public static IEnumerable<Tuple<string>> GetFlights(string connectionString, DateTime firstDate,DateTime secondDate,string destination,string source) 
        {
            using SqlConnection connection = new(connectionString);
            string queryString = "SELECT Fnum From Flight where Destination=@destination AND TakeOff=@source AND TakeOffDate between @firstDate AND @secondDate";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@destination", destination);
            command.Parameters.AddWithValue("@source", source);
            command.Parameters.AddWithValue("@firstDate", firstDate);
            command.Parameters.AddWithValue("@secondDate", secondDate);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) 
            {
                yield return new(reader.GetString(0));
            }
        }
        public static IEnumerable<Tuple<int,string,string,string>> ReadCustomers(string connectionString)
        {
            using SqlConnection connection = new(connectionString);
            string queryString = "SELECT * FROM Customer;";
            SqlCommand command = new(queryString, connection);
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            }
        }
    }
}
