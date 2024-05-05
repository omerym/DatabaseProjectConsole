using static System.Console;
namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FlightSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            DatabaseHandler.InsertInfant(connection, "Fayez", 1, DateTime.Parse("2022/11/19"), "Ab809");
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
