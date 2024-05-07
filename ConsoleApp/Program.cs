using System.Diagnostics;
using static System.Console;

namespace ConsoleApp
{
    internal class Program
    {
        static void HandelCmmand(string input)
        {
            //command insert name_of_table  [attributes]
            //command delete name_of_table [ID]
            //command update name_of_table  [ID, attribute : value]
            char[] delimiters = { '[', ']'};
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
            default:
                Console.WriteLine("Invalid input.");//test
                break;
            }
        }


        static void Main(string[] args)
        {
            //the start of the program 
            Console.WriteLine("Welcome to flight system\n");
            string input;
            do
            {
                input = Console.ReadLine();
                HandelCmmand(input);
            } while (input != "exit");

            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Airplane_comm_DP;Integrated Security=True;Connect Timeout=30;Encrypt=false;";
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