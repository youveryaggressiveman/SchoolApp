using SchoolApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для CreateScheduleWindow.xaml
    /// </summary>
    public partial class CreateScheduleWindow : Window
    {
        public CreateScheduleWindow()
        {
            InitializeComponent();

            foreach (Window item in Application.Current.Windows)
            {
                if (item is MainWindow)
                {
                    this.Owner = item;
                }
            }
        }

        private void addScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CreateScheduleViewModel).AddSchedule.Execute(sender);

            timeSubjectComboBox.SelectedIndex = -1;
            subjectComboBox.SelectedIndex = -1;
            employeeComboBox.SelectedIndex = -1;
        }
    }
}
