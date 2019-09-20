using System;
using System.Collections.Generic;
using System.Threading;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public interface IUiGlobalRoomsService
    {
        ICollection<RoomInfo> Search(string searchString);
        ICollection<RoomInfo> GetAll();
    }


    //public class Asyncifier<TResult>
    //{
    //    private readonly SynchronizationContext _context;
    //    public event EventHandler<EventArgs<TResult>> OnResult;

    //    public Asyncifier( SynchronizationContext context )
    //    {
    //        _context = context;
    //    }

    //    public void Run(Func<TResult> func)
    //    {
    //        ThreadPool.QueueUserWorkItem((state) =>
    //        {
    //            var result = func();

    //            _context.Post(_ =>
    //            {
    //                EventHandler<EventArgs<TResult>> handler = OnResult;
    //                if (handler != null) handler(this, new EventArgs<TResult>(result));
    //            }, null);
    //        });
    //    }
    //}

}