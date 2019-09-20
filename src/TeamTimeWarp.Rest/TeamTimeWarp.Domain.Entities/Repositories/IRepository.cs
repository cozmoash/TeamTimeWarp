using System.Collections.Generic;

namespace TeamTimeWarp.Domain.Entities.Repositories
{
    public interface IRepository<T>
       
        where T : class
    {
        IList<T> GetAll();
        
        void Add(T item);

        //void Merge(T item);

    }
}