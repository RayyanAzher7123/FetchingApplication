using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        Console.Write("Enter PLU Number: ");
        string pluNumber = Console.ReadLine();


        string connectionString = "Server=ASB2012SV011LAB;Database=nexris;Trusted_Connection=True;";
        string query = "SELECT * FROM item WHERE PLUNmbr = @PLUNmbr";


        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PLUNmbr", pluNumber);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("\n--- Item Details ---\n");

                    while (reader.Read())
                    {
                        // Display all columns
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine($"{reader.GetName(i)}: {reader[i]}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No item found with that PLU Number.");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
}
