using System.Diagnostics;
using System.Net.Security;
using static System.Console;

namespace ConsoleApp
{
    internal class Program
    {
        //@HandelCmmand - function that handel commmands
        static void HandelCmmand(string input)
        {
            //command insert name_of_table  [attributes]
            //command delete name_of_table ID
            //command update name_of_table  [ID, attribute : value]
            char[] delimiters = { '[', ']' };
            string[] CommandAtributes = input.Split(delimiters); // Split the string by the square prackets to split the atributtes
            string[] Command = CommandAtributes[0].Split(' ');


            foreach (string part in CommandAtributes)
            {
                Console.WriteLine(part);// testing spliting of the command
            }

            switch (Command[0])
            {
                case "insert":
                    Console.WriteLine("You entered one.");//test
                    break;
                case "update":
                    Console.WriteLine("You entered two.");//test
                    break;
                case "delete":
                    Console.WriteLine("You entered three.");//test
                    break;
                case "exit":
                    break;
                case "show":
                    break;
                case "help":
                    Console.WriteLine("- command insert name_of_table  [attributes]\n");
                    Console.WriteLine("- command delete name_of_table [ID]\n");
                    Console.WriteLine("- command update name_of_table  [ID, attribute : value]\n");
                    break;
                default:
                    Console.WriteLine("Invalid input.");//test
                    break;
            }
        }
        static void GetFlights(DatabaseHandler databaseHandler)
        {
            string destination, source, f, s;
            DateTime first, second;
            WriteLine("Enter flight source:");
            source = ReadLine() ?? "";
            WriteLine("Enter flight destination:");
            destination = ReadLine() ?? "";
            WriteLine("Enter first date:");
            f = ReadLine() ?? "";
            WriteLine("Enter second date:");
            s = ReadLine() ?? "";
            if (DateTime.TryParse(f, out first) && DateTime.TryParse(s, out second))
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
            int id;
            WriteLine("Enter Aircraft Id:");
            if (int.TryParse(ReadLine(), out id))
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
            int id;
            WriteLine("Enter Customer Id:");
            if (int.TryParse(ReadLine(), out id))
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
    }
}