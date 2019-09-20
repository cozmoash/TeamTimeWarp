using System.Linq;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Persistence.Accounts
{
    public class AuthenticationSessionRepository : PersistenceRepositoryBase<AuthenticationSession, AuthenticationSessionClassMap>, IAuthenticationSessionRepository
    {
        public AuthenticationSessionRepository() : base( new TimeWarpSessionFactory<AuthenticationSessionClassMap>())
        {
        }

        public bool TryGetByToken(string token , out AuthenticationSession result)
        {
            using (var session = SessionFactory.Get())
            {
                result = (from authenticationSession in session.Query<AuthenticationSession>()
                        where authenticationSession.Token == token
                        select authenticationSession).SingleOrDefault();
            }

            return result != null;
        }
    }
}