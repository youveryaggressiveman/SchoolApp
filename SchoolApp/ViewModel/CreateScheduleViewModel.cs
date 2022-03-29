using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SchoolApp.Command;
using SchoolApp.Controllers;
using SchoolApp.Core.Builders;
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

        private bool _enabledDayOfWeekList = true;
        private bool _enabledGroupList = false;
        private bool _enabledOtherList = false;

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

        public delegate void UseStandardHandler();
        public delegate void UpdateHandler(object name);

        public event UseStandardHandler UseStandart;
        public event UpdateHandler Update;

        public bool EnabledDayOfWeekList
        {
            get => _enabledDayOfWeekList;
            set
            {
                _enabledDayOfWeekList = value;
                OnPropertyChanged(nameof(EnabledDayOfWeekList));
            }
        }

        public bool EnabledGroupList
        {
            get => _enabledGroupList;
            set
            {
                _enabledGroupList = value;
                OnPropertyChanged(nameof(EnabledGroupList));
            }
        }

        public bool EnabledOtherList
        {
            get => _enabledOtherList;
            set
            {
                _enabledOtherList = value;
                OnPropertyChanged(nameof(EnabledOtherList));
            }
        }

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

                if (SelectedEmployee != null)
                {
                    RouteObjectEvent(UpdateInfo, new List<object> { SelectedEmployee.FIO });
                }
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

                if (SelectedGroup != null)
                {
                    RouteObjectEvent(UpdateInfo, new List<object> { SelectedGroup.Name });
                }

                SetEnable(typeof(Group));
            }
        }

        public TimeSubject SelectedTimeSubject
        {
            get => _selectedTimeSubject;
            set
            {
                _selectedTimeSubject = value;
                OnPropertyChanged(nameof(SelectedTimeSubject));

                if (SelectedTimeSubject != null)
                {
                    RouteObjectEvent(UpdateInfo,new List<object> { SelectedTimeSubject.Time });
                }
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

                SetEnable(typeof(DayOfWeek));
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

        public ICommand RefreshList { get; private set; }
        public ICommand AddInAllSchedule { get; private set; }
        public ICommand RemoveSchedule { get; private set; }
        public ICommand AddSchedule { get; private set; }

        public CreateScheduleViewModel()
        {
            AllScheduleList = new ObservableCollection<ObservableCollection<Schedule>>();

            RefreshList = new DelegateCommand(Refresh);
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

            RouteEvent(new List<UseStandardHandler> { Initialize, LoadAllInfo });
        }

        private void RouteEvent(List<UseStandardHandler> action)
        {
            foreach (var item in action)
            {
                UseStandart += item;
                UseStandart();
                UseStandart -= item;
            }
        }

        public void RouteObjectEvent(UpdateHandler handler, List<object> name)
        {
            Update += handler;

            foreach (var item in name)
            {
                Update(item);
            }

            Update -= handler;
        }

        private void Refresh(object arg)
        {
            SetEnable(null);

            ScheduleList = new ObservableCollection<Schedule>();
        }

        private void SetEnable(Type type)
        {
            if (type == null)
            {
                EnabledDayOfWeekList = true;
                EnabledGroupList = false;
                EnabledOtherList = false;
            }

            if (type.Equals(typeof(DayOfWeek)))
            {
                EnabledDayOfWeekList = false;
                EnabledGroupList = true;
                EnabledOtherList = false;
            }

            if (type.Equals(typeof(Group)))
            {
                EnabledDayOfWeekList = false;
                EnabledGroupList = false;
                EnabledOtherList = true;
            }
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
            foreach (var item in ScheduleList.ToList())
            {
                if (item.Equals(SelectedSchedule))
                {
                    ScheduleList.Remove(item);

                    RouteEvent(new List<UseStandardHandler> { Initialize, LoadAllInfo });
                    RouteObjectEvent(UpdateInfo, new List<object>{ item.Employee.FIO, item.Group.Name, item.TimeSubject.Time});
                }
            }
        }

        private void AddInListNewSchedule(object obj)
        {
            ScheduleBuilder scheduleBuilder = new ScheduleBuilder();

            var newSchedule = scheduleBuilder.WithID(0)
                .WithDayOfWeekID(SelectedDayOfWeek.ID)
                .WithEmployeeID(SelectedEmployee.ID)
                .WithGroupID(SelectedGroup.ID)
                .WithSubjectID(SelectedSubject.ID)
                .WithTimeSubjectID(SelectedTimeSubject.ID)
                .WithDayOfWeek(SelectedDayOfWeek)
                .WithEmployee(SelectedEmployee)
                .WithGroup(SelectedGroup)
                .WithSubject(SelectedSubject)
                .WithTimeSubject(SelectedTimeSubject)
                .Build();

            ScheduleList.Add(newSchedule);
        }

        private void Initialize()
        {
            _deleteEmployeeList = new List<Employee>();
            _deleteGroupList = new List<Group>();
            _deleteTimeSubjectList = new List<TimeSubject>();
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

        private void UpdateInfo(object name)
        {
            if (ScheduleList.Count == 0)
            {
                return;
            }

            foreach (var item in _deleteEmployeeList.ToList())
            {
                ModifyCollection(EmployeeList, item, _deleteEmployeeList);
            }

            foreach (var item in _deleteGroupList.ToList())
            {
                if (!GroupList.Contains(item))
                {
                    ModifyCollection(GroupList, item, _deleteGroupList);
                }
            }

            foreach (var item in _deleteTimeSubjectList.ToList())
            {
                ModifyCollection(TimeSubjectList, item, _deleteTimeSubjectList);
            }

            foreach (var item in ScheduleList.ToList())
            {
                if (item.Employee.FIO.Equals(name))
                {
                    foreach (var group in GroupList.ToList())
                    {
                        if (group.ID == item.Group.ID)
                        {
                            ModifyCollection(_deleteGroupList, group, GroupList);
                        }
                    }

                    foreach (var timeSubject in TimeSubjectList.ToList())
                    {
                        if (timeSubject.ID == item.TimeSubject.ID)
                        {
                            ModifyCollection(_deleteTimeSubjectList, timeSubject, TimeSubjectList);
                        }
                    }
                }

                if (item.Group.Name.Equals(name))
                {
                    foreach (var timeSubject in TimeSubjectList.ToList())
                    {
                        if (timeSubject.ID == item.TimeSubject.ID)
                        {
                            ModifyCollection(_deleteTimeSubjectList, timeSubject, TimeSubjectList);
                        }
                    }

                    foreach (var employee in EmployeeList.ToList())
                    {
                        if (employee.ID == item.Employee.ID)
                        {
                            ModifyCollection(_deleteEmployeeList, employee, EmployeeList);
                        }
                    }
                }

                if (item.TimeSubject.Time.Equals(name))
                {
                    foreach (var employee in EmployeeList.ToList())
                    {
                        if (employee.ID == item.Employee.ID)
                        {
                            ModifyCollection(_deleteEmployeeList, employee, EmployeeList);
                        }
                    }

                    foreach (var group in GroupList.ToList())
                    {
                        if (group.ID == item.Group.ID)
                        {
                            ModifyCollection(_deleteGroupList, group, GroupList);
                        }
                    }
                }
            }
        }

        public void ModifyCollection<Unit, Target, Deleted>(Target target, Unit unit, Deleted deleted)
            where Target : ICollection<Unit>
            where Deleted : ICollection<Unit>
        {
            target.Add(unit);
            deleted.Remove(unit);
        }

        private async void LoadAllInfo()
        {
            GroupList = new ObservableCollection<Group>();
            EmployeeList = new ObservableCollection<Employee>();
            DayOfWeekList = new ObservableCollection<DayOfWeek>();
            TimeSubjectList = new ObservableCollection<TimeSubject>();
            SubjectList = new ObservableCollection<Subject>();

            IEnumerable<Group> groupList;
            IEnumerable<Employee> employeeList;
            IEnumerable<DayOfWeek> dayOfWeekList;
            IEnumerable<TimeSubject> timeSubjectList;
            IEnumerable<Subject> subjectList;

            SetSplash(true);

            try
            {
                groupList = await _getGroupListController.GetList(new[] { "Group", "GetAll" });
                employeeList = await _getEmployeeListController.GetList(new[] { "Employee", "GetAll" });
                dayOfWeekList = await _getDayOfWeekController.GetList(new[] { "DayOfWeek", "GetAll" });
                subjectList = await _getSubjectListController.GetList(new[] { "Subject", "GetAll" });
                timeSubjectList = await _getTimeSubjectListController.GetList(new[] { "TimeSubject", "GetAll" });

                groupList.ToList().ForEach(GroupList.Add);
                employeeList.ToList().ForEach(EmployeeList.Add);
                dayOfWeekList.ToList().ForEach(DayOfWeekList.Add);
                subjectList.ToList().ForEach(SubjectList.Add);
                timeSubjectList.ToList().ForEach(TimeSubjectList.Add);
            }
            catch (Exception e)
            {
                MessageBox.Error("Произошла ошибка подключения. " + e.Message, "Получение данных");
            }
            finally
            {
                SetSplash(false);
            }
        }
    }
}
