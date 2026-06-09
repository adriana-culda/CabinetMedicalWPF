using CabinetMedical.Models; 
using CabinetMedical.ViewModels.Admin;
using CabinetMedical.ViewModels.Angajat;
using CabinetMedical.ViewModels.Client;
using CabinetMedical.Views.Admin; 
using CabinetMedical.Views.Angajat;
using CabinetMedical.Views.Client;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CabinetMedical.ViewModels
{
    public partial class MainShellViewModel : ObservableObject
    {
        public Utilizator? CurrentUser { get; set; }

        [ObservableProperty]
        private object? _currentView;

        public void Navigate(string tag)
        {
            if (CurrentUser == null) return;

            CurrentView = tag switch
            {
                "Dashboard" => CurrentUser.Rol switch
                {
                    "Admin" => new AdminDashboardView
                    {
                        DataContext = new AdminDashboardViewModel(CurrentUser)
                    },
                    "Angajat" => new AngajatDashboardView
                    {
                        DataContext = new AngajatDashboardViewModel(CurrentUser)
                    },
                    "Client" => new ClientDashboardView
                    {
                        DataContext = new ClientDashboardViewModel(CurrentUser)
                    },
                    _ => null
                },
                _ => null
            };
        }
    }
}