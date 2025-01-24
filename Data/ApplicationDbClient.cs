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

    public static List<Category>? RunSelectQuery()
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

    public static bool RunInsertQuery(Category category){
        try
        {
            using (var connection = new SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                if (connection == null) return false;

                using (var command = new SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText = 
                        "INSERT INTO Categories(Name, DisplayOrder) " +
                        $"VALUES('{category.Name}',{category.DisplayOrder})";
                    command.ExecuteNonQuery();
                }

                connection.Close();
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool RunUpdateQuery(Category category){
        try
        {
            using (var connection = new SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                if (connection == null) return false;

                using (var command = new SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText = 
                    $"UPDATE Categories SET Name='{category.Name}', DisplayOrder={category.DisplayOrder} WHERE Id={category.Id}";
                    command.ExecuteNonQuery();
                }

                connection.Close();
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool RunDeleteQuery(int id){
        try
        {
            using (var connection = new SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                if (connection == null) return false;

                using (var command = new SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText = @"DELETE FROM Categories WHERE Id="+id;
                    command.ExecuteNonQuery();
                }

                connection.Close();
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
}
