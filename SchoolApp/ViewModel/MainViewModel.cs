using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SchoolApp.Command;
using SchoolApp.Core;
using SchoolApp.Properties;
using SchoolApp.View.Pages;
using SchoolApp.View.Windows;

namespace SchoolApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Visibility _visibility = Visibility.Collapsed;

        private User _thisUser;

        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        public User ThisUser
        {
            get => _thisUser;
            set
            {
                _thisUser = value;
                OnPropertyChanged(nameof(ThisUser));
            }
        }

        public ICommand ExitAccount { get; private set; }

        public MainViewModel(Frame mainFrame)
        {
            ExitAccount = new DelegateCommand(Exit);

            LoadUserInfo();
            //GetFrame(mainFrame);
        }

        public void SetSplash(bool visibl)
        {
            if (visibl)
            {
                Visibility = Visibility.Visible;
            }

            if (!visibl)
            {
                Visibility = Visibility.Collapsed;
            }
        }

        private void Exit(object obj)
        {
            UserSingleton.User = null;

            Settings.Default.Password = null;
            Settings.Default.Email = null;

            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();

            foreach (Window item in Application.Current.Windows)
            {
                if (item is MainWindow)
                {
                    item.Close();
                }
            }
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
