using System;
using System.Collections.Generic;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services.Interfaces
{
    public interface IUiGlobalRoomsService
    {
        event EventHandler<AsyncCompletedEventArgs<ICollection<RoomInfo>>> SearchCompleted; 
        void SearchAsync(string searchString);
    }
}