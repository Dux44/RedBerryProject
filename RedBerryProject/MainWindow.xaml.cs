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
using RedBerryProject.Models;

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
            authPage.AuthSuccesUser += AuthPage_AuthSucces;
            authPage.AuthSuccesAdmin += AuthPage_AuthSuccesAdmin;
            MainFrame.Navigate(authPage);
        }

       

        private void ShowRegisterPage()
        {
            var registerPage = new Register();
            registerPage.NavigateToAuth += ShowAuthPage;
            MainFrame.Navigate(registerPage);
        }
        private void AuthPage_AuthSucces(Receiver userData, string username)
        {
           
            var receiverWindow = new ReceiverWindow(userData,username);
            
            receiverWindow.Show();
            Close();
        }
        private void AuthPage_AuthSuccesAdmin(Admin admin, string username)
        {
            throw new NotImplementedException();
        }
    }
}