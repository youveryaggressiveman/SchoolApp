using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SchoolApp.Command;
using SchoolApp.Controllers;
using SchoolApp.Core.Builders;
using SchoolApp.Model;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SchoolApp.ViewModel
{
    public class SecretaryViewModel : BaseViewModel
    {
        private readonly UniversalController<Group> _putGroupController;
        private readonly UniversalController<object> _postNewStudentGroupController;
        private readonly UniversalController<City> _getCityListByCountryController;
        private readonly UniversalController<Employee> _getCuratorListController;
        private readonly UniversalController<Country> _getCountryListController;
        private readonly UniversalController<Group> _getGroupListController;

        private ObservableCollection<Group> _groupList;
        private ObservableCollection<Employee> _curatorList;
        private ObservableCollection<Student> _studentList;
        private ObservableCollection<Country> _countryList;
        private ObservableCollection<City> _cityListBySelectedCountry;

        private Country _selectedCountry;
        private City _selectedCity;
        private Address _addressByNewStudent;
        private Passport _passportByNewStudent;
        private Group _selectedGroup;
        private Employee _selectedCurator;
        private Student _selectedStudent;
        private Student _newStudent;

        public delegate void StandartLoad();
        public event StandartLoad Load;

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));

                WorkEvent(LoadInfoAboutSelectedStudent);
            }
        }

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));

                LoadCityByCountry();
            }
        }

        public City SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        public Address AddressByNewStudent
        {
            get => _addressByNewStudent;
            set
            {
                _addressByNewStudent = value;
                OnPropertyChanged(nameof(AddressByNewStudent));
            }
        }

        public ObservableCollection<City> CityListBySelectedCountry
        {
            get => _cityListBySelectedCountry;
            set
            {
                _cityListBySelectedCountry = value;
                OnPropertyChanged(nameof(CityListBySelectedCountry));
            }
        }

        public ObservableCollection<Country> CountryList
        {
            get => _countryList;
            set
            {
                _countryList = value;
                OnPropertyChanged(nameof(CountryList));
            }
        }

        public Passport PassportByNewStudent
        {
            get => _passportByNewStudent;
            set
            {
                _passportByNewStudent = value;
                OnPropertyChanged(nameof(PassportByNewStudent));
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

        public ObservableCollection<Employee> CuratorList
        {
            get => _curatorList;
            set
            {
                _curatorList = value;
                OnPropertyChanged(nameof(CuratorList));
            }
        }

        public ObservableCollection<Student> StudentList
        {
            get => _studentList;
            set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentList));
            }
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        public Employee SelectedCurator
        {
            get => _selectedCurator;
            set
            {
                _selectedCurator = value;
                OnPropertyChanged(nameof(SelectedCurator));
            }
        }

        public Student NewStudent
        {
            get => _newStudent;
            set
            {
                _newStudent = value;
                OnPropertyChanged(nameof(NewStudent));
            }
        }

        public ICommand Default { get; private set; }
        public ICommand RefreshCode { get; set; }
        public ICommand AddNewGroupInDatabase { get; private set; }
        public ICommand AddNewUserInGroup { get; private set; }
        public ICommand DeleteNewStudent { get; private set; }
        public ICommand RefactorNewStudent { get; private set; }

        public SecretaryViewModel()
        {
            _putGroupController = new UniversalController<Group>();
            _postNewStudentGroupController = new UniversalController<object>();
            _getCityListByCountryController = new UniversalController<City>();
            _getCuratorListController = new UniversalController<Employee>();
            _getCountryListController = new UniversalController<Country>();
            _getGroupListController = new UniversalController<Group>();

            GroupList = new ObservableCollection<Group>();
            CountryList = new ObservableCollection<Country>();
            CuratorList = new ObservableCollection<Employee>();
            StudentList = new ObservableCollection<Student>();
            CityListBySelectedCountry = new ObservableCollection<City>();

            RefreshCode = new DelegateCommand(NewCode);
            NewStudent = new Student();
            AddressByNewStudent = new Address();
            PassportByNewStudent = new Passport();

            Default = new DelegateCommand((object obj) => WorkEvent(SetDefault));

            DeleteNewStudent = new DelegateCommand(Delete);
            AddNewGroupInDatabase = new DelegateCommand(AddNewGroup);
            AddNewUserInGroup = new DelegateCommand(AddNewUser);

            WorkEvent(LoadAllInfo);
        }

        private void WorkEvent(StandartLoad action)
        {
            Load += action;
            Load();
            Load -= action;
        }

        private void NewCode(object obj)
        {
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                NewStudent.User.Code += random.Next(0, 10);
            }
        }

        private void LoadInfoAboutSelectedStudent()
        {
            if (SelectedStudent == null)
            {
                return;
            }

            NewStudent = SelectedStudent;
            PassportByNewStudent = SelectedStudent.User.Passport;
            AddressByNewStudent = SelectedStudent.User.Address;

            foreach (var item in CountryList)
            {
                if (item.ID == SelectedStudent.User.Address.City
                                .ToList()[0].Countries
                                .ToList()[0].ID)
                {
                    SelectedCountry = item;
                }
            }

            OnPropertyChanged(nameof(SelectedCountry));

            //await LoadCityByCountry();

            foreach (var item in CityListBySelectedCountry)
            {
                if (item.ID == SelectedStudent.User.Address.City
                                .ToList()[0].ID)
                {
                    SelectedCity = item;

                }
            }
        }

        private void Delete(object arg)
        {
            if (SelectedStudent == null)
            {
                return;
            }

            foreach (var item in StudentList)
            {
                if (item == SelectedStudent)
                {
                    StudentList.Remove(item);
                    return;
                }
            }
        }

        private async void AddNewGroup(object obj)
        {
            if (StudentList.Count == 0)
            {
                MessageBox.Info("Список группы не может быть пустым", "Информация");

                return;
            }

            if(SelectedCurator == null || SelectedGroup == null)
            {
                MessageBox.Info("Выберите группу и куратора для занесения в бд", "Информация");

                return;
            }

            SetSplash(true);

            var newGroup = new Group()
            {
                Name = SelectedGroup.Name,
                CuratorID = SelectedCurator.ID,
            };

            try
            {
                await _putGroupController.PutAsync(newGroup, new string[] { "Group", "Put", "group" }, new string[] { });
            }
            catch (Exception)
            {
                SetSplash(false);

                MessageBox.Error("Произошла ошибка подключения", "Получение данных");

                throw;
            }

            try
            {
                foreach (var item in StudentList)
                {
                    item.GroupID = SelectedGroup.ID;

                    try
                    {
                        await _postNewStudentGroupController.PostListSomething(item, new string[] { "Student", "Create" }, new string[] { });
                    }
                    catch (Exception e)
                    {
                        MessageBox.Info($"Данные o {item.User.FIO} не смогли занестись. Попробуйте позже. {e.Message}", "Занесение данных");
                        return;
                    }
                    finally
                    {
                        SetSplash(false);
                    }              
                }
               
            }
            catch (Exception e)
            {
                MessageBox.Error($"Произошла ошибка подключения. {e.Message}", "Получение данных");

                return;
            }
            finally
            {
                SetSplash(false);
            }
        }

        private void AddNewUser(object obj)
        {
            if (string.IsNullOrEmpty(NewStudent.User.FirstName) || string.IsNullOrEmpty(NewStudent.User.SecondName) ||
                string.IsNullOrEmpty(NewStudent.User.LastName) || string.IsNullOrEmpty(NewStudent.User.Password) ||
                string.IsNullOrEmpty(NewStudent.User.Email) || string.IsNullOrEmpty(PassportByNewStudent.PassportNumber) ||
                string.IsNullOrEmpty(PassportByNewStudent.PassportSerial) || PassportByNewStudent.DateBirth == DateTime.Now ||
                SelectedCountry == null || SelectedCity == null ||
                string.IsNullOrEmpty(AddressByNewStudent.AddressName) ||
                string.IsNullOrEmpty(AddressByNewStudent.AddressNumber))
            {
                MessageBox.Info("Заполните все поля", "Информация");

                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(NewStudent.User.Email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
            {
                MessageBox.Info("Неверный формат e-mail", "Информация");

                return;
            }

            StudentBuilder studentBuilder = new StudentBuilder();

            var newStudent = studentBuilder
                .UserBuilder
                    .WithFullName(new string[] { NewStudent.User.FirstName, NewStudent.User.SecondName, NewStudent.User.LastName })
                    .WithEmail(NewStudent.User.Email)
                    .WithCode(NewStudent.User.Code)
                    .WithPassword(NewStudent.User.Password)
                .PassportBuilder
                    .WithPassportSerial(PassportByNewStudent.PassportSerial)
                    .WithPassportNumber(PassportByNewStudent.PassportNumber)
                    .WithDateBirth(PassportByNewStudent.DateBirth)
                .AddressBuilder
                    .WithAddressName(AddressByNewStudent.AddressName)
                    .WithAddressNumber(AddressByNewStudent.AddressNumber)
                .CityBuilder
                    .WithCity(SelectedCity)
                .CountryBuilder
                    .WithCountry(SelectedCountry)
                .Build();

            SetDefault();

            StudentList.Add(newStudent);
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

        private void SetDefault()
        {
            NewStudent = new Student();
            AddressByNewStudent = new Address();
            PassportByNewStudent = new Passport();

           WorkEvent(LoadAllInfo);
        }

        private async void LoadAllInfo()
        {
            SetSplash(true);

            CountryList = new ObservableCollection<Country>();
            CuratorList = new ObservableCollection<Employee>();
            GroupList = new ObservableCollection<Group>();

            IEnumerable<Group> listGroup;
            IEnumerable<Country> listCountry;
            IEnumerable<Employee> listCurator;

            try
            {
                listCurator = await _getCuratorListController.GetList(new string[] { "Employee", "GetAll" });
                listCountry = await _getCountryListController.GetList(new string[] { "Country", "GetAll" });
                listGroup = await _getGroupListController.GetList(new string[] { "Group", "GetAll" });

                listCurator.ToList().ForEach(CuratorList.Add);
                listCountry.ToList().ForEach(CountryList.Add);
                listGroup.ToList().ForEach(GroupList.Add);

            }
            catch (Exception e)
            {
                MessageBox.Error($"Произошла ошибка подключения. {e.Message}", "Получение данных");

                return;
            }
            finally
            {
                SetSplash(false);
            }
        }

        private async void LoadCityByCountry()
        {
            SetSplash(true);

            CityListBySelectedCountry = new ObservableCollection<City>();
            IEnumerable<City> listCity;

            try
            {
                if (SelectedCountry != null)
                {
                    listCity = await _getCityListByCountryController.GetListBySomething(new string[] { "City", "GetCityByCountry", "countryID" }, new string[] { SelectedCountry.ID.ToString() });

                    foreach (var item in listCity)
                    {
                        CityListBySelectedCountry.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Error($"Произошла ошибка подключения. {e.Message}", "Получение данных");

                return;
            }
            finally
            {
                SetSplash(false);
            }
        }
    }
}
