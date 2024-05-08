using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class DatabaseHandler
    {   
        //class Atributes
        static private string connectionString  = null;
        static SqlConnection connection = null;

        //SetConnectionString - function to open the connection
        public static void SetConnectionString(string connectionString)
        {
            if (DatabaseHandler.connectionString == null)
            {
                try
                {
                    DatabaseHandler.connectionString = connectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    Console.WriteLine("Connection start succefuly\n");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Connection Error\n");
                }
            }
            else
            {
                Console.WriteLine("Connection is already running ");
            }
        }

        // ColseConnection -  function to close the conection
        public static void ColseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Connection closed\n");
            }
            else
            {
                Console.WriteLine("NO connection\n");
            }
        }


        public static void ChangeCustomerNameById(string newName,int id)
        {
            string queryString = "Update Customer Set Name = @Name where ID = @Id";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", newName);
            command.ExecuteNonQuery();
        }
        public static void ChangeFlightTakeOffDateByFnum(DateTime newTime, string fnum)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string queryString = "Update Flight Set TakeOffDate = @newTime where Fnum = @fnum";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@fnum", fnum);
            command.Parameters.AddWithValue("@newTime", newTime);
            command.ExecuteNonQuery();
        }
        public static void DeleteAircraftById(int id)
        {
            string queryString = "DELETE FROM Aircraft WHERE ID = @Id;";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
        public static void InsertInfant(string name,int companionId,DateTime birthDate,string fnum)
        {
            string queryString = "Insert Into Infant values(@name,@companionId,@birthDate,@fnum);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@companionId", companionId);
            command.Parameters.AddWithValue("@birthDate", birthDate);
            command.Parameters.AddWithValue("@fnum", fnum);
            command.ExecuteNonQuery();
        }
        public static void InsertCustomer(string name,string passport, string nationality)
        {
            string queryString = "Insert Into Customer values(@name,@passport,@nationality);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@passport", passport);
            command.Parameters.AddWithValue("@nationality", nationality);
            command.ExecuteNonQuery();
        }
        public static void InsertAdmin(string name)
        {
            string queryString = "Insert Into Admin values(@name);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
        }

        public static void DeleteCustomerById(int Id) 
        {
            
            string queryString = "Delete from Customer where ID=@Id";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }
        public static IEnumerable<string[]> ReadTable(string tableName)
        {
            string queryString = $"SELECT * FROM {tableName};";
            SqlCommand command = new(queryString, connection);
 
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
        public static IEnumerable<Tuple<string>> GetFlights(DateTime firstDate,DateTime secondDate,string destination,string source) 
        {
            string queryString = "SELECT Fnum From Flight where Destination=@destination AND TakeOff=@source AND TakeOffDate between @firstDate AND @secondDate";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@destination", destination);
            command.Parameters.AddWithValue("@source", source);
            command.Parameters.AddWithValue("@firstDate", firstDate);
            command.Parameters.AddWithValue("@secondDate", secondDate);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) 
            {
                yield return new(reader.GetString(0));
            }
        }
        public static IEnumerable<Tuple<int,string,string,string>> ReadCustomers()
        {
            string queryString = "SELECT * FROM Customer;";
            SqlCommand command = new(queryString, connection);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            }
        }
    }
}
