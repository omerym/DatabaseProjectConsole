using static System.Console;
namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FlightSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            var admins = DatabaseHandler.ReadTable(connection, "Infant");
            foreach (var admin in admins)
            {
                for (int i = 0;i<admin.Length;i++)
                {
                    Write($"{admin[i]} ");
                }
                WriteLine("");
            }
        }
    }
}
