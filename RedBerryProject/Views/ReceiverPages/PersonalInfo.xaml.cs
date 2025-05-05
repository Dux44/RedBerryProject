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
        private UserData _receiver;
        
        public PersonalInfo(Action openEditPage, ref UserData receiver)
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
        private void ShowAllInfoOnUI(UserData receiver)
        {
            tbFullName.Text = $"{receiver.secondname} {receiver.firstname} {receiver.middlename}";
            tbNationality.Text = receiver.nationality == "" ? "не вказано" : receiver.nationality;
            tbDateOfBirth.Text = $"{receiver.date_of_birth}"; 
            tbBirthPlace.Text = receiver.addres_of_birth == "" ? "не вказано" : receiver.addres_of_birth;
            tbGender.Text = receiver.gender == null? "не вказано" : receiver.gender.ToString();
            tbOfficialAddress.Text = receiver.addres_offical == "" ? "не вказано" : receiver.addres_offical;
            tbActualAddress.Text = receiver.addres_current == "" ? "не вказано" : receiver.addres_current;
            tbPhoneNumber.Text = receiver.phone_number == "" ? "не вказано" : receiver.phone_number;
            tbBankCardNumber.Text = receiver.card_number == "" ? "не вказано" : receiver.card_number;
            //фото не буде реалізовавно
        }
    }
}
