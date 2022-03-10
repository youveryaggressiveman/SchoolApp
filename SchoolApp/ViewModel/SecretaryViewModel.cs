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
using MessageBox = HandyControl.Controls.MessageBox;

namespace SchoolApp.ViewModel
{
    public class SecretaryViewModel : BaseViewModel
    {
        private readonly UniversalController<object> _postNewStudentGroupController;
        private readonly UniversalController<City> _getCityListByCountryController;
        private readonly UniversalController<Employee> _getCuratorListController;
        private readonly UniversalController<Country> _getCountryListController;
        private readonly UniversalController<Group> _getGroupListController;

        private ObservableCollection<Group> _groupList;
        private ObservableCollection<Employee> _curatorList;
        private ObservableCollection<User> _studentList;
        private ObservableCollection<Country> _countryList;
        private ObservableCollection<City> _cityListBySelectedCountry;

        private Country _selectedCountry;
        private City _selectedCity;
        private Address _addressByNewUser;
        private Passport _passportByNewUser;
        private Group _selectedGroup;
        private Employee _selectedCurator;
        private User _newUser;

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

        public Address AddressByNewUser
        {
            get => _addressByNewUser;
            set
            {
                _addressByNewUser = value;
                OnPropertyChanged(nameof(AddressByNewUser));
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

        public Passport PassportByNewUser
        {
            get => _passportByNewUser;
            set
            {
                _passportByNewUser = value;
                OnPropertyChanged(nameof(PassportByNewUser));
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

        public ObservableCollection<User> StudentList
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

        public User NewUser
        {
            get => _newUser;
            set
            {
                _newUser = value;
                OnPropertyChanged(nameof(NewUser));
            }
        }

        public ICommand AddNewGroupInDatabase { get; private set; }
        public ICommand AddNewUserInGroup { get; private set; }

        public SecretaryViewModel()
        {
            _postNewStudentGroupController = new UniversalController<object>();
            _getCityListByCountryController = new UniversalController<City>();
            _getCuratorListController = new UniversalController<Employee>();
            _getCountryListController = new UniversalController<Country>();
            _getGroupListController = new UniversalController<Group>();

            GroupList = new ObservableCollection<Group>();
            CountryList = new ObservableCollection<Country>();
            CuratorList = new ObservableCollection<Employee>();
            StudentList = new ObservableCollection<User>();
            CityListBySelectedCountry = new ObservableCollection<City>();

            AddNewGroupInDatabase = new DelegateCommand(AddNewGroup);
            AddNewUserInGroup = new DelegateCommand(AddNewUser);

            LoadAllInfo();
        }

        private async void AddNewGroup(object obj)
        {
            if (StudentList.Count != 0)
            {
                MessageBox.Show("Список группы не может быть пустым", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            (null as MainViewModel).SetSplash(true);

            try
            {
                if (await _postNewStudentGroupController.PostListSomething(StudentList, new string[]{}, new string[]{}))
                {
                    MessageBox.Show("Данные о новой группе успешно занеслись в БД", "Занесение данных", MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    return;
                }

                MessageBox.Show("Данные не смогли занестись. Попробуйте позже", "Занесение данных", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка подключения", "Получение данных", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }
            finally
            {
                (null as MainViewModel).SetSplash(false);
            }
        }

        private void AddNewUser(object obj)
        {
            if (string.IsNullOrEmpty(NewUser.FirstName) || string.IsNullOrEmpty(NewUser.SecondName) ||
                string.IsNullOrEmpty(NewUser.LastName) || string.IsNullOrEmpty(NewUser.Password) ||
                string.IsNullOrEmpty(NewUser.Email) || string.IsNullOrEmpty(PassportByNewUser.PassportNumber) ||
                string.IsNullOrEmpty(PassportByNewUser.PassportSerial) || PassportByNewUser.DateBith == DateTime.Now ||
                SelectedGroup == null || SelectedCountry == null || SelectedCity == null ||
                string.IsNullOrEmpty(AddressByNewUser.AddressName) ||
                string.IsNullOrEmpty(AddressByNewUser.AddressNumber))
            {
                MessageBox.Show("Заполните все поля", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            User newUser = new User()
            {
                FirstName = NewUser.FirstName,
                SecondName = NewUser.SecondName,
                LastName = NewUser.LastName,
                Password = NewUser.Password,
                Email = NewUser.Email,
                Passport = PassportByNewUser,
                Address = new Address()
                {
                    AddressName = AddressByNewUser.AddressName,
                    AddressNumber = AddressByNewUser.AddressNumber,
                    City = new City()
                    {
                        Name = SelectedCity.Name,
                        Country = new Country()
                        {
                            Name = SelectedCountry.Name
                        }
                    }
                }
            };

            StudentList.Add(newUser);
        }

        private async void LoadAllInfo()
        {
            (null as MainViewModel).SetSplash(true);

            IEnumerable<Group> listGroup;
            IEnumerable<Country> listCountry;
            IEnumerable<Employee> listCurator;

            try
            {
                listCurator = await _getCuratorListController.GetList(new string[] {"Employees"});
                listCountry = await _getCountryListController.GetList(new string[] {"Countries"});
                listGroup = await _getGroupListController.GetList(new string[] {"Groups"});
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка подключения", "Получение данных", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }
            finally
            {
                (null as MainViewModel).SetSplash(false);
            }
        }

        private async void LoadCityByCountry()
        {
            (null as MainViewModel).SetSplash(true);

            IEnumerable<City> listCity;

            try
            {
                listCity = await _getCityListByCountryController.GetListBySomething(new string[]{}, new string[]{});

                foreach (var item in listCity)
                {
                    CityListBySelectedCountry.Add(item);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка подключения", "Получение данных", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }
            finally
            {
                (null as MainViewModel).SetSplash(false);
            }
        }
    }
}
