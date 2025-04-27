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

namespace RedBerryProject.Views.ReceiverPages
{
    /// <summary>
    /// Interaction logic for PersonalInfo.xaml
    /// </summary>
    public partial class PersonalInfo : Page
    {
        private Frame _parentFrame;
        public PersonalInfo(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
        }
        private void ButtonPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            var personalInfoEdit = new PersonalInfoEdit(_parentFrame);
            _parentFrame.Navigate(personalInfoEdit);
        }
    }
}
