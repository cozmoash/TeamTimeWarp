using System;
using NHibernate;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;

namespace TeamTimeWarp.Database.RandomGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeWarpSessionFactory<TimeWarpUserStateClassMap> x = new TimeWarpSessionFactory<TimeWarpUserStateClassMap>();

            var now = DateTime.UtcNow;
            var sqlFormattedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
            string query =
                    string.Format("update UserState Set UserState.PeriodStartTime = '{0}' From	Account	Join UserState on Account.Id = UserState.Account_Id Where Account.IsFake = 1", sqlFormattedDate);

            using (var session = x.Get())
            {
                ISQLQuery sqlquery = session.CreateSQLQuery(query);
                sqlquery.ExecuteUpdate();
            }
        }
    }
}
