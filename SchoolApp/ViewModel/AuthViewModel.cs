using SchoolApp.Command;
using SchoolApp.Controllers;
using SchoolApp.Core;
using SchoolApp.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolApp.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthViewModelController _controller;

        private string _email;
        private string _password;

        private Visibility _visibility = Visibility.Collapsed; 

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

        private void CloseWindow(object arg)
        {
            Application.Current.Shutdown();
        }

        private async void Authorize(object obj)
        {
            if (!Regex.IsMatch(Email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
            {
                MessageBox.Show("Неверный формат e-mail", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Поля не могут быть пустыми", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            try
            {
                SetSplash(true);

                if (await _controller.GetUserAsync(Email, Password))
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

                    return;
                }

                MessageBox.Show("Такого пользователя не существует", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка подключения", "Проверка подключения", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            finally
            {
                SetSplash(false);
            }
        }
    }
}
