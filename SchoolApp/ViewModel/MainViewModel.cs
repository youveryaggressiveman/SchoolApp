using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SchoolApp.Core;
using SchoolApp.View.Pages;

namespace SchoolApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private User _thisUser;

        public User ThisUser
        {
            get => _thisUser;
            set
            {
                _thisUser = value;
                OnPropertyChanged(nameof(ThisUser));
            }
        }

        public MainViewModel(Frame mainFrame)
        {
            LoadUserInfo();
            GetFrame(mainFrame);
        }

        public void GetFrame(Frame mainFrame)
        {
            if (UserSingleton.User.Role.Name == "Секретарь")
            {
                FrameManager.MainFrame = mainFrame;

                FrameManager.SetSource(new SecretaryPage());

                return;
            }

            if (UserSingleton.User.Role.Name == "Служба безопасности")
            {
                FrameManager.MainFrame = mainFrame;

                FrameManager.SetSource(new SecurityPage());

                return;
            }

            if (UserSingleton.User.Role.Name == "Пользователь")
            {
                FrameManager.MainFrame = mainFrame;

                FrameManager.SetSource(new ClientPage());
            }
        }

        private void LoadUserInfo()
        {
            ThisUser = UserSingleton.User;
        }
    }
}
