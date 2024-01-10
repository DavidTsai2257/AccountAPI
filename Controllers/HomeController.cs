using Microsoft.AspNetCore.Mvc;
using AccountAPI.Models;
using AccountAPI.Services;
using AccountAPI.SysEnum;
using Newtonsoft.Json.Linq;

namespace AccountAPI.Controllers;
[ApiController]
[Route("Home")]
public class HomeController : Controller
{
    private SQLServerService _sqlServerService;
    private CommonService _commonService;

    public HomeController(SQLServerService sqlServerService,CommonService commonService)
    {
        _sqlServerService = sqlServerService;
        _commonService = commonService;
    }
    [HttpGet]
    public IActionResult GetAccountList()
    {
        var result = _sqlServerService.GetAccountList();
        if (result.Count > 0)
            return Json(_commonService.ResponseResult<List<AccountModel>>(ResponseStatusEnum.success, "撈取成功", result));
        else
            return Json(_commonService.ResponseResult<JArray>(ResponseStatusEnum.fail, "查無資料", new JArray()));
    }
    [HttpGet("{id}")]
    public IActionResult GetAccount(int id)
    {
        var result = _sqlServerService.GetAccountById(id);
        if (result != null)
            return Json(_commonService.ResponseResult<AccountModel>(ResponseStatusEnum.success, "撈取成功", result));
        else
            return Json(_commonService.ResponseResult<JArray>(ResponseStatusEnum.fail, "查無資料", new JArray()));
    }
    [HttpPost]
    public IActionResult AddAccount([FromBody] AddAccountRequestModel model)
    {
        var result = new ResponseModel<string>();
        var service = _sqlServerService.AddAccount(model);
        if(service == -1)
        {
            HttpContext.Response.StatusCode = 400;
            result = _commonService.ResponseResult<string>(ResponseStatusEnum.fail, "新增失敗");
        }
        else
        {
            result = _commonService.ResponseResult<string>(ResponseStatusEnum.success, "新增成功");
        }
        return Json(result);
    }
    [HttpPut("{id}")]
    public IActionResult EditFeature([FromRoute] int id, [FromBody] EditAccountRequestModel model)
    {
        var result = new ResponseModel<string>();
        var service = _sqlServerService.EditAccount(id, model);
        if(service == false)
        {
            HttpContext.Response.StatusCode = 400;
            result = _commonService.ResponseResult<string>(ResponseStatusEnum.fail, "修改失敗");
        }
        else
        {
            result = _commonService.ResponseResult<string>(ResponseStatusEnum.success, "修改成功");
        }
        return Json(result);
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteAccount(int id)
    {
        var result = new ResponseModel<string>();
        var service = _sqlServerService.DeleteAccount(id);
        if (service == true)
            return Json(_commonService.ResponseResult<string>(ResponseStatusEnum.success, "刪除成功"));
        else
            return Json(_commonService.ResponseResult<JArray>(ResponseStatusEnum.fail, "刪除失敗"));
    }
}
