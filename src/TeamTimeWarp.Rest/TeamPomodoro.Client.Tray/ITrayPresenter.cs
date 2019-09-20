using System;
using TeamTimeWarp.Client.Tray.Wpf.ViewModel;

namespace TeamTimeWarp.Client.Tray
{
    public interface ITrayPresenter : IDisposable
    {
        event EventHandler<EventArgs> ShowPostLoginTrayInformation;
        event EventHandler<ShowBalloonEventArgs> ShowBallon;
        event EventHandler<TooltipTextChangedEventArgs> TooltipTextChanged;
        void StartWork();
        void StopWork();
        void TrayIconClick();
        void TrayIconBallonTipClicked();
        MainWindowViewModel TeamViewModel { get; }

    }
}