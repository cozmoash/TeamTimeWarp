namespace TeamTimeWarp.Rest.Controllers
{
    public class FullAccountCreationInfo : QuickCreationInfo
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class QuickCreationInfo
    {
        public string DisplayName { get; set; }
    }
}