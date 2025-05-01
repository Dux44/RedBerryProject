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

using RedBerryProject.Models;
using RedBerryProject.Services;

namespace RedBerryProject.Views.ReceiverPages
{
    /// <summary>
    /// Interaction logic for PersonalInfoEdit.xaml
    /// </summary>
    public partial class PersonalInfoEdit : Page
    {
        private Receiver _receiver;
        private readonly Action _openPersonalInfoPage; 
        public PersonalInfoEdit(Action openPersonalInfoPage, ref Receiver receiver)
        {
            InitializeComponent();
            _openPersonalInfoPage = openPersonalInfoPage;
            _receiver = receiver;
        }
        private void ButtonConfirmEditing_Click(object sender, RoutedEventArgs e)
        {
            

        }
        private void ButtonCancelEdiging_Click(object sender, RoutedEventArgs e)
        {
            _openPersonalInfoPage.Invoke();
        }
    }
}
