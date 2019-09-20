using System.ComponentModel;
using System.Windows.Controls;

namespace TeamTimeWarp.Client.Tray.Controls.CirularProgressBar
{
    public class CircularProgressBarViewModel :CircularProgressBarViewModelBase
    {
        protected override void SetProgressBar(ProgressBar progressBar)
        {
            ProgressBar = progressBar;
            SegmentColor = ProgressBar.Foreground;
            progressBar.SizeChanged += (s, e) => ComputeViewModelProperties();
            progressBar.ValueChanged += (o, args) => ComputeViewModelProperties();

            DependencyPropertyDescriptor descForeground = DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(ProgressBar));
            descForeground.AddValueChanged(progressBar, (sender, args) => ForegroundChanged());
            
            ComputeViewModelProperties();
        }


        private void ForegroundChanged()
        {
            SegmentColor = ProgressBar.Foreground;
        }
    }
}