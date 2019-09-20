using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TeamTimeWarp.Client.Tray.Controls.CirularProgressBar
{

    public class CircularProgressBarViewModel : CircularProgressBarViewModelBase
    {

        protected override void SetProgressBar(ProgressBar progressBar)
        {
            ProgressBar = progressBar;
            SegmentColor = ProgressBar.Foreground;
            progressBar.SizeChanged += (s, e) => ComputeViewModelProperties();
            progressBar.ValueChanged += (o, args) => ComputeViewModelProperties();

            RegisterForNotification("Foreground", progressBar, (d, e) => ForegroundChanged());


            ComputeViewModelProperties();
        }

        private void ForegroundChanged()
        {
            SegmentColor = ProgressBar.Foreground;
        }


        /// Add a handler for a DP change
        /// see: http://amazedsaint.blogspot.com/2009/12/silverlight-listening-to-dependency.html
        private void RegisterForNotification
            (string propertyName, FrameworkElement element, PropertyChangedCallback callback)
        {

            //Bind to a dependency property  
            Binding b = new Binding(propertyName) { Source = element };
            var prop = System.Windows.DependencyProperty.RegisterAttached(
                "ListenAttached" + propertyName,
                typeof(object),
                typeof(UserControl),
                new PropertyMetadata(callback));

            element.SetBinding(prop, b);
        }
    }
}
