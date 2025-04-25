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

namespace RedBerryProject.OauthPages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private Frame _parentFrame;
        private RegisterViewModel _viewModel;
        public Register(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            _viewModel = new RegisterViewModel();
            DataContext = _viewModel;
        }
        private void NavigateToAuth_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new Auth(_parentFrame));
        }
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
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
            //оновлення змінної firstPassword 
            _viewModel.FirstPassword = FirstHiddenPasswordBox.Password;
            if (!_viewModel.IsPasswordVisible) FirstVisiblePasswordBox.Text = _viewModel.FirstPassword;
        }
        private void SecondHiddenPasswordBox_PasswordChanged(object sender,EventArgs e) 
        {
            //оновлення змінної secondPassword
            _viewModel.SecondPassword = SecondHiddenPasswordBox.Password;
            if (!_viewModel.IsPasswordVisible) SecondVisiblePasswordBox.Text = _viewModel.SecondPassword;
        }
    }
}
