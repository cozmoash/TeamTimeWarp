using System;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.UnitTests
{
    public static class TestHelper
    {
        public static string EmailAddressMock = "bean@teamtimewarp.com";
        public static string NameMock = "ash";
        public static string RoomName = "a room";

        public static Func<Account> AccountMock = () => new Account(0, NameMock, EmailAddressMock,AccountType.Full);

        public static string PasswordMock = "password";
    }
    
}
