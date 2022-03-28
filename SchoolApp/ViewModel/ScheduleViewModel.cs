using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SchoolApp.Command;
using SchoolApp.Controllers;
using SchoolApp.Model;
using SchoolApp.View.Pages;
using SchoolApp.View.Windows;
using DayOfWeek = SchoolApp.Model.DayOfWeek;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SchoolApp.ViewModel
{
    public class ScheduleViewModel : BaseViewModel
    {
        private readonly UniversalController<Schedule> _getScheduleBySelectedDay;
        private readonly UniversalController<DayOfWeek> _getDayOfWeekList;

        private ObservableCollection<Schedule> _allScheduleDataGridList;
        private ObservableCollection<DayOfWeek> _dayOfWeekList;

        private DayOfWeek _selectedDayOfWeek;

        public delegate void LoadInfo();
        public event LoadInfo Load;

        public DayOfWeek SelectedDayOfWeek
        {
            get => _selectedDayOfWeek;
            set
            {
                _selectedDayOfWeek = value;
                OnPropertyChanged(nameof(SelectedDayOfWeek));

                LoadingControl(new List<LoadInfo> { LoadScheduleBySelectedDay });
            }
        }

        public ObservableCollection<DayOfWeek> DayOfWeekList
        {
            get => _dayOfWeekList;
            set
            {
                _dayOfWeekList = value;
                OnPropertyChanged(nameof(DayOfWeekList));
            }
        }

        public ObservableCollection<Schedule> AllScheduleDateGridList
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
            _getScheduleBySelectedDay = new UniversalController<Schedule>();
            _getDayOfWeekList = new UniversalController<DayOfWeek>();

            Print = new DelegateCommand(PrintSchedule);
            CreateAutoNewSchedule = new DelegateCommand(CreateAutoSchedule);
            CreateNewSchedule = new DelegateCommand(OpenWindowByCreateNewSchedule);

            DayOfWeekList = new ObservableCollection<DayOfWeek>();
            AllScheduleDateGridList = new ObservableCollection<Schedule>();

            LoadingControl(new List<LoadInfo> { LoadDayOfWeek });
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

        private void LoadingControl(List<LoadInfo> action)
        {
            foreach (var item in action)
            {
                Load += item;
                Load();
                Load -= item;
            }
        }

        private async void LoadScheduleBySelectedDay()
        {
            IEnumerable<Schedule> scheduleList;

            SetSplash(true);

            try
            {
                scheduleList = await _getScheduleBySelectedDay.GetListBySomething(new string[]{ "Schedule", "GetScheduleByDayID", "scheduleID"}, new string[] {SelectedDayOfWeek.ID.ToString() });
            }
            catch (Exception e)
            {

                MessageBox.Error($"Произошла ошибка. {e.Message}");
            }
            finally
            {
                SetSplash(false);
            }
        }

        private async void LoadDayOfWeek()
        {
            IEnumerable<DayOfWeek> dayOfWeekList;

            SetSplash(true);
            try
            {
                dayOfWeekList = await _getDayOfWeekList.GetList(new string[] { "DayOfWeek", "GetAll" });

                dayOfWeekList.ToList().ForEach(DayOfWeekList.Add);
            }
            catch (Exception e)
            {
                MessageBox.Error($"Произошла ошибка подключения. {e.Message}", "Ошибка");
            }
            finally
            {
                SetSplash(false);
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
