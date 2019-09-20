using System;
using System.Threading;
using log4net;

namespace TeamTimeWarp.Rest.RandomDataGenerator
{
    public class FakeAccountUserStateGenerator : IDisposable
    {
        private Timer _resetTimer;
        private readonly object _disposeTimer = new object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(FakeAccountUserStateGenerator));

        public FakeAccountUserStateGenerator()
        {
            _resetTimer = new Timer(ResetFakeAccountUserStates);
        }

        public void Start()
        {
            _resetTimer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(-1));
        }
        
        public void ResetFakeAccountUserStates(object state)
        {
            try
            {
                FakeAccountUserStateResetter.Reset();
            }
            catch (Exception ex)
            {
                Log.Error("unable to update fake data", ex);
            }

            lock (_disposeTimer)
            {
                if(_resetTimer != null)
                    _resetTimer.Change(TimeSpan.FromMinutes(30), TimeSpan.FromMilliseconds(-1));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_disposeTimer)
                {
                    if (_resetTimer != null)
                    {
                        _resetTimer.Dispose();
                        _resetTimer = null;
                    }
                }
            }
            // get rid of unmanaged resources
        }
    }
}