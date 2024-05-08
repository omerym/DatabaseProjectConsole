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


        static void Main(string[] args)
        {
            //the start of the program 
            Console.WriteLine("Welcome to flight system\n");
            string input;
            do
            {
                input = Console.ReadLine();//user command
                HandelCmmand(input);
            } while (input != "exit");

            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Airplane_comm_DP;Integrated Security=True;Connect Timeout=30;Encrypt=false;";
            DatabaseHandler.SetConnectionString(connection);
            DatabaseHandler.SetConnectionString(connection);
            DatabaseHandler.InsertAdmin("omar");
            DatabaseHandler.InsertAdmin("mahmoud");
            DatabaseHandler.InsertAdmin("person");
            DatabaseHandler.ColseConnection();
            
        }
    }
}