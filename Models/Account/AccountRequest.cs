namespace AccountAPI.Models;

public class AccountRequestModel
{
    public string UserCode{get;set;}

}
public class AddAccountRequestModel : AccountRequestModel
{
    public string UserName{get;set;}
    public int UserAge{get;set;}
}
public class EditAccountRequestModel
{
    public string UserName{get;set;}
    public int UserAge{get;set;}
}