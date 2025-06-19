using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        Console.Write("Enter PLU number, Article Number or Description to Search: ");
        string userInput = Console.ReadLine();

        string connectionString = "Server=ASB2012SV011LAB;Database=nexris;Trusted_Connection=True;";
        string query = "";
        string columnToSearch = "";

        if (int.TryParse(userInput, out int numericInput))
        {
            if (userInput.Length <= 7)
            {
                columnToSearch = "StkNmbr";
            }
            else
            {
                columnToSearch = "PLUNmbr";  
            }

            query = $"SELECT PLUNmbr, StkNmbr, Descr FROM item WHERE {columnToSearch} LIKE @input + '%'";
        }
        else
        {
            columnToSearch = "Descr";
            query = $"SELECT PLUNmbr, StkNmbr, Descr FROM item WHERE {columnToSearch} LIKE @input + '%'";
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@input", userInput);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("\n--- Item Details ---\n");

                        while (reader.Read())
                        {
                            Console.WriteLine($"PLUNmbr: {reader["PLUNmbr"]}, StkNmbr: {reader["StkNmbr"]}, Descr: {reader["Descr"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No item found.");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
}
