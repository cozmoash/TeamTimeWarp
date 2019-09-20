namespace TeamTimeWarp.Domain.Entities
{
    public class AccountPassword
    {
        public virtual long Id { get; protected set; }
        public virtual Account Account { get; protected set; }
        public virtual string Password { get; protected set; }


        protected AccountPassword()
        {
            
        }


        public AccountPassword(Account accountId, string password)
        {
            Account = accountId;
            Password = password;
        }
    }
}