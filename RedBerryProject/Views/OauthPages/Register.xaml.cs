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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

using RedBerryProject.ViewModels.Oauth;
using RedBerryProject.Services;
using RedBerryProject.Models;

namespace RedBerryProject.Views.OauthPages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private RegisterViewModel _viewModel;

        private readonly string pattern = @"^(?=(?:.*\d){2,})(?=.*[A-Z]).*$"; //регулярка на обов'язково дві цифри і одна велика літера

        public event Action NavigateToAuth;
        public Register()
        {
            InitializeComponent();
            _viewModel = new RegisterViewModel();
            DataContext = _viewModel;
        }
        private void NavigateToAuth_Click(object sender, RoutedEventArgs e) //переадресація на сторінку Auth
        {
           NavigateToAuth?.Invoke(); //перехід до MainWindow де і відбувається логіка переходу між сторінками
        }
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e) // логіка поведінки кнопок show/hide і перемикання між TextBox і PasswordBox
        {
            _viewModel.IsPasswordVisible = !_viewModel.IsPasswordVisible;

            if (_viewModel.IsPasswordVisible)
            {
                FirstVisiblePasswordBox.Text = _viewModel.FirstPassword;
                FirstVisiblePasswordBox.Visibility = Visibility.Visible;
                FirstHiddenPasswordBox.Visibility = Visibility.Collapsed;

                SecondVisiblePasswordBox.Text = _viewModel.SecondPassword;
                SecondVisiblePasswordBox.Visibility = Visibility.Visible;
                SecondHiddenPasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                FirstHiddenPasswordBox.Password = FirstVisiblePasswordBox.Text;
                FirstVisiblePasswordBox.Visibility = Visibility.Collapsed;
                FirstHiddenPasswordBox.Visibility = Visibility.Visible;

                SecondHiddenPasswordBox.Password = SecondVisiblePasswordBox.Text;
                SecondVisiblePasswordBox.Visibility = Visibility.Collapsed;
                SecondHiddenPasswordBox.Visibility = Visibility.Visible;
            }

            FirstShowHideButton.Content = _viewModel.ShowPasswordText;
            SecondShowHideButton.Content = _viewModel.ShowPasswordText;
        }
        private void FirstHiddenPasswordBox_PasswordChanged(object sender, EventArgs e)
        {
            //оновлення тексту в обох текстбоксах FirstVisiblePasswordBox i FisrtHiddenPasswordBox
            _viewModel.FirstPassword = FirstHiddenPasswordBox.Password;
            FirstVisiblePasswordBox.Text = _viewModel.FirstPassword;

            FirstPasswordErrorMessage.Visibility = Visibility.Collapsed; //при будь-якій зміні поідомлення зникає
        }
        private void FirstVisiblePasswordBox_TextChanged(object sender, EventArgs e)
        {
            FirstPasswordErrorMessage.Visibility = Visibility.Collapsed; //при будь-якій зміні поідомлення зникає
        }
        private void SecondHiddenPasswordBox_PasswordChanged(object sender,EventArgs e) 
        {
            //оновлення тексту в обох текстбоксах SecondVisiblePasswordBox i SecondHiddenPasswordBox
            _viewModel.SecondPassword = SecondHiddenPasswordBox.Password;
            SecondVisiblePasswordBox.Text = _viewModel.SecondPassword;

            SecondPasswordErrorMessage.Visibility = Visibility.Collapsed; //при будь-якій зміні поідомлення зникає
        }
        private void SecondVisiblePasswordBox_TextChanged(object sender,EventArgs e)
        {
            SecondPasswordErrorMessage.Visibility = Visibility.Collapsed; //при будь-якій зміні поідомлення зникає
        }
        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            UsernameBoxError.Visibility = Visibility.Collapsed; //при будь-якій зміні поідомлення зникає
        }
        
        private void RegisterButton_Click(object sender, EventArgs e) //тут створити користувача і записати до бази і переадресувати на авторизацію
        {
            if (!ValidateFields()) return;

            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "database.db");
            var db = new DataBaseService(dbPath);

            if (db.UserExists(UsernameBox.Text))
            {
                UsernameBoxError.Text = "Це ім'я зайнятe!";
                UsernameBoxError.Visibility = Visibility.Visible;
                return;
            }

            //string hashedPassword = HashPassword(_viewModel.FirstPassword);
            var newUser = new User
            {
                Username = UsernameBox.Text,
                Password = _viewModel.FirstPassword, // краще хешувати!
                Role = cbIsUserAdmin.IsChecked == true ? "Admin" : "Receiver"
            };
            long userId = db.InsertUser(newUser);

            if(cbIsUserAdmin.IsChecked == true)
            {
                //TODO: перевірка comboBox.SelectedIndex ЧИ ОБРАНО ЯКЩО НІ НЕ ПУСКАТИ АДМІНА ДО РЕЄСТРАЦІЇ
                var newAdminData = new Admin
                {
                    IdUser = userId,
                    IdHelpPoint = cbNumberOfHelpPoint.SelectedIndex,
                };
                //запис у таблицю ADMINDATA
                db.InsertAdminData(newAdminData);
            }
            else if(cbIsUserAdmin.IsChecked == false)
            {
                var newUserData = new Receiver
                {
                    Id_user = userId,
                    DateOfBirth = new DateTime(1999, 1, 1),
                };
                //Запис у таблицю USERDATA
                db.InsertUserData(newUserData);
            }
            MessageBox.Show("Успішна реєстрація!");
            NavigateToAuth?.Invoke();
        }
        private bool ValidateFields()
        {
            bool emptyName = false;
            bool emptyFirstPassword = false;
            bool firstPasswordLenghtViolation = false;
            bool weakPassword = false;
            bool emptySecondPassword = false;
            bool passwordsDontMatch = false;

            if (string.IsNullOrWhiteSpace(UsernameBox.Text))
            {
                UsernameBoxError.Visibility = Visibility.Visible;
                emptyName = true;
            }
            if (string.IsNullOrEmpty(_viewModel.FirstPassword))
            {
                FirstPasswordErrorMessage.Text = "Це поле не може бути пустим!";
                FirstPasswordErrorMessage.Visibility = Visibility.Visible;
                emptyFirstPassword = true;
            }
            else
            {
                if (_viewModel.FirstPassword.Length < 5)
                {
                    FirstPasswordErrorMessage.Text = "Пароль має мати як найменш 5 символів!";
                    FirstPasswordErrorMessage.Visibility = Visibility.Visible;
                    firstPasswordLenghtViolation = true;
                }
                else
                {
                    if (!Regex.IsMatch(_viewModel.FirstPassword, pattern))
                    {
                        FirstPasswordErrorMessage.Text = "Необхідно мати хочаб 2 цифри та одну літеру у великому регістрі!";
                        FirstPasswordErrorMessage.Visibility = Visibility.Visible;
                        weakPassword = true;
                    }
                }
            }
            if (string.IsNullOrEmpty(_viewModel.SecondPassword))
            {
                SecondPasswordErrorMessage.Text = "Це поле не може бути пустим!";
                SecondPasswordErrorMessage.Visibility = Visibility.Visible;
                emptySecondPassword = true;
            }
            else
            {
                if (_viewModel.FirstPassword != _viewModel.SecondPassword)
                {
                    SecondPasswordErrorMessage.Text = "Паролі не співпадають!";
                    SecondPasswordErrorMessage.Visibility = Visibility.Visible;
                    passwordsDontMatch = true;
                }
            }
            if(!emptyName && !emptyFirstPassword && !emptySecondPassword && !passwordsDontMatch && !weakPassword && !firstPasswordLenghtViolation)
            {
                FirstPasswordErrorMessage.Visibility = Visibility.Collapsed;
                SecondPasswordErrorMessage.Visibility = Visibility.Collapsed;
                return true;
            }
            return false;
        }
    }
}
