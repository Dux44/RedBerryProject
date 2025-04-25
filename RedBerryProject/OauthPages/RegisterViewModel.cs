using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.OauthPages
{
    public class RegisterViewModel:ViewModelBase
    {
        private string _firstPassword;
        private string _secondPassword;
        private bool _isPasswordVisible;

        public string FirstPassword
        {
            get => _firstPassword;
            set
            {
                if(_firstPassword != value)
                {
                    _firstPassword = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SecondPassword
        {
            get => _secondPassword;
            set
            {
                if (_secondPassword != value)
                {
                    _secondPassword = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                if(_isPasswordVisible != value)
                {
                    _isPasswordVisible = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsPasswordVisible));
                }
            }
        }
        public string ShowPasswordText => IsPasswordVisible ? "Hide" : "Show";
    }
}
