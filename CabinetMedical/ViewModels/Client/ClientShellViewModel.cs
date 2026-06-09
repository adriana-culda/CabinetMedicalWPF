using CabinetMedical.Data; // Asigură-te că ai importat asta
using CabinetMedical.Models;
using CabinetMedical.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Threading;

namespace CabinetMedical.ViewModels.Client
{
    public partial class ClientShellViewModel : ObservableObject
    {
        public string NumeClient { get; set; }

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

        public ClientShellViewModel(Utilizator user)
        {
            _currentUser = user;
            NumeClient = $"{user.Nume} {user.Prenume}".Trim();
            if (string.IsNullOrEmpty(NumeClient)) NumeClient = user.NumeUtilizator;

            //ceasul
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");
            _timer.Start();
            CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");

            //dashboard-ul este pagina implicita
            CurrentView = new Views.Client.ClientDashboardView { DataContext = new ClientDashboardViewModel(_currentUser) };
        }

        [RelayCommand]
        public void Navigate(string tag)
        {
            switch (tag)
            {
                case "Dashboard":
                    CurrentView = new Views.Client.ClientDashboardView { DataContext = new ClientDashboardViewModel(_currentUser) };
                    break;
                case "Servicii":
                    CurrentView = new Views.Client.ClientServiciiView { DataContext = new ClientServiciiViewModel() };
                    break;
                case "ProgramareNoua":
                    CurrentView = new Views.Client.ClientProgramareNouaView { DataContext = new ClientProgramareNouaViewModel(_currentUser) };
                    break;
                case "Istoric":
                    CurrentView = new Views.Client.ClientIstoricView { DataContext = new ClientIstoricViewModel(_currentUser) };
                    break;
                case "Profil":
                    CurrentView = new Views.Client.ClientProfilView { DataContext = new ClientProfilViewModel(_currentUser) };
                    break;
            }
        }

        [RelayCommand]
        public void Logout()
        {
            _timer.Stop();

            var mainWindow = new MainWindow();
            mainWindow.Show();

            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window != mainWindow)
                {
                    window.Close();
                }
            }
        }
    }
}