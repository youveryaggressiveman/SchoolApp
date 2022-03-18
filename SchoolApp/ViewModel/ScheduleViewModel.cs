using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SchoolApp.Command;
using SchoolApp.Model;
using SchoolApp.View.Pages;
using SchoolApp.View.Windows;

namespace SchoolApp.ViewModel
{
    public class ScheduleViewModel : BaseViewModel
    {
        private ObservableCollection<ObservableCollection<Schedule>> _allScheduleDataGridList;

        public ObservableCollection<ObservableCollection<Schedule>> AllScheduleDateGridList
        {
            get => _allScheduleDataGridList;
            set
            {
                _allScheduleDataGridList = value;
                OnPropertyChanged(nameof(AllScheduleDateGridList));
            }
        }

        public ICommand CreateNewSchedule { get; private set; }
        public ICommand CreateAutoNewSchedule { get; private set; }
        public ICommand Print { get; private set; }

        public ScheduleViewModel()
        {
            Print = new DelegateCommand(PrintSchedule);
            CreateAutoNewSchedule = new DelegateCommand(CreateAutoSchedule);
            CreateNewSchedule = new DelegateCommand(OpenWindowByCreateNewSchedule);

            AllScheduleDateGridList = new ObservableCollection<ObservableCollection<Schedule>>();
        }

        private void SetSplash(bool isEnabled)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item is MainWindow)
                {
                    (item.DataContext as MainViewModel).SetSplash(isEnabled);
                }
            }
        }

        private void PrintSchedule(object obj)
        {
            throw new NotImplementedException();
        }

        private void CreateAutoSchedule(object obj)
        {
            throw new NotImplementedException();
        }

        private void OpenWindowByCreateNewSchedule(object obj)
        {
            SetSplash(true);

            CreateScheduleWindow createScheduleWindow = new CreateScheduleWindow();
            createScheduleWindow.ShowDialog();

            SetSplash(false);
        }
    }
}
