using System;
using System.Windows.Forms;

namespace TimeManager.Client.Tray
{
    public class TimeMangerProxy
    {
        private static string GetUserName()
        {
            var nameSegments = System.Environment.UserName.Split('.');

            if(nameSegments.Length > 1)
            {
                return string.Concat(nameSegments[0],'.',nameSegments[1][0]);
            }
            else
            {
                return nameSegments[0];   
            }
        }

        public void StartWork()
        {
            try
            {
                using (var timeManagerServiceClient = new TimeManagerServiceClient())
                {
                    timeManagerServiceClient.StartWork(GetUserName());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Team Pomodoro");
            }
        }

        public PomodoroUserDto GetUserInfo()
        {
            try
            {
                using (var timeManagerServiceClient = new TimeManagerServiceClient())
                {
                    return timeManagerServiceClient.GetUserInfo(GetUserName());
                }
            }
            catch (Exception e)
            {
                //    MessageBox.Show("Team Pomodoro", e.Message);
            }
            return new PomodoroUserDto();
        }

        public void StopWork()
        {
            try
            {
                using (var timeManagerServiceClient = new TimeManagerServiceClient())
                {
                    timeManagerServiceClient.StopWork(GetUserName());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Team Pomodoro");
            }
        }

    }
}