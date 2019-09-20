using System.Runtime.Serialization;

namespace TeamTimeWarp.Public.Models.v001
{
    /// <summary>
    /// Version 1 Authentication Request.
    /// Contains a username and password representing a valid Version 1 API user.
    /// </summary>

    [DataContract]
    internal class AuthenticationRequest
    {
        /// <summary>
        /// The Version 1 API user's Username in the system
        /// </summary>

        
        [DataMember(IsRequired = true)]
        public string Username { get; private set; }

        /// <summary>
        /// The Version 1 API user's Password in the system
        /// </summary>

        
        [DataMember(IsRequired = true)]
        public string Password { get; private set; }

        /// <summary>
        /// Constructs a Version 1 Authentication Request
        /// </summary>
        /// <param name="username">The Version 1 API user's Username in the system</param>
        /// <param name="password">The Version 1 API user's Password in the system</param>
        public AuthenticationRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public bool Equals(AuthenticationRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Username, other.Username) && string.Equals(Password, other.Password);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AuthenticationRequest)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Username != null ? Username.GetHashCode() : 0) * 397) ^ (Password != null ? Password.GetHashCode() : 0);
            }
        }
    }
}