using BiznesApp.ViewModels;
using Microsoft.Maui.Controls;

namespace BiznesApp.Views
{
    public partial class OrderDetailsPage : ContentPage
    {
        public OrderDetailsPage(OrderDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
} 