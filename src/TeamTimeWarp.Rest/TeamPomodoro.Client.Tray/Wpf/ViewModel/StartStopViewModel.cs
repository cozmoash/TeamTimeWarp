using System.Windows.Input;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services;
using TimeManager.Client.Tray.Wpf;

namespace TeamTimeWarp.Client.Tray.Wpf.ViewModel
{
    public class StartStopViewModel : ViewModelBase
    {
        public StartStopViewModel(IUserStateService userStateService)
        {
            WorkCommand = new DelegateCommand(_ => 
                userStateService.StartWorkAsync());
            RestCommand = new DelegateCommand(_ => userStateService.StopWorkAsync());
        }

        public ICommand WorkCommand { get; private set; }
        public ICommand RestCommand { get; private set; }
    }
}