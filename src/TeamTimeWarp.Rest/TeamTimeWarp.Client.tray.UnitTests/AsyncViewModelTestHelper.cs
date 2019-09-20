using System;
using System.ComponentModel;
using System.Threading;
using NUnit.Framework;

namespace TeamTimeWarp.Client.tray.UnitTests
{
    public static class AsyncViewModelTestHelper
    {
        public static void WaitFor(this INotifyPropertyChanged notifyPropertyChanged, string propertyName, Action run)
        {
            using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
            {
                notifyPropertyChanged.PropertyChanged += delegate(object o, PropertyChangedEventArgs args)
                    {
                        if (args.PropertyName == propertyName)
                        {
                            try
                            {
                                if (manualResetEvent != null)
                                    manualResetEvent.Set();
                            }
                            catch (ObjectDisposedException)
                            {
                            }
                        
                        }
                    }; 

                run();

                Assert.IsTrue(manualResetEvent.WaitOne(TimeSpan.FromSeconds(2)));
            }
        }

      
    }
}