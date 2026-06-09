using CabinetMedical.Models;
using CabinetMedical.Views;
using CabinetMedical.Views.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Threading;

namespace CabinetMedical.ViewModels.Admin
{
    public partial class AdminShellViewModel : ObservableObject
    {
        public string NumeUtilizator { get; set; }

        private string _currentDateTime = string.Empty;
        public string CurrentDateTime
        {
            get => _currentDateTime;
            set => SetProperty(ref _currentDateTime, value);
        }

        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        private DispatcherTimer _timer;
        private Utilizator _currentUser;

        
        public AdminShellViewModel(Utilizator user)
        {
            _currentUser = user;

            NumeUtilizator = $"{user.Nume} {user.Prenume}".Trim();
            if (string.IsNullOrEmpty(NumeUtilizator))
            {
                NumeUtilizator = user.NumeUtilizator;
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");
            _timer.Start();

            CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");
            CurrentView = new AdminDashboardView { DataContext = new AdminDashboardViewModel(_currentUser) };
        }

        //leaga interfata de codul din spate
        public void Navigate(string tag)
        {
            if (tag == "Dashboard")
            {
                CurrentView = new AdminDashboardView { DataContext = new AdminDashboardViewModel(_currentUser) };
            }
            else if (tag == "Utilizatori")
            {
                
                CurrentView = new AdminUtilizatoriView { DataContext = new AdminUtilizatoriViewModel() };
            }
            else if (tag == "Servicii")
            {
                
                CurrentView = new AdminServiciiView { DataContext = new AdminServiciiViewModel() };
            }
            else if (tag == "Rapoarte")
            {
                
                CurrentView = new AdminRapoarteView { DataContext = new AdminRapoarteViewModel() };
            }
            else if (tag == "Jurnal")
            {
                
                CurrentView = new AdminJurnalView { DataContext = new AdminJurnalViewModel() };
            }
        }

        [RelayCommand]
        public void Logout()
        {
            _timer.Stop();

            var loginWindow = new MainWindow();
            loginWindow.Show();

            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window != loginWindow)
                {
                    window.Close();
                }
            }
        }
    }
}