using System;
using System.Collections.Generic;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray;
using WPFGrowlNotification;

namespace TeamTimeWarp.Client.Tray
{
    public class MessagesReceiver : IDisposable
    {
        private readonly IUserStateListener _userStateListener;
        private readonly GrowlNotifiactions _growlNotifiactions;
        private readonly IUiUserMessageService _uiUserMessageService;

        public MessagesReceiver(IUserStateListener userStateListener, GrowlNotifiactions growlNotifiactions, IUiUserMessageService uiUserMessageService)
        {
            _userStateListener = userStateListener;
            _growlNotifiactions = growlNotifiactions;
            _uiUserMessageService = uiUserMessageService;
            _userStateListener.UserStateChanged += HandleStateChanged;
            _uiUserMessageService.ReceivedMessages += HandleUserMessageServiceReceivedMessages;
        }

        //todo: unit test.
        private void HandleUserMessageServiceReceivedMessages(object sender, Core.Services.AsyncCompletedEventArgs<ICollection<UserMessageReceipt>> e)
        {
            if (e.Error != null || e.Result == null) return;

            foreach (UserMessageReceipt message in e.Result)
            {
                _growlNotifiactions.AddNotification(new Notification()
                    {
                        ImageUrl = "pack://application:,,,/Wpf/Resources/mail_24x18.png",
                        Message =
                            string.Format("{0} | {1} : {2}", message.FromName, message.SendTime, message.Message),
                        Title = "Time Warp Message"
                    });
            }
        }

        private void HandleStateChanged(object sender, UserMessageEventArgs e)
        {
            if (e.UpdatedState == TimeWarpStateUserMessage.TakeABreak)
            {
                _uiUserMessageService.GetMessagesAsync();
            }
        }


        public void Dispose()
        {
            _userStateListener.UserStateChanged -= HandleStateChanged;
            _uiUserMessageService.ReceivedMessages -= HandleUserMessageServiceReceivedMessages;
        }
    }
}