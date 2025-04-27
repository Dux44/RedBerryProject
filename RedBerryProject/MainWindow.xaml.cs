using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RedBerryProject.Views.OauthPages;
using RedBerryProject.Views.ReceiverPages;

namespace RedBerryProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowAuthPage();
        }
        private void ShowAuthPage()
        {
            var authPage = new Auth();
            authPage.NavigateToRegister += ShowRegisterPage;
            authPage.AuthSucces += AuthPage_AuthSucces;
            MainFrame.Navigate(authPage);
        }
        private void ShowRegisterPage()
        {
            var registerPage = new Register();
            registerPage.NavigateToAuth += ShowAuthPage;
            MainFrame.Navigate(registerPage);
        }
        private void AuthPage_AuthSucces()
        {
            var receiverWindow = new ReceiverWindow();
            
            receiverWindow.Show();
            Close();
        }
    }
}