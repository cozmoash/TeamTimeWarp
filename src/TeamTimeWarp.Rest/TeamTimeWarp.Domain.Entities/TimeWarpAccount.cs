namespace TeamTimeWarp.Domain.Entities
{
    public class TimeWarpAccount
    {
        public virtual long Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string Password { get; protected set; }

        public TimeWarpAccount(long id, string name, string emailAddress,string password)
        {
            Id = id;
            Name = name;
            Email = emailAddress;
            Password = password;
        }

        protected TimeWarpAccount()
        {
            
        }
    }
}
