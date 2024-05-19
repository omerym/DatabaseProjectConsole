using System.Diagnostics;
using System.Net.Security;
using System.Xml.Linq;
using static System.Console;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FlightSystem;Integrated Security=True;Connect Timeout=30;Encrypt=false;";
            using DatabaseHandler dbHandler = new(connectionString);
            int input = 1;
            while (input != 0)
            {
                WriteLine("Enter Input for:");
                WriteLine("0: Exit");
                WriteLine("1: Print Table");
                WriteLine("2: Get Flights between two dates");
                WriteLine("3: Delete Customer By Id");
                WriteLine("4: Delete Aircraft By Id");
                WriteLine("5: Delete all flights before a date");
                WriteLine("6: Insert new customer");
                WriteLine("7: Insert new admin");
                WriteLine("8: Insert new infant");
                WriteLine("9: Change customer name by ID");
                WriteLine("10: Change flight takeoff date by flight number");
                WriteLine("11: Get Aircraft flights report");
                if (int.TryParse(ReadLine(), out input))
                {
                    switch (input)
                    {
                        case 0:
                            break;
                        case 1:
                            PrintTable(dbHandler);
                            break;
                        case 2:
                            GetFlights(dbHandler);
                            break;
                        case 3:
                            DeleteCustomer(dbHandler);
                            break;
                        case 4:
                            DeleteAircraft(dbHandler);
                            break;
                        case 5:
                            DeleteFlights(dbHandler);
                            break;
                        case 6:
                            InsertCustomer(dbHandler);
                            break;
                        case 7:
                            InsertAdmin(dbHandler);
                            break;
                        case 8:
                            InsertInfant(dbHandler);
                            break;
                        case 9:
                            ChangeCustomerName(dbHandler);
                            break;
                        case 10:
                            ChangeFlightTakeOffDate(dbHandler);
                            break;
                        case 11:
                            GetFlightAircraftReport(dbHandler);
                            break;
                        default:
                            WriteLine("Invalid Input");
                            break;
                    }
                }
                else
                {
                    input = 1;
                    WriteLine("Invalid Input");
                }
                WriteLine();
                WriteLine();
            }
        }
        private static void GetFlightAircraftReport(DatabaseHandler dbHandler)
        {
            try
            {
                foreach (string[] item in dbHandler.ReadReport())
                {
                    foreach (string value in item)
                    {
                        Write($"{value}, ");
                    }
                    WriteLine();
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }
        private static void ChangeFlightTakeOffDate(DatabaseHandler dbHandler)
        {
            string fnum;
            string? dateS;
            WriteLine("Enter flight number:");
            fnum = ReadLine() ?? "";
            try
            {
                Flight flight = dbHandler.GetFlightByFnum(fnum);
                while (true)
                {
                    WriteLine($"Old takeoff date: {flight.TakeOffDate}");
                    WriteLine("Enter new takeoff date:");
                    dateS = ReadLine() ?? "";
                    if (DateTime.TryParse(dateS, out DateTime date))
                    {
                        dbHandler.ChangeFlightTakeOffDateByFnum(date, fnum);
                        break;
                    }
                    WriteLine("Invalid date!");
                    WriteLine("Do you want to try again? (yes | no):");
                    if (!"yes".Equals(ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private static void ChangeCustomerName(DatabaseHandler dbHandler)
        {
            string name;
            string? IDS;
            WriteLine("Enter customer ID:");
            IDS = ReadLine();
            if (int.TryParse(IDS, out int Id))
            {
                try
                {
                    Customer customer = dbHandler.GetCustomerById(Id);
                    WriteLine($"Old name: {customer.Name}");
                    WriteLine("Enter new name:");
                    name = ReadLine() ?? "";
                    dbHandler.ChangeCustomerNameById(name, Id);
                }
                catch (Exception ex)
                {
                    WriteLine(ex);
                }
            }
            else
            {
                WriteLine("Invalid id!");
                WriteLine("Do you want to try again? (yes | no):");
                if ("yes".Equals(ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                {
                    ChangeCustomerName(dbHandler);
                }
            }
        }

        static void GetFlights(DatabaseHandler databaseHandler)
        {
            string destination, source;
            string? f, s;
            WriteLine("Enter flight source:");
            source = ReadLine() ?? "";
            WriteLine("Enter flight destination:");
            destination = ReadLine() ?? "";
            WriteLine("Enter first date:");
            f = ReadLine();
            WriteLine("Enter second date:");
            s = ReadLine();
            if (DateTime.TryParse(f, out DateTime first) && DateTime.TryParse(s, out DateTime second))
            {
                try
                {
                    foreach (Flight item in databaseHandler.GetFlights(first, second, destination, source))
                    {
                        WriteLine(item);
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                WriteLine("Invalid date!");
                WriteLine("Do you want to try again? (yes | no):");
                if ("yes".Equals(ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                {
                    GetFlights(databaseHandler);
                }
            }
        }
        static void PrintTable(DatabaseHandler dbHandler)
        {
            WriteLine("Enter table name:");
            string tableName = ReadLine() ?? "";
            try
            {
                foreach (string[] item in dbHandler.ReadTable(tableName))
                {
                    foreach (string value in item)
                    {
                        Write($"{value}, ");
                    }
                    WriteLine();
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }
        static void DeleteAircraft(DatabaseHandler databaseHandler)
        {
            WriteLine("Enter Aircraft Id:");
            if (int.TryParse(ReadLine(), out int id))
            {
                try
                {
                    databaseHandler.DeleteAircraftById(id);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                WriteLine("Invalid id!");
                WriteLine("Do you want to try again? (yes | no):");
                if ("yes".Equals(ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                {
                    GetFlights(databaseHandler);
                }
            }
        }
        static void DeleteCustomer(DatabaseHandler databaseHandler)
        {
            WriteLine("Enter Customer Id:");
            if (int.TryParse(ReadLine(), out int id))
            {
                try
                {
                    databaseHandler.DeleteCustomerById(id);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                WriteLine("Invalid id!");
                WriteLine("Do you want to try again? (yes | no):");
                if ("yes".Equals(ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                {
                    GetFlights(databaseHandler);
                }
            }
        }

        private static void InsertInfant(DatabaseHandler dbHandler)
        {
            string name, fnum;
            string? idS, birthdateS;
            WriteLine("Enter name:");
            name = ReadLine() ?? "";
            WriteLine("Enter flight number:");
            fnum = ReadLine() ?? "";
            WriteLine("Enter companion Id");
            idS = ReadLine();
            WriteLine("Enter birthdate:");
            birthdateS = ReadLine();
            if (int.TryParse(idS, out int id) && DateTime.TryParse(birthdateS, out DateTime birthdate))
            {
                try
                {
                    dbHandler.InsertInfant(name, id, birthdate, fnum);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                WriteLine("Invalid input!");
                WriteLine("Do you want to try again? (yes | no):");
                if ("yes".Equals(ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                {
                    InsertInfant(dbHandler);
                }
            }
        }

        private static void InsertAdmin(DatabaseHandler dbHandler)
        {
            string name;
            WriteLine("Enter name:");
            name = ReadLine() ?? "";
            try
            {
                dbHandler.InsertAdmin(name);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private static void InsertCustomer(DatabaseHandler dbHandler)
        {
            string name, passport, nationality;
            WriteLine("Enter name:");
            name = ReadLine() ?? "";
            WriteLine("Enter passport:");
            passport = ReadLine() ?? "";
            WriteLine("Enter nationality:");
            nationality = ReadLine() ?? "";
            try
            {
                dbHandler.InsertCustomer(name, passport, nationality);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private static void DeleteFlights(DatabaseHandler dbHandler)
        {
            WriteLine("Enter date:");
            string? s = ReadLine();
            if (DateTime.TryParse(s, out DateTime date))
            {
                try
                {
                    dbHandler.DeleteFlightsByDate(date);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                WriteLine("Invalid date!");
                WriteLine("Do you want to try again? (yes | no):");
                if ("yes".Equals(ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                {
                    DeleteFlights(dbHandler);
                }
            }
        }
    }
}