using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TeamTimeWarp.Rest.Authentication
{
    /// <summary>
    /// Version 1 Authentication Request.
    /// Contains a username and password representing a valid Version 1 API user.
    /// </summary>

    [DataContract]
    public class AuthenticationRequest : IEquatable<AuthenticationRequest>
    {
        /// <summary>
        /// The Version 1 API user's EmailAddress in the system
        /// </summary>

        [Required]
        [DataMember(IsRequired = true)]
        public string EmailAddress { get; private set; }

        /// <summary>
        /// The Version 1 API user's Password in the system
        /// </summary>

        [Required]
        [DataMember(IsRequired = true)]
        public string Password { get; private set; }

        /// <summary>
        /// Constructs a Version 1 Authentication Request
        /// </summary>
        /// <param name="username">The Version 1 API user's EmailAddress in the system</param>
        /// <param name="password">The Version 1 API user's Password in the system</param>
        public AuthenticationRequest(string username, string password)
        {
            EmailAddress = username;
            Password = password;
        }

        public bool Equals(AuthenticationRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(EmailAddress, other.EmailAddress) && string.Equals(Password, other.Password);
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
                return ((EmailAddress != null ? EmailAddress.GetHashCode() : 0) * 397) ^ (Password != null ? Password.GetHashCode() : 0);
            }
        }
    }
}