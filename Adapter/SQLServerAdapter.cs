using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace CSICsSystemNotifySchedule.Adapters;
/// <summary>    
/// SQL Server DB連接  
/// </summary>    
public class SQLServerAdapter
{
    private string _connStr;
    public SQLServerAdapter(IConfiguration config)
    {
        _connStr = config.GetSection("DBConnection").Value;
    }
    public DataTable GetDataTable(string cmd, SqlParameter[] parameters = null){
        var result = new DataTable();
        using (SqlConnection connection = new SqlConnection(_connStr))
        {
            SqlCommand command = new SqlCommand(cmd, connection);
            if(parameters != null){
                foreach(var parameterData in parameters){
                    command.Parameters.Add(parameterData);
                }
            }
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader()){
                    result.Load(reader);
                }
                result = ColumnOptionReset(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        return result;
    }
    public List<T> GetObjectType<T>(string cmd, SqlParameter[] parameters = null){
        var result = new List<T>();
        var resultTB = new DataTable();
        using (SqlConnection connection = new SqlConnection(_connStr))
        {
            SqlCommand command = new SqlCommand(cmd, connection);
            if(parameters != null){
                foreach(var parameterData in parameters){
                    command.Parameters.Add(parameterData);
                }
            }
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader()){
                    resultTB.Load(reader);
                }
                result = ConvertDataTable<T>(resultTB);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        return result;
    }
    public int ExecuteNonQuery(string cmd, SqlParameter[] parameters = null){
        var result = 0;
        using (SqlConnection connection = new SqlConnection(_connStr))
        {
            SqlCommand command = new SqlCommand(cmd, connection);
            if(parameters != null){
                foreach(var parameterData in parameters){
                    command.Parameters.Add(parameterData);
                }
            }
            try
            {
                connection.Open();
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        return result;
    }
    public SqlParameter GetParameter(string parameterName, object value){
        return new SqlParameter(parameterName:parameterName, value:value);
    }

    private DataTable ColumnOptionReset(DataTable dt){
        foreach (DataColumn col in dt.Columns) {
            col.ReadOnly = false;
        }
        return dt;
    }
    private List<T> ConvertDataTable<T>(DataTable dt)  
    {  
        List<T> data = new List<T>();  
        foreach (DataRow row in dt.Rows)  
        {  
            T item = GetItem<T>(row);  
            data.Add(item);  
        }  
        return data;  
    }  
    private T GetItem<T>(DataRow dr)  
    {  
        Type temp = typeof(T);  
        T obj = Activator.CreateInstance<T>();  
    
        foreach (DataColumn column in dr.Table.Columns)  
        {  
            foreach (PropertyInfo pro in temp.GetProperties())  
            {  
                if (pro.Name == column.ColumnName){
                    var drVal = dr[column.ColumnName] == DBNull.Value ? null: dr[column.ColumnName];
                    pro.SetValue(obj, drVal, null);  
                }else  
                    continue;  
            }  
        }  
        return obj;  
    }
}