using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Client
{
    public partial class ClientProfilViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly Utilizator _currentUser;

        private string _nume = "";
        public string Nume { get => _nume; set => SetProperty(ref _nume, value); }

        private string _prenume = "";
        public string Prenume { get => _prenume; set => SetProperty(ref _prenume, value); }

        private string _telefon = "";
        public string Telefon { get => _telefon; set => SetProperty(ref _telefon, value); }

        public ClientProfilViewModel(Utilizator user)
        {
            _context = new AppDbContext();
            _currentUser = user;

            //se initializeaza date din utilizatorul curent
            Nume = user.Nume ?? "";
            Prenume = user.Prenume ?? "";
            Telefon = user.Telefon ?? "";
        }

        [RelayCommand]
        public void SalveazaProfil()
        {
            var user = _context.Utilizatori.Find(_currentUser.Id);
            if (user != null)
            {
                user.Nume = Nume;
                user.Prenume = Prenume;
                user.Telefon = Telefon;

                _context.SaveChanges();
                MessageBox.Show("Datele au fost actualizate cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}