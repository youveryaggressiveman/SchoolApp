using SchoolApp.Command;
using SchoolApp.Controllers;
using SchoolApp.Core;
using SchoolApp.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolApp.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthViewModelController controller;

        private string _email;
        private string _password;

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

        public ICommand Auth { get; private set; }

        public AuthViewModel()
        {
            Auth = new DelegateCommand(Authorize);

            controller = new AuthViewModelController();
        }

        private async void Authorize(object obj)
        { 
            try
            {
                if (await controller.GetUserAsync(Email, Password))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    MessageBox.Show($"{UserSingleton.User.FirstName} добро пожаловать!", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Information);

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
        }
    }
}
