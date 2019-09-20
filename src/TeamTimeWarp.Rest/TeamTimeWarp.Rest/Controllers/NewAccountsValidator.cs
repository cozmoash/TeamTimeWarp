using System;
using System.Linq;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Rest.Controllers
{
    //public class NewAccountsValidator : INewAccountsValidator
    //{
    //    private readonly ConcurrentCache<long,Account> _accounts;

    //    public NewAccountsValidator(ConcurrentCache<long, Account> accounts)
    //    {
    //        if(accounts == null)
    //            throw new ArgumentNullException("accounts");

    //        _accounts = accounts;
    //    }

    //    public bool EmailExists(string email)
    //    {
    //        //todo: improve performance?
    //        return _accounts.Any(account => account.Email.Equals(email));
    //    }


    //}
}