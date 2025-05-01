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


namespace RedBerryProject.Views.ReceiverPages
{
    /// <summary>
    /// Interaction logic for PersonalInfo.xaml
    /// </summary>
    public partial class PersonalInfo : Page
    {
        private readonly Action _openEditPage;
        private Receiver _receiver;
        
        public PersonalInfo(Action openEditPage, ref Receiver receiver)
        {
            InitializeComponent();
            _openEditPage = openEditPage;
            _receiver = receiver;
            ShowAllInfoOnUI(_receiver);
        }
        private void ButtonPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            _openEditPage.Invoke();
        }
        private void ShowAllInfoOnUI(Receiver receiver)
        {
            tbFullName.Text = $"{receiver.Surname} {receiver.FirstName} {receiver.MiddleName}";
            tbNationality.Text = $"{receiver.Nationality}";
            tbDateOfBirth.Text = $"{receiver.DateOfBirth}"; // тут може бути помилка
            tbBirthPlace.Text = $"{receiver.AddresOfBirth}";
            tbGender.Text = $"{receiver.Gender}";
            tbOfficialAddress.Text = $"{receiver.AddresOffical}";
            tbActualAddress.Text = $"{receiver.AddresCurrent}";
            tbPhoneNumber.Text = $"{receiver.PhoneNumber}";
            tbBankCardNumber.Text = $"{receiver.CardNumber}";
            //наступна чатина це парсинг фото
        }
    }
}
