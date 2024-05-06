using static System.Console;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FlightSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;";


            DatabaseHandler.DeleteInfant(connection, 1);


            var infants = DatabaseHandler.ReadTable(connection, "Infant");
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