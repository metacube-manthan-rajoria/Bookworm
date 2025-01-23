using SqlClient = Microsoft.Data.SqlClient;
using DT = System.Data;
using Bookworm.Models;
using Microsoft.Data.SqlClient;

namespace Bookworm.Data;

public class ApplicationDbClient
{
    private static string connectionString =
        "Server=MANTHANRAJ-RAJO\\SQLEXPRESS2022;" +
        "Database=Bookworm;" +
        "Trusted_Connection=True;TrustServerCertificate=True;" +
        "Connection Timeout=30;" +
        "Encrypt=True;";

    static public List<Category>? RunSelectQuery()
    {
        try
        {
            using (var connection = new SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                if (connection == null) return null;

                List<Category> categoryList = new List<Category>();

                using (var command = new SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText = @"SELECT * FROM Categories";

                    SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        categoryList.Add(new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            DisplayOrder = reader.GetInt32(2)
                        });
                    }
                }

                connection.Close();
                return categoryList;
            }
        }
        catch (SqlException)
        {
            return new List<Category>();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
