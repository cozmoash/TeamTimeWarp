﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.ComponentModel;
using TeamTimeWarp.Client.Tray.Controls.CirularProgressBar;

namespace CircularProgressBar
{
  /// <summary>
  /// An attached view model that adapts a ProgressBar control to provide properties
  /// that assist in the creation of a segmented circular template
  /// </summary>
  public class SegmentedProgressBarViewModel : CircularProgressBarViewModel
  {
    private int _segmentCount = 8;

    private List<SegmentData> _segments;

    public int SegmentCount
    {
      get { return _segmentCount; }
      set
      {
        _segmentCount = value;
        BuildSegments();
        ComputeViewModelProperties();
      }
    }

    public List<SegmentData> Segments
    {
      get { return _segments; }
      set { _segments = value; OnPropertyChanged("Segments"); }
    }

    public SegmentedProgressBarViewModel()
    {
      BuildSegments();      
    }

    private void BuildSegments()
    {
      var segments = new List<SegmentData>();
      double endAngle = 360.0 / (double)SegmentCount;
      for (int i = 0; i < SegmentCount; i++)
      {
        double startAngle = (double)i * 360 / (double)SegmentCount;
        segments.Add(new SegmentData(startAngle, endAngle, this));
      }

      Segments = segments;
    }

    protected override void ComputeViewModelProperties()
    {
      if (ProgressBar == null)
        return;

      double normalValue = ProgressBar.Value / (ProgressBar.Maximum - ProgressBar.Minimum);

      for (int i = 0; i < SegmentCount; i++)
      {
        double startValue = (double)i / (double)SegmentCount;
        double endValue = (double)(i + 1) / (double)SegmentCount;
        double opacity = (normalValue - startValue) / (endValue - startValue);;
        opacity = Math.Min(1, Math.Max(0,opacity));
        _segments[i].Opacity = opacity;
      }
      base.ComputeViewModelProperties();
    }


    /// <summary>
    /// The data for an individual segment.
    /// </summary>
    public class SegmentData : INotifyPropertyChanged
    {
      public double StartAngle { get; private set; }

      public double WedgeAngle { get; private set; }

      private double _opacity;

      public double Opacity
      {
        get { return _opacity; }
        set { _opacity = value; OnPropertyChanged("Opacity"); }
      }

      public SegmentedProgressBarViewModel Parent { get; private set; }

      public SegmentData(double start, double wedge,
        SegmentedProgressBarViewModel parent)
      {
        StartAngle = start;
        WedgeAngle = wedge;
        Parent = parent;
        Opacity = 0;
      }

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
}
