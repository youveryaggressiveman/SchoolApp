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
using DayOfWeek = SchoolApp.Model.DayOfWeek;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SchoolApp.ViewModel
{
    public class CreateScheduleViewModel : BaseViewModel
    {
        private readonly UniversalController<Group> _getGroupListController;
        private readonly UniversalController<Employee> _getEmployeeListController;
        private readonly UniversalController<Subject> _getSubjectListController;
        private readonly UniversalController<TimeSubject> _getTimeSubjectListController;
        private readonly UniversalController<DayOfWeek> _getDayOfWeekController;

        private ObservableCollection<ObservableCollection<Schedule>> _allScheduleList;

        private List<Employee> _deleteEmployeeList;
        private List<Group> _deleteGroupList;
        private List<TimeSubject> _deleteTimeSubjectList;

        private ObservableCollection<Employee> _employeeList;
        private ObservableCollection<Subject> _subjectList;
        private ObservableCollection<TimeSubject> _timeSubjectList;
        private ObservableCollection<Group> _groupList;
        private ObservableCollection<DayOfWeek> _dayOfWeekList;
        private ObservableCollection<Schedule> _scheduleList;

        private Employee _selectedEmployee;
        private Subject _selectedSubject;
        private TimeSubject _selectedTimeSubject;
        private Group _selectedGroup;
        private DayOfWeek _selectedDayOfWeek;
        private Schedule _selectedSchedule;

        public ObservableCollection<ObservableCollection<Schedule>> AllScheduleList
        {
            get => _allScheduleList;
            set
            {
                _allScheduleList = value;
                OnPropertyChanged(nameof(AllScheduleList));
            }
        }

        public ObservableCollection<TimeSubject> TimeSubjectList
        {
            get => _timeSubjectList;
            set
            {
                _timeSubjectList = value;
                OnPropertyChanged(nameof(TimeSubjectList));
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));

                UpdateInfo(SelectedEmployee.FIO);
            }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
            }
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));

                UpdateInfo(SelectedGroup.Name);
            }
        }

        public TimeSubject SelectedTimeSubject
        {
            get => _selectedTimeSubject;
            set
            {
                _selectedTimeSubject = value;
                OnPropertyChanged(nameof(SelectedTimeSubject));

                UpdateInfo(SelectedTimeSubject.Time);
            }
        }

        public Schedule SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                _selectedSchedule = value;
                OnPropertyChanged(nameof(SelectedSchedule));
            }
        }

        public DayOfWeek SelectedDayOfWeek
        {
            get => _selectedDayOfWeek;
            set
            {
                _selectedDayOfWeek = value;
                OnPropertyChanged(nameof(SelectedDayOfWeek));
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

        public ObservableCollection<Group> GroupList
        {
            get => _groupList;
            set
            {
                _groupList = value;
                OnPropertyChanged(nameof(GroupList));
            }
        }

        public ObservableCollection<Employee> EmployeeList
        {
            get => _employeeList;
            set
            {
                _employeeList = value;
                OnPropertyChanged(nameof(EmployeeList));
            }
        }

        public ObservableCollection<Subject> SubjectList
        {
            get => _subjectList;
            set
            {
                _subjectList = value;
                OnPropertyChanged(nameof(SubjectList));
            }
        }

        public ObservableCollection<Schedule> ScheduleList
        {
            get => _scheduleList;
            set
            {
                _scheduleList = value;
                OnPropertyChanged(nameof(ScheduleList));
            }
        }

        public ICommand AddInAllSchedule { get; private set; }
        public ICommand RemoveSchedule { get; private set; }
        public ICommand AddSchedule { get; private set; }

        public CreateScheduleViewModel()
        {
            AllScheduleList = new ObservableCollection<ObservableCollection<Schedule>>();

            AddInAllSchedule = new DelegateCommand(AddAllScheduleForGroup);
            RemoveSchedule = new DelegateCommand(RemoveInListNewSchedule);
            AddSchedule = new DelegateCommand(AddInListNewSchedule);

            _getDayOfWeekController = new UniversalController<DayOfWeek>();
            _getEmployeeListController = new UniversalController<Employee>();
            _getGroupListController = new UniversalController<Group>();
            _getSubjectListController = new UniversalController<Subject>();
            _getTimeSubjectListController = new UniversalController<TimeSubject>();

            ScheduleList = new ObservableCollection<Schedule>();
            GroupList = new ObservableCollection<Group>();
            EmployeeList = new ObservableCollection<Employee>();
            DayOfWeekList = new ObservableCollection<DayOfWeek>();
            TimeSubjectList = new ObservableCollection<TimeSubject>();
            SubjectList = new ObservableCollection<Subject>();

            LoadAllInfo();
        }

        private void AddAllScheduleForGroup(object obj)
        {
            if (ScheduleList.Count != 0)
            {
                MessageBox.Info("Список на расписание не может быть пустым", "Информация");
                return;
            }

            foreach (var item in ScheduleList)
            {
                item.DayOfWeekID = SelectedDayOfWeek.ID;
            }

            AllScheduleList.Add(ScheduleList);
        }

        private void RemoveInListNewSchedule(object obj)
        {
            foreach (var item in ScheduleList)
            {
                if (item.Equals(SelectedSchedule))
                {
                    ScheduleList.Remove(item);
                }
            }
        }

        private void AddInListNewSchedule(object obj)
        {
            Schedule newSchedule = new Schedule()
            {
                EmployeeID = SelectedEmployee.ID,
                GroupID = SelectedGroup.ID,
                SubjectID = SelectedSubject.ID,
                TimeSubjectID = SelectedTimeSubject.ID
            };

            ScheduleList.Add(newSchedule);
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

        private void UpdateInfo(string name)
        {
            if (ScheduleList.Count == 0)
            {
                return;;
            }

            foreach (var item in _deleteEmployeeList)
            {
                EmployeeList.Add(item);

                _deleteEmployeeList.Remove(item);
            }

            foreach (var item in _deleteGroupList)
            {
                GroupList.Add(item);

                _deleteGroupList.Remove(item);
            }

            foreach (var item in _deleteTimeSubjectList)
            {
                TimeSubjectList.Add(item);

                _deleteTimeSubjectList.Remove(item);
            }

            foreach (var item in ScheduleList)
            {
                if (item.Employee.FIO == name)
                {
                    foreach (var group in GroupList)
                    {
                        if (group == item.Group)
                        {
                            _deleteGroupList.Add(group);

                            GroupList.Remove(group);
                        }
                    }

                    foreach (var timeSubject in TimeSubjectList)
                    {
                        if (timeSubject == item.TimeSubject)
                        {
                            _deleteTimeSubjectList.Add(timeSubject);

                            TimeSubjectList.Remove(timeSubject);
                        }
                    }
                }

                if (item.Group.Name == name)
                {
                    foreach (var timeSubject in TimeSubjectList)
                    {
                        if (timeSubject == item.TimeSubject)
                        {
                            _deleteTimeSubjectList.Add(timeSubject);

                            TimeSubjectList.Remove(timeSubject);
                        }
                    }

                    foreach (var employee in EmployeeList)
                    {
                        if (employee == item.Employee)
                        {
                            _deleteEmployeeList.Add(employee);

                            EmployeeList.Remove(employee);
                        }
                    }
                }

                if (item.TimeSubject.Time == name)
                {
                    foreach (var employee in EmployeeList)
                    {
                        if (employee == item.Employee)
                        {
                            _deleteEmployeeList.Add(employee);

                            EmployeeList.Remove(employee);
                        }
                    }

                    foreach (var group in GroupList)
                    {
                        if (group == item.Group)
                        {
                            _deleteGroupList.Add(group);

                            GroupList.Remove(group);
                        }
                    }
                }
            }
        }

        private async void LoadAllInfo()
        {
            IEnumerable<Group> groupList;
            IEnumerable<Employee> employeeList;
            IEnumerable<DayOfWeek> dayOfWeekList;
            IEnumerable<TimeSubject> timeSubjectList;
            IEnumerable<Subject> subjectList;

            SetSplash(true);

            try
            {
                groupList = await _getGroupListController.GetList(new[] {"Group", "GetAll"});
                employeeList = await _getEmployeeListController.GetList(new[] {"Employee", "GetAll"});
                dayOfWeekList = await _getDayOfWeekController.GetList(new[] {"DayOfWeek", "GetAll"});
                subjectList = await _getSubjectListController.GetList(new[] {"Subject", "GetAll"});
                timeSubjectList = await _getTimeSubjectListController.GetList(new[] {"TimeSubject", "GetAll"});

                groupList.ToList().ForEach(GroupList.Add);
                employeeList.ToList().ForEach(EmployeeList.Add);
                dayOfWeekList.ToList().ForEach(DayOfWeekList.Add);
                subjectList.ToList().ForEach(SubjectList.Add);
                timeSubjectList.ToList().ForEach(TimeSubjectList.Add);
            }
            catch (Exception e)
            {
                MessageBox.Error("Произошла ошибка подключения", "Получение данных");
            }
            finally
            {
                SetSplash(false);
            }
        }
    }
}
