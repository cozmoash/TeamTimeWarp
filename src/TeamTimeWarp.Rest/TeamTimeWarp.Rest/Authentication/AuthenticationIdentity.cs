using System.Security.Principal;

namespace TeamTimeWarp.Rest.Authentication
{
    internal class AuthenticationIdentity : IIdentity
    {
        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public AuthenticationIdentity(string username, string authenticationType, bool isAuthenticated)
        {
            Name = username;
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
        }
    }
}