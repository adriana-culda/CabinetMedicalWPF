using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Admin
{
    public class AdminJurnalViewModel : ObservableObject
    {
        private ObservableCollection<JurnalAudit> _jurnalList = new();
        public ObservableCollection<JurnalAudit> JurnalList
        {
            get => _jurnalList;
            set => SetProperty(ref _jurnalList, value);
        }

        public AdminJurnalViewModel()
        {
            IncarcaJurnal();
        }

        public void IncarcaJurnal()
        {
            using (var context = new AppDbContext())
            {
                var logs = context.Jurnale.OrderByDescending(j => j.DataOra).ToList();
                JurnalList = new ObservableCollection<JurnalAudit>(logs);
            }
        }
    }
}