using AccountAPI.Models;
using CSICsSystemNotifySchedule.Adapters;
using Microsoft.Data.SqlClient;

namespace AccountAPI.Services;

public class AccountService
{
    private SQLServerAdapter _sqlServerAdapter;
    public AccountService(IConfiguration config,SQLServerAdapter sqlServerAdapter)
    {
        _sqlServerAdapter = sqlServerAdapter;
    }
    public List<AccountModel> GetAccountList()
    {
        string sqlStr = @"select [ID],[UserCode],[UserName],[UserAge] 
                          FROM [AccountSystem].[dbo].[Account]";
        var result = this._sqlServerAdapter.GetObjectType<AccountModel>(sqlStr);
        return result;
    }
    public AccountModel GetAccountById(int id)
    {
        string sqlStr = @"select [UserCode],[UserName],[UserAge] 
                          from [AccountSystem].[dbo].[Account] 
                          where [ID] = @id";
        List<SqlParameter> sqlParamList = new List<SqlParameter>();
        sqlParamList.Add(new SqlParameter("@id", id));
        SqlParameter[] sqlParams = sqlParamList.ToArray();
        var result = this._sqlServerAdapter.GetObjectType<AccountModel>(sqlStr,sqlParams)[0];
        return result;
    }
    public int AddAccount(AddAccountRequestModel reqModel)
    {
        try
        {
            string sqlStr = @"insert into [AccountSystem].[dbo].[Account]  
                                            ([UserCode],[UserName],[UserAge]) 
                                    values (@UserCode, @UserName, @UserAge);";
            sqlStr += "select @@identity";
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@UserCode", reqModel.UserCode));
            sqlParamList.Add(new SqlParameter("@UserName", reqModel.UserName));
            sqlParamList.Add(new SqlParameter("@UserAge", reqModel.UserAge));
            SqlParameter[] sqlParams = sqlParamList.ToArray();
            var id = this._sqlServerAdapter.GetObjectType<int>(sqlStr,sqlParams)[0];
            return id;
        }
        catch
        {
            return -1;
        }
        
    }
    public int EditAccount(int id, EditAccountRequestModel reqModel)
    {
        try
        {
            string sqlStr = @"update [AccountSystem].[dbo].[Account] 
                                set UserName = @UserName, 
                                    UserAge = @UserAge 
                                where id = @id";
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@id", id));
            sqlParamList.Add(new SqlParameter("@UserName", reqModel.UserName));
            sqlParamList.Add(new SqlParameter("@UserAge", reqModel.UserAge));
            SqlParameter[] sqlParams = sqlParamList.ToArray();
            var result = this._sqlServerAdapter.ExecuteNonQuery(sqlStr,sqlParams);
            return result;
        }
        catch
        {
            return -1;
        }
    }
    public int DeleteAccount(int id)
    {
        try
        {
            string sqlStr = @" delete from [AccountSystem].[dbo].[Account] 
                            where id = @id ";
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@id", id));
            SqlParameter[] sqlParams = sqlParamList.ToArray();
            var result = this._sqlServerAdapter.ExecuteNonQuery(sqlStr,sqlParams);
            return result;
        }
        catch
        {
            return -1;
        }
    }
}