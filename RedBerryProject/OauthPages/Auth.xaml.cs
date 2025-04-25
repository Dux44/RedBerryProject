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

namespace RedBerryProject.OauthPages
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        //fields
        private Frame _parentFrame; //тут зберігається посилання на основну панель наякій буде відображено UI 
        private AuthViewModel _viewModel; //це як частина MVVM уся логіка яка динамічно прив'язана до INotifyPropertyChanged має бути винесена у окремий клас
        
        //events
        public Auth(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            _viewModel = new AuthViewModel();
            DataContext = _viewModel;
        }
        private void NavigateToRegister_Click(object sender, RoutedEventArgs e) //навігація на сторінку реєстрації
        {
            _parentFrame.Navigate(new Register(_parentFrame));
        }
        private void HiddenPassworddBox_PasswordChanged(object sender, RoutedEventArgs e) //збереження усього написаного поки обєктом для запису є PasswordBox
        {
            _viewModel.Password = HiddenPasswordBox.Password;
            if (!_viewModel.IsPasswordVisible) VisiblePasswordBox.Text = _viewModel.Password;
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
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            //валідація першого поля на пустоту
            //перевірка пароля з бази даних
            //тоді пустити користувача у його особистий кабінет
        }
    }
}
