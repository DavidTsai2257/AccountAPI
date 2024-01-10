using AccountAPI.Models;
using AccountAPI.Services;
using AccountAPI.SysEnum;
using CSICsSystemNotifySchedule.Adapters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FeatureAPI.Extensions;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊服務
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAPIService(this IServiceCollection services)
    {
        services.AddTransient<AccountService>();
        services.AddTransient<CommonService>();
        services.AddTransient<SQLServerAdapter>();
        // services.AddHttpContextAccessor();
        // //IP
        // services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        return services;
    }
    /// <summary>
    /// 註冊 Router
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRouter(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                return InvalidModelResponse(actionContext);
            };
        });
        return services;
    }
    private static JsonResult InvalidModelResponse(ActionContext actionContext)
    {
        var modelError = actionContext.ModelState.Where(modelError => modelError.Value.Errors.Count > 0).FirstOrDefault();
        actionContext.HttpContext.Response.StatusCode = 400;
        var result = new ResponseModel<JObject>
        {
            Status = ResponseStatusEnum.fail.ToString(),
            Message = modelError.Value == null ? "" : modelError.Value.Errors.FirstOrDefault().ErrorMessage
        };
        return new JsonResult(result);
    }
}