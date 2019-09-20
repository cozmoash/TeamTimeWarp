using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray;
using TimeManager.Client.Tray.Wpf;
using WPFGrowlNotification;
using Application = System.Windows.Forms.Application;

namespace TeamTimeWarp.Client.Tray
{
    public class SysTrayApp : Form
    {
        private const double TopOffset =100;
        private const double LeftOffset = 580;
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenu _trayMenu;

        private readonly TooltipTextFactory _toolTipFactory;
        private readonly TrayIconFactory _iconFactory;
        private readonly ITrayPresenter _trayPresenter;
        private readonly MessagesReceiver _messagesReceiver;
        private MainWindow _mainWindow;//lazily created.
        private bool _mainWindowOpen;
        private readonly GrowlNotifiactions _growlNotifications;
        [STAThread]
        public static void Main()
        {
            
            Application.Run(new SysTrayApp());
        }

        public SysTrayApp()
        {
            _growlNotifications = new GrowlNotifiactions
                {
                    Top = SystemParameters.WorkArea.Top + TopOffset,
                    Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - LeftOffset
                };
            //services
            _iconFactory = new TrayIconFactory();
            _toolTipFactory = new TooltipTextFactory();

            _trayMenu = new ContextMenu();
            PopulateWithPreLoginTrayItems(_trayMenu);

            _notifyIcon = new NotifyIcon
                {
                    Text = _toolTipFactory.Get(TimeWarpState.None, 0),
                    Icon = _iconFactory.Get(TimeWarpState.None),
                    ContextMenu = _trayMenu,
                    Visible = true
                };

            var synchronizationContext = SynchronizationContext.Current;

            var serviceContainer = new ServiceContainer(synchronizationContext);
            var userStateListener = new UserStateListener(serviceContainer.UserStateService,
                                                                            synchronizationContext);

            _messagesReceiver = new MessagesReceiver(userStateListener, _growlNotifications,serviceContainer.UserMessageService);
            _trayPresenter = new TrayPresenter(serviceContainer,userStateListener,_iconFactory,_toolTipFactory);
            
            _trayPresenter.TooltipTextChanged += HandleTooltipTextChanged;
            _trayPresenter.ShowPostLoginTrayInformation += HandleLoginCompleted;
            _trayPresenter.ShowBallon += HandleShowBallon;
            _notifyIcon.Click += NotifyIconClick;
            _notifyIcon.BalloonTipClicked += NotifyIconBalloonTipClicked;
            
            ShowMainWindow();
        }

        private void HandleLoginCompleted(object sender, EventArgs e)
        {
            PopulateWithPostLoginTrayItems(_trayMenu);
        }

        private void ShowMainWindow()
        {
            if (_mainWindowOpen)
            {
                _mainWindow.Focus();
            }
            else
            {
                _mainWindowOpen = true;
                _mainWindow = new MainWindow { DataContext = _trayPresenter.TeamViewModel };
                _mainWindow.Closed += HandleMainWindowClosed;
                System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(_mainWindow);
                _mainWindow.Show();
            }
        }

        private void HandleMainWindowClosed(object sender, EventArgs e)
        {
            _mainWindow.Closed -= HandleMainWindowClosed;
            _mainWindow = null;
            _mainWindowOpen = false;
        }

        private void HandleShowBallon(object sender, ShowBalloonEventArgs e)
        {

            _growlNotifications.AddNotification(new Notification
                {
                    Title = e.Title,
                    ImageUrl = e.ToastImageResource,//"pack://application:,,,/Wpf/Resources/bolt_32x32.png",
                    Message = e.Message
                });

            _notifyIcon.Icon = e.TrayIcon;
            //_notifyIcon.ShowBalloonTip(e.Timeout,e.Title,e.Message,ToolTipIcon.None);
        }

        private void HandleTooltipTextChanged(object sender, TooltipTextChangedEventArgs e)
        {
            _notifyIcon.Text = e.Text;
        }

        private void OnLoginCxtMenu(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void PopulateWithPreLoginTrayItems(ContextMenu contextMenu)
        {
            contextMenu.MenuItems.Clear();
            contextMenu.MenuItems.Add("Login", OnLoginCxtMenu);
            contextMenu.MenuItems.Add("Exit", OnExit);
        }

        private void PopulateWithPostLoginTrayItems(ContextMenu contextMenu)
        {
            contextMenu.MenuItems.Clear();
            contextMenu.MenuItems.Add("Show Team View", ShowTeamView);
            contextMenu.MenuItems.Add("Start Work", OnStartWork);
            contextMenu.MenuItems.Add("End Work", OnStopWork);
            contextMenu.MenuItems.Add("Exit", OnExit);
        }

        private void ShowTeamView(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void NotifyIconClick(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void NotifyIconBalloonTipClicked(object sender, EventArgs e)
        {
            _trayPresenter.TrayIconBallonTipClicked();            
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnStartWork(object sender, EventArgs e)
        {
            _trayPresenter.StartWork();
        }

        private void OnStopWork(object sender, EventArgs e)
        {
            _trayPresenter.StopWork();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Dispose();
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _messagesReceiver.Dispose();
                _trayPresenter.TooltipTextChanged -= HandleTooltipTextChanged;
                _trayPresenter.ShowBallon -= HandleShowBallon;
                _trayPresenter.ShowPostLoginTrayInformation -= HandleLoginCompleted;
                _notifyIcon.Click -= NotifyIconClick;
                _notifyIcon.BalloonTipClicked -= NotifyIconBalloonTipClicked;
                
                _trayPresenter.Dispose();
                _notifyIcon.Dispose();                
            }

            base.Dispose(isDisposing);
        }
    }
}
