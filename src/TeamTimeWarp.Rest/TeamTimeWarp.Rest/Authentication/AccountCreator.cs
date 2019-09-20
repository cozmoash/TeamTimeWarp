using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Controllers;
using log4net;

namespace TeamTimeWarp.Rest.Authentication
{
    public interface IAccountCreator
    {
        AccountCreationResponse CreateAccount(FullAccountCreationInfo fullAccountCreationInfo);
        AccountCreationResponse CreateAccount(QuickCreationInfo quickAccountCreationInfo);
    }

    public interface IAccountCreationInformationValidator
    {
        bool IsEmailValid(string emailAddress);
        bool IsDuplicateEmailAddress(string emailAddress);
        bool IsPasswordValid(string password );
        bool IsDisplayNameValid(string displayName);
    }

    public class AccountCreationInformationValidator : IAccountCreationInformationValidator
    {
        private readonly IAccountRepository _accountRepository;

        public AccountCreationInformationValidator(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool IsDuplicateEmailAddress(string emailAddress)
        {
            if (_accountRepository.GetByEmail(emailAddress).Any())
                return false;

            return true;
        }

        public bool IsEmailValid(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                return false;

            if(emailAddress.HasInvalidChars())
                return false;

            return true;
        }

        public bool IsPasswordValid(string password )
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            return true;
        }

        public bool IsDisplayNameValid(string displayName)
        {
            if(string.IsNullOrWhiteSpace(displayName))
                return false;

            if (displayName.HasInvalidChars())
            {
                return false;
            }

            return true;
        }


    }

    public class AccountCreator : IAccountCreator
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountPasswordRepository _passwordRepository;
        private readonly ITimeWarpUserStateRepository _userStateRepository;
        private readonly ITimeWarpAuthenticationManager _authenticationManager;
        private readonly IAccountCreationInformationValidator _validator;

        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));

        public AccountCreator(IAccountRepository accountRepository,
                              IAccountPasswordRepository passwordRepository,
                              ITimeWarpUserStateRepository userStateRepository,
                              ITimeWarpAuthenticationManager authenticationManager
                              
            )
        {
            _accountRepository = accountRepository;
            _passwordRepository = passwordRepository;
            _userStateRepository = userStateRepository;
            _authenticationManager = authenticationManager;
            _validator = new AccountCreationInformationValidator(accountRepository);
        }

        public AccountCreationResponse CreateAccount(FullAccountCreationInfo fullAccountCreationInfo)
        {
            if (!_validator.IsEmailValid(fullAccountCreationInfo.EmailAddress))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "email address is invalid" });

            if(!_validator.IsDuplicateEmailAddress(fullAccountCreationInfo.EmailAddress))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "email address is duplicate" });

            if(!_validator.IsDisplayNameValid(fullAccountCreationInfo.DisplayName))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "display name is invalid" });

            //does email address exist?
            var account = new Account(0, fullAccountCreationInfo.DisplayName, fullAccountCreationInfo.EmailAddress, AccountType.Full);
            var defaultState = TimeWarpUserState.DefaultState(account);
            var accountPassword = new AccountPassword(account, fullAccountCreationInfo.Password);

            //save
            _accountRepository.Add(account);
            _passwordRepository.Add(accountPassword);
            _userStateRepository.Add(defaultState);

            string token = _authenticationManager.AddUser(accountPassword);

            if (Log.IsDebugEnabled)
                Log.DebugFormat("account created for {0}", fullAccountCreationInfo.EmailAddress);

            return new AccountCreationResponse(account.Id, token);
        }

        public AccountCreationResponse CreateAccount(QuickCreationInfo quickAccountCreationInfo)
        {
            if (!_validator.IsDisplayNameValid(quickAccountCreationInfo.DisplayName))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "display name is invalid" });

            var randomString = Guid.NewGuid().ToString();

            string fakeEmail = string.Empty;
            string fakePassword = randomString;
            var account = new Account(0, quickAccountCreationInfo.DisplayName, fakeEmail , AccountType.Quick);
            var defaultState = TimeWarpUserState.DefaultState(account);
            var accountPassword = new AccountPassword(account, fakePassword);

            //save

            _accountRepository.Add(account);
            _passwordRepository.Add(accountPassword);
            _userStateRepository.Add(defaultState);

            string token = _authenticationManager.AddUser(accountPassword);

            if (Log.IsDebugEnabled)
                Log.DebugFormat("account created for {0}", randomString);


            return new AccountCreationResponse(account.Id, token);
        }
    }
}