using System.Collections.Generic;
using System.Security.Principal;

namespace TeamTimeWarp.Rest.Authentication
{
    internal class AuthenticationPrincipal : IPrincipal
    {
        public static readonly Authentication UnAuthenticatedAuthentication;
        public static readonly AuthenticationPrincipal UnAuthenticatedPrincipal;
        public static readonly AuthenticationIdentity UnAuthenticationIdentity;
        
        static AuthenticationPrincipal()
        {        
            UnAuthenticatedAuthentication = new Authentication(UnAuthenticatedId, UnAuthenticatedClientAccountId, UnAuthenticatedUsername, UnAuthenticatedToken);
            UnAuthenticationIdentity = new AuthenticationIdentity(UnAuthenticatedUsername, UnAuthenticatedAuthenticationType, false);
            UnAuthenticatedPrincipal = new AuthenticationPrincipal(UnAuthenticatedAuthentication, UnAuthenticationIdentity, new List<string>());
        }

        public const int UnAuthenticatedId = -1;
        public const int UnAuthenticatedClientAccountId = -1;
        public const string UnAuthenticatedUsername = "UnAuthenticatedUsername";
        public const string UnAuthenticatedToken = "UnAuthenticatedToken";
        public const string UnAuthenticatedAuthenticationType = "UnAuthenticated";
        
        public Authentication Authentication { get; private set; }
        public List<string> Roles { private set; get; }

        public AuthenticationPrincipal(Authentication authentication, IIdentity authenticationIdentity, List<string> roles)
        {
            Authentication = authentication;
            Identity = authenticationIdentity;
            Roles = roles;
        }

        public bool IsInRole(string role)
        {
            var isInRole = Roles.Contains(role);
            return isInRole;
        }

        public IIdentity Identity { get; private set; }
    }
}