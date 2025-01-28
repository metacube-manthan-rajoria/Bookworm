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
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection == null) return null;

                List<Category> categoryList = new List<Category>();

                using (var command = new SqlCommand("selectCategories", connection))
                {
                    command.CommandType = DT.CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
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
                }

                return categoryList;
            }
        }
        catch
        {
            return null;
        }
    }

    public static bool RunInsertQuery(Category category)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            if (connection == null) return false;

            SqlTransaction transaction = connection.BeginTransaction();

            try{
                using (var command = new SqlCommand("insertCategories", connection, transaction))
                {
                    command.CommandType = DT.CommandType.StoredProcedure;

                    SqlParameter param1 = new SqlParameter
                    {
                        ParameterName = "@name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = category.Name,
                        Direction = ParameterDirection.Input
                    };
                    SqlParameter param2 = new SqlParameter
                    {
                        ParameterName = "@displayOrder",
                        SqlDbType = SqlDbType.Int,
                        Value = category.DisplayOrder,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(param1);
                    command.Parameters.Add(param2);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(category.Name!.Equals("Demo")) throw new Exception("Demo exception for checking transaction");
                        transaction.Commit();
                        return true;
                    }
                }
            }catch{
                transaction.Rollback();
                return false;
            }
        }
    }

    public static bool RunUpdateQuery(Category category)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            if (connection == null) return false;

            SqlTransaction transaction = connection.BeginTransaction();

            try{
                using (var command = new SqlCommand("updateCategories", connection, transaction))
                {
                    command.CommandType = DT.CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Value = category.Id,
                        Direction = ParameterDirection.Input
                    };
                    SqlParameter param2 = new SqlParameter
                    {
                        ParameterName = "@name",
                        SqlDbType = SqlDbType.VarChar,
                        Value = category.Name,
                        Direction = ParameterDirection.Input
                    };
                    SqlParameter param3 = new SqlParameter
                    {
                        ParameterName = "@displayOrder",
                        SqlDbType = SqlDbType.Int,
                        Value = category.DisplayOrder,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(param1);
                    command.Parameters.Add(param2);
                    command.Parameters.Add(param3);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(category.Name!.Equals("Demo")) throw new Exception("Demo exception for transaction");
                        transaction.Commit();
                        return true;
                    }
                }
            }catch{
                transaction.Rollback();
                return false;
            }   
        }
    }

    public static bool RunDeleteQuery(int id)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection == null) return false;

                using (var command = new SqlCommand("deleteCategories", connection))
                {
                    command.CommandType = DT.CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Value = id,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(param);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return true;
                    }
                }
            }
        }
        catch
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
