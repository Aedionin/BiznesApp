using BiznesApp.ViewModels;
using Microsoft.Maui.Controls;
using BiznesApp.Models;

namespace BiznesApp.Views
{
    public partial class EditOrderPage : ContentPage
    {
        public EditOrderPage(EditOrderViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 