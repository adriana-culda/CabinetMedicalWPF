using CabinetMedical.Models;
using CabinetMedical.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CabinetMedical.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly Action<Utilizator> _loginSuccessCallback;

        private string _username = "";
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _errorMessage="";
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthService authService, Action<Utilizator> onLoginSuccess)
        {
            _authService = authService;
            _loginSuccessCallback = onLoginSuccess;
            LoginCommand = new RelayCommand<object>(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox?.Password;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Te rog introdu username și parolă.";
                return;
            }

            var utilizator = _authService.Autentificare(Username, password ?? "");

            if (utilizator == null)
            {
                ErrorMessage = "Username sau parolă incorectă!";
                return;
            }

            _loginSuccessCallback?.Invoke(utilizator);
        }
    }
}