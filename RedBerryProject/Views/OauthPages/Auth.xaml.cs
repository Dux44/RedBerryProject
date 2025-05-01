using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using RedBerryProject.Services;
using RedBerryProject.ViewModels.Oauth;
using RedBerryProject.Models;

namespace RedBerryProject.Views.OauthPages
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        //fields
        private AuthViewModel _viewModel; //це як частина MVVM уся логіка яка динамічно прив'язана до INotifyPropertyChanged має бути винесена у окремий клас

        //events
        public event Action NavigateToRegister;
        public delegate void AuthSuccesUserHandler(Receiver receiver, string username);
        public event AuthSuccesUserHandler AuthSuccesUser;

        public delegate void AuthSuccesAdminHandler(Admin admin, string username);
        public event AuthSuccesAdminHandler AuthSuccesAdmin;
        public Auth()
        {
            InitializeComponent();
            _viewModel = new AuthViewModel();
            DataContext = _viewModel;
        }
        private void NavigateToRegister_Click(object sender, RoutedEventArgs e) //навігація на сторінку реєстрації
        {
            NavigateToRegister?.Invoke();
        }
        private void HiddenPassworddBox_PasswordChanged(object sender, RoutedEventArgs e) //збереження усього написаного поки обєктом для запису є PasswordBox
        {
            _viewModel.Password = HiddenPasswordBox.Password;
            if (!_viewModel.IsPasswordVisible) VisiblePasswordBox.Text = _viewModel.Password;

            PasswordErrorMessage.Visibility= Visibility.Collapsed;
        }
        private void VisiblePasswordBox_TextChanged(object sender, RoutedEventArgs e)
        {
            PasswordErrorMessage.Visibility= Visibility.Collapsed;
        }
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e) //зміна UI обєкта PasswordBox на TextBox логіка роботи Кнопки Show/Hide додатково передається вміст у властивість
        {
            _viewModel.IsPasswordVisible = !_viewModel.IsPasswordVisible;
            if (_viewModel.IsPasswordVisible)
            {
                VisiblePasswordBox.Text = _viewModel.Password;
                VisiblePasswordBox.Visibility=Visibility.Visible;
                HiddenPasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                HiddenPasswordBox.Password = VisiblePasswordBox.Text;
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
                HiddenPasswordBox.Visibility= Visibility.Visible;
            }
            ShowHideButton.Content = _viewModel.ShowPasswordText; //зміна тексту кнопки
        }
        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsernameBoxError.Visibility = Visibility.Collapsed;
        }
        private void AuthButton_Click(object sender, RoutedEventArgs e) //пошук користувча з бази даних та передача даних у його сторінку
        {
            if (!ValidateFields()) return;

            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "database.db");
            var db = new DataBaseService(dbPath);

            var user = db.GetUserByUsername(UsernameBox.Text); //тут краще поміняти логіку і отримувати увесь обєкт після перевірки на пароль ОПТИМІЗАЦІЯ

            if (user == null)
            {
                UsernameBoxError.Text = "Користувача не знайдено!";
                UsernameBoxError.Visibility = Visibility.Visible;
                return;
            }
            if(user.Password != _viewModel.Password)
            {
                PasswordErrorMessage.Text = "Неправильний пароль або логін!";
                PasswordErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            if(user.Role == "Receiver")
            {

                var userData = db.GetUserDataByUserId(user.Id);
                AuthSuccesUser?.Invoke(userData, user.Username);
                MessageBox.Show("Пароль коректний вхожу до особистого кабінету користувача");

            }
            else if(user.Role == "Admin")
            {
                var adminData = db.GetAdminDataByUserId(user.Id);
                AuthSuccesAdmin?.Invoke(adminData, user.Username);
                MessageBox.Show($"Пароль коректний вхожу до робочої зони пункту №{adminData.IdHelpPoint}");
            }

            
           
            
        }
        private bool ValidateFields()
        {
            //валідація першого поля на пустоту
            //перевірка пароля з бази даних
            //тоді пустити користувача у його особистий кабінет
            bool emptyName = false;
            //bool nameDontExists = false; //додаткова перевірка чи ім'я існує в базі даних
            bool emptyPassword = false;
            //bool incorrectPassword = false;
            if (string.IsNullOrWhiteSpace(UsernameBox.Text))
            {
                UsernameBoxError.Visibility = Visibility.Visible;
                emptyName = true;
            }
            if (string.IsNullOrWhiteSpace(_viewModel.Password)) 
            {
                PasswordErrorMessage.Text = "Це поле не може бути пустим!";
                PasswordErrorMessage.Visibility = Visibility.Visible;
                emptyPassword = true;    
            }
            if (!emptyName && !emptyPassword)
            {
                return true;
            }
            return false;
        }
    }
}
