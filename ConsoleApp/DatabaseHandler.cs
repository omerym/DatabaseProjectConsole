using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class DatabaseHandler : IDisposable
    {
        //class Atributes
        private readonly string connectionString;
        private SqlConnection connection;
        private bool disposedValue;

        public DatabaseHandler(string _connectionString)
        {
            connectionString = _connectionString;
            connection = new SqlConnection(connectionString);
        }
        /// <summary>
        /// Ensure connection to database. To be called before using connection field.
        /// </summary>
        private void EnsureConnection()
        {
            connection ??= new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }
        #region Update
        public void ChangeCustomerNameById(string newName, int id)
        {
            EnsureConnection();
            string queryString = "Update Customer Set Name = @Name where ID = @Id";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", newName);
            command.ExecuteNonQuery();
        }
        public void ChangeFlightTakeOffDateByFnum(DateTime newTime, string fnum)
        {
            EnsureConnection();
            string queryString = "Update Flight Set TakeOffDate = @newTime where Fnum = @fnum";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@fnum", fnum);
            command.Parameters.AddWithValue("@newTime", newTime);
            command.ExecuteNonQuery();
        }
        #endregion
        #region Insert
        public void InsertInfant(string name, int companionId, DateTime birthDate, string fnum)
        {
            EnsureConnection();
            string queryString = "Insert Into Infant values(@name,@companionId,@birthDate,@fnum);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@companionId", companionId);
            command.Parameters.AddWithValue("@birthDate", birthDate);
            command.Parameters.AddWithValue("@fnum", fnum);
            command.ExecuteNonQuery();
        }
        public void InsertCustomer(string name, string passport, string nationality)
        {
            EnsureConnection();
            string queryString = "Insert Into Customer values(@name,@passport,@nationality);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@passport", passport);
            command.Parameters.AddWithValue("@nationality", nationality);
            command.ExecuteNonQuery();
        }
        public void InsertAdmin(string name)
        {
            EnsureConnection();
            string queryString = "Insert Into Admin values(@name);";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
        }
        #endregion
        #region Delete
        /// <summary>
        /// Deletes all flights before a given date.
        /// </summary>
        /// <param name="date"></param>
        public void DeleteFlightsByDate(DateTime date)
        {
            EnsureConnection();
            string query = "Delete from Flight where TakeOffDate < @date";
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@date", date);
            command.ExecuteNonQuery();

        }
        public void DeleteCustomerById(int Id)
        {
            EnsureConnection();
            string queryString = "Delete from Customer where ID=@Id";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }
        public void DeleteAircraftById(int id)
        {
            EnsureConnection();
            string queryString = "DELETE FROM Aircraft WHERE ID = @Id;";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
        #endregion
        #region Select
        public Flight GetFlightByFnum(string fnum)
        {
            string queryString = $"SELECT * FROM Flight WHERE Fnum = '{fnum}';";
            using SqlDataReader reader = GetReaderFromQuery(queryString);
            reader.Read();
            int? aircraftId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
            int? creatorId = reader.IsDBNull(6) ? null : reader.GetInt32(6);
            return new Flight(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetDateTime(4), aircraftId, creatorId);
        }
        public Customer GetCustomerById(int Id)
        {
            string queryString = $"SELECT * FROM Customer WHERE ID = {Id};";
            using SqlDataReader reader = GetReaderFromQuery(queryString);
            reader.Read();
            string passport = reader.IsDBNull(2) ? "" : reader.GetString(2);
            string nationality = reader.IsDBNull(3) ? "" : reader.GetString(3);
            return new Customer(reader.GetInt32(0), reader.GetString(1), passport, nationality);
        }
        public SqlDataReader GetReaderFromQuery(string query)
        {
            EnsureConnection();
            SqlCommand command = new(query, connection);
            return command.ExecuteReader();
        }
        public SqlDataReader GetTableReader(string tableName, string columns = "*")
        {
            string queryString = $"SELECT {columns} FROM {tableName};";
            return GetReaderFromQuery(queryString);
        }
        public IEnumerable<string[]> ReadTable(string tableName)
        {
            using SqlDataReader reader = GetTableReader(tableName);
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
        /// <summary>
        /// Gets flights between two dates
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <returns>SqlDataReader for flights</returns>
        public SqlDataReader GetFlightsReader(DateTime firstDate, DateTime secondDate, string destination, string source)
        {
            string queryString = $"SELECT * From Flight where Destination='{destination}'" +
                $" AND TakeOff='{source}' AND " +
                $"TakeOffDate between '{firstDate}' AND '{secondDate}'";
            return GetReaderFromQuery(queryString);
        }
        /// <summary>
        /// Gets flights before a date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>SqlDataReader for flights</returns>
        public SqlDataReader GetFlightsReader(DateTime date)
        {
            string queryString = $"SELECT * From Flight where  TakeOffDate < '{date}'";
            return GetReaderFromQuery(queryString);
        }
        /// <summary>
        /// Gets flights between two dates
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<Flight> GetFlights(DateTime firstDate, DateTime secondDate, string destination, string source)
        {
            using SqlDataReader reader = GetFlightsReader(firstDate, secondDate, destination, source);
            foreach (var flight in GetFlights(reader))
            {
                yield return flight;
            }
        }



        /// <summary>
        /// Gets flights before a date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<Flight> GetFlights(DateTime date)
        {
            using SqlDataReader reader = GetFlightsReader(date);
            foreach (var flight in GetFlights(reader))
            {
                yield return flight;
            }
        }
        /// <summary>
        /// Reads flights from an SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<Flight> GetFlights(SqlDataReader reader)
        {
            while (reader.Read())
            {
                int? aircraftId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                int? creatorId = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                yield return new(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetDateTime(3),
                    reader.GetDateTime(4),
                    aircraftId,
                    creatorId);
            }
        }
        public IEnumerable<Customer> ReadCustomers()
        {
            using SqlDataReader reader = GetTableReader("Customer");
            while (reader.Read())
            {
                string passport = reader.IsDBNull(2) ? "" : reader.GetString(2);
                string nationality = reader.IsDBNull(3) ? "" : reader.GetString(3);
                yield return new(reader.GetInt32(0), reader.GetString(1), passport, nationality);
            }
        }
        #endregion
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
