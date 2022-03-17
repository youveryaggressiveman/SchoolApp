using SchoolApp.Command;
using SchoolApp.Controllers;
using SchoolApp.Core;
using SchoolApp.Properties;
using SchoolApp.View.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SchoolApp.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthViewModelController _controller;

        private string _email = "andrey.banplay2017@yandex.ru";
        private string _password = "123";

        private bool? _check;

        private Visibility _visibility = Visibility.Collapsed;

        public bool? Check
        {
            get => _check;
            set
            {
                _check = value;
                OnPropertyChanged(nameof(Check));
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

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand Close { get; private set; }
        public ICommand Auth { get; private set; }

        public AuthViewModel()
        {
            Close = new DelegateCommand(CloseWindow);
            Auth = new DelegateCommand(Authorize);

            _controller = new AuthViewModelController();

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

            CheckUser();
        }

        public async void CheckUser()
        {
            if (!(string.IsNullOrEmpty(Settings.Default.Email) && string.IsNullOrEmpty(Settings.Default.Password)))
            {
                try
                {
                    SetSplash(true);

                    if (await _controller.GetUserAsync(Settings.Default.Email, Settings.Default.Password))
                    {
                        OpenMainWindow();

                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Error("Произошла ошибка подключения", "Проверка подключения");
                    Properties.Settings.Default.Reset();
                    return;
                }
                finally
                {
                    SetSplash(false);
                }
            }
        }

        private static void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            foreach (Window item in Application.Current.Windows)
            {
                if (item is AuthWindow)
                {
                    item.Close();
                }
            }
        }

        private void SetSplash(bool visibl)
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

        private void RememberUser()
        {
            if (Check == true)
            {
                Settings.Default.Email = Email;
                Settings.Default.Password = Password;
                Settings.Default.Save();
            }

            if (Check == false)
            {
                Settings.Default.Email = null;
                Settings.Default.Password = null;
            }
        }

        private void CloseWindow(object arg)
        {
            Application.Current.Shutdown();
        }

        private async void Authorize(object obj)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Info("Поля не могут быть пустыми", "Информация");

                return;
            }

            if (!Regex.IsMatch(Email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
            {
                MessageBox.Info("Неверный формат e-mail", "Информация");

                return;
            }

            try
            {
                SetSplash(true);

                if (await _controller.GetUserAsync(Email, Password))
                {
                    RememberUser();

                    OpenMainWindow();

                    return;
                }

                MessageBox.Info("Такого пользователя не существует", "Проверка данных");
            }
            catch (Exception)
            {
                MessageBox.Error("Произошла ошибка подключения", "Проверка подключения");
                return;
            }
            finally
            {
                SetSplash(false);
            }
        }
    }
}
