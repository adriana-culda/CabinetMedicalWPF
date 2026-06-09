using CabinetMedical.Models;
using CabinetMedical.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Threading;

namespace CabinetMedical.ViewModels.Angajat
{
    public partial class AngajatShellViewModel : ObservableObject
    {
        public string NumeAngajat { get; set; }

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

        public AngajatShellViewModel(Utilizator user)
        {
            _currentUser = user;
            NumeAngajat = $"{user.Nume} {user.Prenume}".Trim();
            if (string.IsNullOrEmpty(NumeAngajat))
            {
                NumeAngajat = user.NumeUtilizator;
            }

            //ceasul din dreapta sus
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");
            _timer.Start();

            CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");

            //se deschide automat imediat ce se logheaza angajatul
            CurrentView = new Views.Angajat.AngajatDashboardView { DataContext = new AngajatDashboardViewModel(_currentUser) };
        }

        public void Navigate(string tag)
        {
            if (tag == "Dashboard")
            {
                CurrentView = new Views.Angajat.AngajatDashboardView { DataContext = new AngajatDashboardViewModel(_currentUser) };
            }
            else if (tag == "Programari")
            {
                CurrentView = new Views.Angajat.AngajatProgramariView { DataContext = new AngajatProgramariViewModel(_currentUser) };
            }
            else if (tag == "Pacienti")
            {
               
                CurrentView = new Views.Angajat.AngajatPacientiView { DataContext = new AngajatPacientiViewModel() };
            }
        }

        [RelayCommand]
        public void Logout()
        {
            _timer.Stop();

            //fereastra de login
            var loginWindow = new MainWindow();
            loginWindow.Show();

            //inchide fereastra curentă
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