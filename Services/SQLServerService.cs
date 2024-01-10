using AccountAPI.Models;
using Microsoft.Data.SqlClient;

namespace AccountAPI.Services;

public class SQLServerService
{
    private string _dbConn;
    public SQLServerService(IConfiguration config)
    {
        _dbConn = config.GetSection("DBConnection").Value;
    }
    public List<AccountModel> GetAccountList()
    {
        using SqlConnection connection = new SqlConnection(_dbConn);
        string sqlStr = @"select [ID],[UserCode],[UserName],[UserAge] 
                          FROM [AccountSystem].[dbo].[Account]";
        using SqlCommand command = connection.CreateCommand();
        command.Connection.Open();
        command.CommandText = sqlStr;
        using SqlDataReader reader = command.ExecuteReader();
        List<AccountModel> result = new();
        while (reader.Read())
        {
            result.Add(new()
            {
                ID = reader.GetInt32(0),
                UserCode = reader.GetString(1),
                UserName = reader.GetString(2),
                UserAge = reader.GetInt32(3),
            });
        }
        reader.Close();
        return result;
    }
    public AccountModel GetAccountById(int id)
    {
        using SqlConnection connection = new SqlConnection(_dbConn);
        string sqlStr = @"select [UserCode],[UserName],[UserAge] 
                          from [AccountSystem].[dbo].[Account] 
                          where [ID] = @id";
        using SqlCommand command = connection.CreateCommand();
        command.Connection.Open();
        command.CommandText = sqlStr;
        command.Parameters.AddWithValue("@id", id);
        using SqlDataReader reader = command.ExecuteReader();
        reader.Read();

        return new AccountModel()
        {
                ID = reader.GetInt32(0),
                UserCode = reader.GetString(1),
                UserName = reader.GetString(2),
                UserAge = reader.GetInt32(3),
        };
    }
    public int AddAccount(AddAccountRequestModel reqModel)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_dbConn);
            string insertData = @"insert into [AccountSystem].[dbo].[Account]  
                                            ([UserCode],[UserName],[UserAge]) 
                                    values (@UserCode, @UserName, @UserAge)";
            using SqlCommand command = connection.CreateCommand();
            command.Connection.Open();
            command.CommandText = insertData;
            command.Parameters.AddWithValue("@UserCode", reqModel.UserCode);
            command.Parameters.AddWithValue("@UserName", reqModel.UserName);
            command.Parameters.AddWithValue("@UserAge", reqModel.UserAge);

            // 取得新增資料後自動產生的 id
            command.CommandText += "select @@identity";
            int id = Convert.ToInt32(command.ExecuteScalar());
            command.Connection.Close();
            return id;
        }
        catch
        {
            return -1;
        }
        
    }
    public bool EditAccount(int id, EditAccountRequestModel reqModel)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_dbConn);
            string updateData = @"update [AccountSystem].[dbo].[Account] 
                                set UserName = @UserName, 
                                    UserAge = @UserAge 
                                where id = @id";
            using SqlCommand command = connection.CreateCommand();
            command.Connection.Open();
            command.CommandText = updateData;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@UserName", reqModel.UserName);
            command.Parameters.AddWithValue("@UserAge", reqModel.UserAge);
            command.ExecuteNonQuery();
            command.Connection.Close();
            return true;
        }
        catch
        {
            return false;
        }
        
    }
    public bool DeleteAccount(int id)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_dbConn);
            string sql = @" delete from [AccountSystem].[dbo].[Account] 
                            where id = @id ";
            using SqlCommand command = connection.CreateCommand();
            command.Connection.Open();
            command.CommandText = sql;
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            command.Connection.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
}