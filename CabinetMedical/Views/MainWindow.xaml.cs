using System;
using System.Linq;
using System.Windows;
using CabinetMedical.Data;
using CabinetMedical.Models;
using CabinetMedical.Services;
using CabinetMedical.ViewModels;
using CabinetMedical.Views;

namespace CabinetMedical.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); //incarca designul XAML

            var loginView = new LoginView();

            //configurare viewmodel pentru autentificare
            loginView.DataContext = new LoginViewModel
                (
                new AuthService(new AppDbContext()),
                (utilizator) => //se executa doar dupa logare cu succes
                {
                    //redimensionare fereastra pentru Dashboard
                    this.Width = 1200;
                    this.Height = 800;
                    this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                    this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;

                    //incarcarea paginii pentru rolul respectiv
                    if (utilizator.Rol == "Admin")
                    {
                        var adminShell = new CabinetMedical.Views.Admin.AdminShellView();
                        adminShell.DataContext = new CabinetMedical.ViewModels.Admin.AdminShellViewModel(utilizator);
                        this.MainContent.Content = adminShell;
                    }
                    else if (utilizator.Rol == "Doctor" || utilizator.Rol == "Asistenta" || utilizator.Rol == "Asistentă")
                    {
                        var angajatShell = new CabinetMedical.Views.Angajat.AngajatShellView();
                        angajatShell.DataContext = new CabinetMedical.ViewModels.Angajat.AngajatShellViewModel(utilizator);
                        this.MainContent.Content = angajatShell;
                    }
                    else if (utilizator.Rol == "Client" || utilizator.Rol == "Pacient")
                    {
                        var clientShell = new CabinetMedical.Views.Client.ClientShellView();
                        var clientShellVm = new CabinetMedical.ViewModels.Client.ClientShellViewModel(utilizator);
                        clientShell.DataContext = clientShellVm;
                        this.MainContent.Content = clientShell;
                    }
                    else //pentru roluri inexistente
                    {
                        MessageBox.Show($"Rolul '{utilizator.Rol}' nu este recunoscut de sistem.", "Eroare Logare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            );

            this.MainContent.Content = loginView; //ecran initial de logare
        }
    }
}