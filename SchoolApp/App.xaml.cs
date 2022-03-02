using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigHelper.Instance.SetLang("Ru-ru");
        }

        private void UpdateSkin(SkinType skinType)
        {

        }
    }
}
