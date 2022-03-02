﻿using SchoolApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolApp.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void remeberCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (remeberCheckBox.IsChecked == true)
            {
             
            }
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AuthViewModel).Password = pswBox.Password;
        }
    }
}
