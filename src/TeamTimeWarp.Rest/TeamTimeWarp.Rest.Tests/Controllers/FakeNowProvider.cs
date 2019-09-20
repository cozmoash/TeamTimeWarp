using System;
using TeamTimeWarp.Service;

namespace TeamTimeWarp.Rest.Tests.Controllers
{
    internal class FakeNowProvider : INowProvider
    {
        private DateTime _now = new DateTime(2000,12,12,12,12,0);

        public DateTime Now
        {
            get { return _now; }
            set { _now = value; }
        }
    }
}