using SqlClient = Microsoft.Data.SqlClient;
using DT = System.Data;
using Bookworm.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bookworm.Data;

public class ApplicationDbClient
{
    private static string connectionString =
        "Server=MANTHANRAJ-RAJO\\SQLEXPRESS2022;" +
        "Database=Bookworm;" +
        "Trusted_Connection=True;TrustServerCertificate=True;" +
        "Connection Timeout=30;" +
        "Encrypt=True;";

    private static readonly DT.DataSet? dataSet = new DT.DataSet();

    static ApplicationDbClient()
    {
        dataSet = RunCrudSelectQuery();
    }

    // Connected Data Access
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

    // Disconnected Data Access
    public static DataSet? RunCrudSelectQuery()
    {
        try
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("SELECT * FROM Categories", connection);
            query.CommandTimeout = 30;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = query;
            connection.Open();
            DT.DataSet categoriesSet = new DT.DataSet();
            adapter.Fill(categoriesSet, "Categories");
            connection.Close();
            return categoriesSet;

        }
        catch
        {
            return null;
        }

    }

    public static bool RunCrudInsertQuery(Category category){
        if(dataSet == null) return false;
        foreach (DT.DataTable table in dataSet.Tables)
        {
            if (table.TableName.Equals("Categories"))
            {
                table.Rows.Add(category.Id, category.Name, category.DisplayOrder);
                return true;
            }
        }
        return false;
    }

    public static bool RunCrudUpdateQuery(Category category)
    {
        try
        {
            if(dataSet == null) return false;
            foreach(DataTable table in dataSet.Tables){
                if(table.TableName.Equals("Categories")){
                    foreach(DataRow row in table.Rows){
                        if(Convert.ToInt32(row["Id"]) == category.Id){
                            row["Name"] = category.Name;
                            row["DisplayOrder"] = category.DisplayOrder;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public static bool RunCrudDeleteQuery(int id)
    {
        try
        {
            if(dataSet == null) return false;
            foreach(DataTable table in dataSet.Tables){
                if(table.TableName.Equals("Categories")){
                    foreach(DataRow row in table.Rows){
                        if(Convert.ToInt32(row["Id"]) == id){
                            table.Rows.Remove(row);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    // Helper Methods
    public static List<Category> GetCategoryList()
    {
        // Creating List from DataSet
        List<Category> categories = new List<Category>();
        if(dataSet == null) return categories;
        foreach (DT.DataTable table in dataSet.Tables)
        {
            if (table.TableName.Equals("Categories"))
            {
                foreach (DT.DataRow row in table.Rows)
                {
                    categories.Add(new Category
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = Convert.ToString(row["Name"]),
                        DisplayOrder = Convert.ToInt32(row["DisplayOrder"])
                    });
                }
            }
        }
        return categories;
    }
    
    public static bool RunDOQuery(int id)
    {
        try
        {
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
        }
        catch
        {
            return false;
        }
    }
}
