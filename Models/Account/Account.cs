namespace AccountAPI.Models;

public class AccountShareModel
{
    public string UserCode{get;set;}

}
public class AccountModel : AccountShareModel
{
    public int ID{get;set;}
    public string UserName{get;set;}
    public int UserAge{get;set;}
}