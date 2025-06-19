using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=ASB2012SV011LAB;Database=nexris;Trusted_Connection=True;";

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Press ESC to exit or enter a search term (PLU number, Stock number, or Description):");

            string userInput = ReadLineOrEscape();
            if (userInput == null)
            {
                Console.WriteLine("\nExiting...");
                break; // Escape key pressed
            }

            string query = "";
            string columnToSearch = "";

            if (int.TryParse(userInput, out int numericInput))
            {
                columnToSearch = (userInput.Length <= 7) ? "StkNmbr" : "PLUNmbr";
            }
            else
            {
                columnToSearch = "Descr";
            }

            query = $"SELECT PLUNmbr, StkNmbr, Descr FROM item WHERE {columnToSearch} LIKE @input + '%'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@input", userInput);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Console.WriteLine("\n--- Matching Results ---\n");
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"PLUNmbr: {reader["PLUNmbr"]}, StkNmbr: {reader["StkNmbr"]}, Descr: {reader["Descr"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No matching results found.");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("\nPress any key to search again or ESC to exit...");
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Exiting...");
                break;
            }
        }
    }

    static string ReadLineOrEscape()
    {
        string input = "";
        ConsoleKeyInfo key;

        while (true)
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine(); // move to next line
                return input;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                return null;
            }
            else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input.Substring(0, input.Length - 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        }
    }
}
