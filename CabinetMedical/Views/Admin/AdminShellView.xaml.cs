using System.Windows;
using System.Windows.Controls;
using CabinetMedical.ViewModels.Admin;

namespace CabinetMedical.Views.Admin
{
    public partial class AdminShellView : UserControl
    {
        public AdminShellView()
        {
            InitializeComponent();
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender; //butonul apasat
            string tag = btn.Tag?.ToString() ?? "";

            if (string.IsNullOrEmpty(tag)) 
                return;

            if (DataContext is AdminShellViewModel vm)
            {
                vm.Navigate(tag); //incarca pagina respectiva
            }
        }
    }
}