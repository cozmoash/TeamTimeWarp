using System;
using System.Web.Mvc;
using TeamTimeWarp.Rest.Authentication;

namespace TeamTimeWarp.Rest.Controllers
{
    public class CreateAccountController : Controller
    {
        private IAccountCreator _accountCreator;
        private readonly IAccountCreationInformationValidator _validator;

        public CreateAccountController(IAccountCreator accountCreator, IAccountCreationInformationValidator validator)
        {
            _accountCreator = accountCreator;
            _validator = validator;
        }

        public ActionResult Index()
        {
            return new RedirectResult("Create");
        }

         [HttpPost]
         public ActionResult Create(FullAccountCreationInfo accountCreationInfo)
         {
             bool valid = true;
             if(!_validator.IsDisplayNameValid(accountCreationInfo.DisplayName))
             {
                 ModelState.AddModelError("DisplayName", "Display name is invalid");
                 valid = false;
             }

             if(!_validator.IsEmailValid(accountCreationInfo.EmailAddress))
             {
                 ModelState.AddModelError("EmailAddress", "Email Address is invalid");
                 valid = false;
             }

             if (!valid)
                 return View(accountCreationInfo);

            
             if(!_validator.IsDuplicateEmailAddress(accountCreationInfo.EmailAddress))
             {
                 ModelState.AddModelError("EmailAddress", "Email address already exists");
                 return View(accountCreationInfo);
             }
             
             _accountCreator.CreateAccount(accountCreationInfo);
             
             return new RedirectResult("Home");
         }

         [HttpGet]
         public ActionResult Create()
         {
             FullAccountCreationInfo accountCreationInfo = new FullAccountCreationInfo();
             return View(accountCreationInfo);
         }
        
       

    }
}
