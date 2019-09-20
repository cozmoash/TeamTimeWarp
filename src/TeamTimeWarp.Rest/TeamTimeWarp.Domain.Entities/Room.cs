using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamTimeWarp.Domain.Entities
{
    public class Room
    {
        private HashSet<Account> _users;
        
        public Room(int id, string name, DateTime creationTime)
        {
            Id = id;
            Name = name;
            CreationTime = creationTime;
            Users = Enumerable.Empty<Account>();
        }

        protected Room()
        {
            //nh
        }

        public virtual IEnumerable<Account> Users
        {
            get { return _users; }
            protected set
            {
                _users = new HashSet<Account>(value,
                                                      new FuncEqualityCompare<Account>(
                                                          (x, y) => Equals(x.Id, y.Id), x => x.Id
                                                                                                   .GetHashCode()));
            }
        }

        
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime CreationTime { get; protected set; }
        public virtual int NumberOfUsers { get { return _users.Count; } }


        public virtual void Add(Account user)
        {
            _users.Add(user);
        }

        public virtual void Remove(Account user)
        {
            _users.Remove(user);
        }
    }
}