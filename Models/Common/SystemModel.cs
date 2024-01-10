namespace AccountAPI.Models;
/// <summary>
/// Response Model
/// </summary>
public class ResponseModel<T>
{
    /// <summary>
    /// HTTP狀態
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// 回傳訊息
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 回傳資料(新增則回傳insertID)
    /// </summary>
    public T ResponseData { get; set; }
}
