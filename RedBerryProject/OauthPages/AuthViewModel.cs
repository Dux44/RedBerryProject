using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.OauthPages
{
    public class AuthViewModel:ViewModelBase
    {
        private string _password;
        private bool _isPasswordVisible;

        public string Password
        {
            get => _password;
            set
            {
                if(_password != value)
                {
                    _password = value;
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
