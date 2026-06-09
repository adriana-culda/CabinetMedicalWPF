using System.Windows;
using System.Windows.Controls;
using CabinetMedical.ViewModels.Angajat;

namespace CabinetMedical.Views.Angajat
{
    public partial class AngajatShellView : UserControl
    {
        public AngajatShellView()
        {
            InitializeComponent();
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                if (DataContext is AngajatShellViewModel vm)
                {
                    vm.Navigate(tag);
                }
            }
        }
    }
}