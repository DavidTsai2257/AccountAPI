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
    private AccountService _accountService;
    private CommonService _commonService;

    public HomeController(AccountService accountService,CommonService commonService)
    {
        _accountService = accountService;
        _commonService = commonService;
    }
    /// <summary>
    /// 獲取人員清單
    /// </summary>
    /// <remarks>
    /// Author : David \
    /// </remarks>
    [HttpGet]
    public IActionResult GetAccountList()
    {
        var result = _accountService.GetAccountList();
        if (result.Count > 0)
            return Json(_commonService.ResponseResult<List<AccountModel>>(ResponseStatusEnum.success, "撈取成功", result));
        else
            return Json(_commonService.ResponseResult<JArray>(ResponseStatusEnum.fail, "查無資料", new JArray()));
    }
    /// <summary>
    /// 獲取人員單筆資料
    /// </summary>
    /// <remarks>
    /// Author : David \
    /// </remarks>
    [HttpGet("{id}")]
    public IActionResult GetAccount(int id)
    {
        var result = _accountService.GetAccountById(id);
        if (result != null)
            return Json(_commonService.ResponseResult<AccountModel>(ResponseStatusEnum.success, "撈取成功", result));
        else
            return Json(_commonService.ResponseResult<JArray>(ResponseStatusEnum.fail, "查無資料", new JArray()));
    }
    /// <summary>
    /// 新增人員
    /// </summary>
    /// <remarks>
    /// Author : David \
    /// </remarks>
    [HttpPost]
    public IActionResult AddAccount([FromBody] AddAccountRequestModel model)
    {
        var result = new ResponseModel<string>();
        var service = _accountService.AddAccount(model);
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
    /// <summary>
    /// 修改人員
    /// </summary>
    /// <remarks>
    /// Author : David \
    /// </remarks>
    [HttpPut("{id}")]
    public IActionResult EditFeature([FromRoute] int id, [FromBody] EditAccountRequestModel model)
    {
        var result = new ResponseModel<string>();
        var service = _accountService.EditAccount(id, model);
        if(service == 1)
        {
            result = _commonService.ResponseResult<string>(ResponseStatusEnum.success, "修改成功");
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
            result = _commonService.ResponseResult<string>(ResponseStatusEnum.fail, "修改失敗");
        }
        return Json(result);
    }
    /// <summary>
    /// 移除人員
    /// </summary>
    /// <remarks>
    /// Author : David \
    /// </remarks>
    [HttpDelete("{id}")]
    public IActionResult DeleteAccount(int id)
    {
        var result = new ResponseModel<string>();
        var service = _accountService.DeleteAccount(id);
        if (service == 1)
            return Json(_commonService.ResponseResult<string>(ResponseStatusEnum.success, "刪除成功"));
        else
            return Json(_commonService.ResponseResult<JArray>(ResponseStatusEnum.fail, "刪除失敗"));
    }
}
