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

    public static bool RunInsertQuery(Category category)
    {
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

    public static bool RunUpdateQuery(Category category)
    {
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

    public static bool RunDeleteQuery(int id)
    {
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
                    command.CommandText = @"DELETE FROM Categories WHERE Id=" + id;
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

    public static int? RunMaxDisplayOrderQuery()
    {
        try
        {
            int? maxDisplayOrders = null;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetMaxDisplayOrder", connection))
                {
                    command.CommandType = DT.CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            maxDisplayOrders = reader.GetInt32(0);
                        }
                    }
                }
            }

            return maxDisplayOrders;
        }
        catch
        {
            return null;
        }
    }

    public static List<Category> RunCrudSelectQuery()
    {
        try{
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("SELECT * FROM Categories", connection);
            query.CommandTimeout = 30;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = query;
            connection.Open();
            DT.DataSet categoriesSet = new DT.DataSet();
            adapter.Fill(categoriesSet, "Categories");
            connection.Close();

            // Creating List from DataSet
            List<Category> categories = new List<Category>();
            foreach(DT.DataTable table in categoriesSet.Tables){
                if(table.TableName.Equals("Categories")){
                    foreach(DT.DataRow row in table.Rows){
                        categories.Add(new Category{
                            Id = Convert.ToInt32(row["Id"]),
                            Name = Convert.ToString(row["Name"]),
                            DisplayOrder = Convert.ToInt32(row["DisplayOrder"])
                        });
                    }
                }
            }
            return categories;
        }catch{
            return new List<Category>();
        }
        
    }

    public static bool RunCrudUpdateQuery(int id)
    {
        try{
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("Update Categories SET DisplayOrder = DisplayOrder + 1 WHERE Id=" + id, connection);
            query.CommandTimeout = 30;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = query;
            connection.Open();
            DT.DataSet categoriesSet = new DT.DataSet();
            adapter.Fill(categoriesSet, "Categories");
            connection.Close();
            return true;
        }catch{
            return false;
        }
    }

    public static bool RunCrudDeleteQuery()
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Integrated Security=SSPI;Initial Catalog=Bookworm");
        SqlCommand query = new SqlCommand("SELECT * FROM Categories", connection);
        query.CommandTimeout = 30;
        SqlDataAdapter customerDA = new SqlDataAdapter();
        customerDA.SelectCommand = query;
        connection.Open();
        DT.DataSet customerDS = new DT.DataSet();
        customerDA.Fill(customerDS, "Customers");
        connection.Close();
        return true;
    }
}
