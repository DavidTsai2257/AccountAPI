using AccountAPI.Models;
using AccountAPI.SysEnum;

namespace AccountAPI.Services;
public class CommonService
{
    public ResponseModel<T> ResponseResult<T>(ResponseStatusEnum status, string failMsg, T responseData = default(T), long? total = null)
    {
        return new ResponseModel<T>()
        {
            Status = status.ToString(),
            Message = failMsg,
            ResponseData = responseData
        };
    }
}