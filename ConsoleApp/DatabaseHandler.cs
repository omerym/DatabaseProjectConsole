﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
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
        private void EnsureConnection()
        {
            connection ??= new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }
        public void ChangeCustomerNameById(string newName,int id)
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
            using SqlConnection connection = new(connectionString);
            string queryString = "Update Flight Set TakeOffDate = @newTime where Fnum = @fnum";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@fnum", fnum);
            command.Parameters.AddWithValue("@newTime", newTime);
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
        public void InsertInfant(string name,int companionId,DateTime birthDate,string fnum)
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
        public void InsertCustomer(string name,string passport, string nationality)
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

        public void DeleteCustomerById(int Id)
        {
            EnsureConnection();
            string queryString = "Delete from Customer where ID=@Id";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }
        public IEnumerable<string[]> ReadTable(string tableName)
        {
            EnsureConnection();
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
        public IEnumerable<Flight> GetFlights(DateTime firstDate,DateTime secondDate,string destination,string source)
        {
            EnsureConnection();
            string queryString = "SELECT * From Flight where Destination=@destination AND TakeOff=@source AND TakeOffDate between @firstDate AND @secondDate";
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@destination", destination);
            command.Parameters.AddWithValue("@source", source);
            command.Parameters.AddWithValue("@firstDate", firstDate);
            command.Parameters.AddWithValue("@secondDate", secondDate);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) 
            {
                yield return new(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetDateTime(4),
                    reader.GetInt32(5),
                    reader.GetInt32(6));
            }
        }
        public IEnumerable<Customer> ReadCustomers()
        {
            EnsureConnection();
            string queryString = "SELECT * FROM Customer;";
            SqlCommand command = new(queryString, connection);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            }
        }

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
