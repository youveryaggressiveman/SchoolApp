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
using System.Windows.Threading;
using SchoolApp.Command;
using SchoolApp.Controllers;
using SchoolApp.Core;
using SchoolApp.Properties;
using SchoolApp.View.Pages;
using SchoolApp.View.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SchoolApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Visibility _visibility = Visibility.Collapsed;
        private Visibility _visibilitySecretary = Visibility.Collapsed;

        private User _thisUser;

        public Visibility VisibilitySecretary
        {
            get => _visibilitySecretary;
            set
            {
                _visibilitySecretary = value;
                OnPropertyChanged(nameof(VisibilitySecretary));
            }
        }

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

        public ICommand OpenFirstPageCommand { get; private set; }
        public ICommand OpenSecondPageCommand { get; private set; }
        public ICommand ExitAccount { get; private set; }

        public MainViewModel()
        {
            UserSingleton.User = new User() 
            {
                FirstName = "GG",
                SecondName = "GG",
                LastName = "GG",
                Role = new Role()
                {
                    Name = "Секретарь"
                }
            };

            OpenFirstPageCommand = new DelegateCommand(FirstRealCommand);
            OpenSecondPageCommand = new DelegateCommand(SecondRealCommand);
            ExitAccount = new DelegateCommand(Exit);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(5);

            timer.Tick += async (sender, args) =>
            {
                if (!await ServerManager.GetAccessibleServer())
                {
                    MessageBox.Fatal("Соединение отсутсвует. Приложение будет закрыто", "Ошибка");

                    Application.Current.Shutdown();
                }
            };

            timer.Start();

            LoadVisibilityMenuButton();
            LoadUserInfo();
        }

        private void FirstRealCommand(object arg)
        {
            if (UserSingleton.User.Role.Name == "Секретарь")
            {
                FrameManager.SetSource(new SecretaryPage());
            }
        }

        private void SecondRealCommand(object arg)
        {
            if (UserSingleton.User.Role.Name == "Секретарь")
            {
                FrameManager.SetSource(new SchedulePage());
            }
        }

        private void LoadVisibilityMenuButton()
        {
            VisibilitySecretary = Visibility.Collapsed;

            if (UserSingleton.User.Role.Name == "Секретарь")
            {
                VisibilitySecretary = Visibility.Visible;
            }
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

        public void GetFrame()
        {
            if (UserSingleton.User.Role.Name == "Секретарь")
            {
                FrameManager.SetSource(new SecretaryPage());

                return;
            }

            if (UserSingleton.User.Role.Name == "Служба безопасности")
            {
                FrameManager.SetSource(new SecurityPage());

                return;
            }

            if (UserSingleton.User.Role.Name == "Пользователь")
            {
                FrameManager.SetSource(new ClientPage());
            }
        }

        private void LoadUserInfo()
        {
            ThisUser = UserSingleton.User;
        }
    }
}
