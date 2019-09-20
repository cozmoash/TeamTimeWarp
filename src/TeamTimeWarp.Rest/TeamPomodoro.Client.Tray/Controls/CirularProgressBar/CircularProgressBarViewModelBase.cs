using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using LinqToVisualTree;
using System.Windows.Data;
using System.ComponentModel;
using System;

namespace TeamTimeWarp.Client.Tray.Controls.CirularProgressBar
{
    /// <summary>
    /// An attached view model that adapts a ProgressBar control to provide properties
    /// that assist in the creation of a circular template
    /// </summary>
    public abstract class CircularProgressBarViewModelBase : FrameworkElement, INotifyPropertyChanged
    {
        #region Attach attached property

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof (object), typeof (CircularProgressBarViewModel),
                                                new PropertyMetadata(null, OnAttachChanged));

        public static CircularProgressBarViewModel GetAttach(DependencyObject d)
        {
            return (CircularProgressBarViewModel) d.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject d, CircularProgressBarViewModel value)
        {
            d.SetValue(AttachProperty, value);
        }

        /// <summary>
        /// Change handler for the Attach property
        /// </summary>
        private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // set the view model as the DataContext for the rest of the template
            var targetElement = d as FrameworkElement;
            var viewModel = e.NewValue as CircularProgressBarViewModel;
            targetElement.DataContext = viewModel;

            // handle the loaded event
            targetElement.Loaded += HandleElementLoaded;
        }

        /// <summary>
        /// Handle the Loaded event of the element to which this view model is attached
        /// in order to enable the attached
        /// view model to bind to properties of the parent element
        /// </summary>
        private static void HandleElementLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement targetElement = sender as FrameworkElement;
            CircularProgressBarViewModel attachedModel = GetAttach(targetElement);

            // find the ProgressBar and associated it with the view model
            var progressBar = targetElement.Ancestors<ProgressBar>().Single() as ProgressBar;
            attachedModel.SetProgressBar(progressBar);
        }



        #endregion


        #region fields

        private double _angle;

        private double _centreX;

        private double _centreY;

        private double _radius;

        private double _innerRadius;

        private double _diameter;

        private double _percent;

        private double _holeSizeFactor;

        protected ProgressBar ProgressBar;
        private Brush _segmentColor;

        #endregion

        #region properties

        public double Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                OnPropertyChanged("Percent");
            }
        }

        public double Diameter
        {
            get { return _diameter; }
            set
            {
                _diameter = value;
                OnPropertyChanged("Diameter");
            }
        }

        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged("Radius");
            }
        }

        public double InnerRadius
        {
            get { return _innerRadius; }
            set
            {
                _innerRadius = value;
                OnPropertyChanged("InnerRadius");
            }
        }

        public double CentreX
        {
            get { return _centreX; }
            set
            {
                _centreX = value;
                OnPropertyChanged("CentreX");
            }
        }

        public double CentreY
        {
            get { return _centreY; }
            set
            {
                _centreY = value;
                OnPropertyChanged("CentreY");
            }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                OnPropertyChanged("Angle");
            }
        }


        public Brush SegmentColor
        {
            get { return _segmentColor; }
            set
            {
                _segmentColor = value;


                OnPropertyChanged("SegmentColor");
            }
        }

        public double HoleSizeFactor
        {
            get { return _holeSizeFactor; }
            set
            {
                _holeSizeFactor = value;
                ComputeViewModelProperties();
            }
        }



        #endregion


        /// <summary>
        /// Re-computes the various properties that the elements in the template bind to.
        /// </summary>
        protected virtual void ComputeViewModelProperties()
        {
            if (ProgressBar == null)
                return;

            Angle = (ProgressBar.Value - ProgressBar.Minimum)*360/(ProgressBar.Maximum - ProgressBar.Minimum);
            CentreX = ProgressBar.ActualWidth/2;
            CentreY = ProgressBar.ActualHeight/2;
            Radius = Math.Min(CentreX, CentreY);
            Diameter = Radius*2;
            InnerRadius = Radius*HoleSizeFactor;
            Percent = Angle/360;
        }

        /// <summary>
        /// Add handlers for the updates on various properties of the ProgressBar
        /// </summary>
        protected abstract void SetProgressBar(ProgressBar progressBar);
        //{
        //    ProgressBar = progressBar;
        //    SegmentColor = ProgressBar.Foreground;
        //    progressBar.SizeChanged += (s, e) => ComputeViewModelProperties();
        //    progressBar.ValueChanged += (o, args) => ComputeViewModelProperties();

        //      RegisterForNotification("Value", progressBar, (d,e) => ComputeViewModelProperties());
        //     RegisterForNotification("Maximum", progressBar, (d, e) => ComputeViewModelProperties());
        //     RegisterForNotification("Minimum", progressBar, (d, e) => ComputeViewModelProperties());
        //    RegisterForNotification("Foreground", progressBar, (d, e) => ForegroundChanged());

        //    DependencyPropertyDescriptor descForeground = DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(ProgressBar));
        //    descForeground.AddValueChanged(progressBar, (sender, args) => ForegroundChanged());

        //    DependencyPropertyDescriptor descValue = DependencyPropertyDescriptor.FromProperty(RangeBase.ValueProperty, typeof(ProgressBar));
        //    descValue.AddValueChanged(this, (sender, args) => ComputeViewModelProperties());
            
        //    ComputeViewModelProperties();
        //}

        //private void ForegroundChanged()
        //{
        //    SegmentColor = ProgressBar.Foreground;
        //}


        /// Add a handler for a DP change
        /// see: http://amazedsaint.blogspot.com/2009/12/silverlight-listening-to-dependency.html
        //private void RegisterForNotification(string propertyName, FrameworkElement element, PropertyChangedCallback callback)
        //  {

        //      DependencyPropertyDescriptor desc =
        //          DependencyPropertyDescriptor.FromName(propertyName, typeof (object), typeof (ProgressBar));

        //    desc.AddValueChanged
        //      (this.myLabel, callback);

        //    ////Bind to a dependency property  
        //    Binding b = new Binding(propertyName) { Source = element };
        //    var prop = DependencyProperty.RegisterAttached(
        //        "ListenAttached" + propertyName,
        //        typeof(object),
        //        typeof(UserControl),
        //        new PropertyMetadata(callback));

        //    element.SetBinding(prop, b);
        //}

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}
