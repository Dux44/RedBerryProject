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
using System.Windows.Shapes;
using RedBerryProject.Models;

namespace RedBerryProject.Views.ReceiverPages
{
    /// <summary>
    /// Interaction logic for ReceiverWindow.xaml
    /// </summary>
    public partial class ReceiverWindow : Window
    {
        /// <summary>
        /// TODO: Сторінка історія замовлень там показує усі замовлення зроблені даним користувачем
        /// </summary>
        private UserData _receiver;
        public ReceiverWindow(UserData receiverData, string username)
        {
            InitializeComponent();
            tbGreetingFrontPage.Text += username;
            _receiver = receiverData;
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
             MessageBoxResult choice = MessageBox.Show("Ви точно впевнені що хочете вийти?", "Вихід з додатку", MessageBoxButton.YesNo, MessageBoxImage.Question);
             if(choice == MessageBoxResult.Yes)
             {
                Close();
             }
            else
            {
                return;
            }
        }
        private void ButtonShowPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            OpenPersonalInfoPage();
        }
        private void OpenPersonalInfoEditPage()
        {
            var personalInfoEditPage = new PersonalInfoEdit(OpenPersonalInfoPage, ref _receiver);
            ReceiverFrame.Navigate(personalInfoEditPage);
        }
        private void OpenPersonalInfoPage()
        {
            var personalInfoPage = new PersonalInfo(OpenPersonalInfoEditPage, ref _receiver);
            ReceiverFrame.Navigate(personalInfoPage);
        }
        private void ButtonCreateApplicateion_Click(object sender, RoutedEventArgs e)
        {
            var application = new HelpApplication(_receiver);
            application.ShowDialog();
        }
        private void ButtonHistoryOfApplications_Click(object sender, RoutedEventArgs e)
        {
            var applicationHistory = new ApplicationsHistory(_receiver); ;
            ReceiverFrame.Navigate(applicationHistory);
        }
    }
}
