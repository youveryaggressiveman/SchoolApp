using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SchoolApp.Core
{
    public static class FrameManager
    {
        public static Frame MainFrame { get; set; }

        public static void SetSource<T>(T target) where T : Page
        {
            var contain = MainFrame.Name.GetType();

            if (contain == typeof(T))
            {
                MainFrame.NavigationService.RemoveBackEntry();

                MainFrame.Navigate(target);

                GC.Collect();
            }
        }
    }
}
