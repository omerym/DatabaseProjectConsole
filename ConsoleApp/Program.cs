using static System.Console;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FlightSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            var infants = DatabaseHandler.ReadTable(connection, "Flight");
            DateTime StartDate = new DateTime(2022, 12, 1, 12, 22, 0);
            DateTime EndDate = new DateTime(2022, 12, 4, 12, 24, 0);

            var flights = DatabaseHandler.GetFlights(connection, StartDate, EndDate, "Krt", "Cai");
            foreach (var flight in flights)
            {
                Write($"{flight} ");
                WriteLine("Working");
            }
            WriteLine("");
            DatabaseHandler.GetFlights(connection, StartDate, EndDate, "Cai","Krt");
            foreach (var infant in infants)
            {
                for (int i = 0; i < infant.Length; i++)
                {
                    Write($"{infant[i]} ");
                }
                WriteLine("");
            }
        }
    }
}