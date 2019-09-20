using System;
using NHibernate;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;

namespace TeamTimeWarp.Rest.RandomDataGenerator
{
    public static class FakeAccountUserStateResetter
    {
        public static void Reset()
        {
            var x =
                new TimeWarpSessionFactory<TimeWarpUserStateClassMap>();

            var query = GetQueryString();

            using (var session = x.Get())
            {
                ISQLQuery sqlquery = session.CreateSQLQuery(query);
                sqlquery.ExecuteUpdate();
            }
        }

        private static string GetQueryString()
        {
            var now = DateTime.UtcNow;
            var sqlFormattedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
            string query =
                string.Format(
                    "update UserState Set UserState.PeriodStartTime = '{0}' From	Account	Join UserState on Account.Id = UserState.Account_Id Where Account.AccountType = 'Fake'",
                    sqlFormattedDate);
            return query;
        }
    }
}