using BiznesApp.Models;
using BiznesApp.ViewModels;
using Microsoft.Maui.Controls;

namespace BiznesApp.Views
{
    public partial class OrdersPage : ContentPage
    {
        private readonly OrdersViewModel _viewModel;
        public OrdersPage(OrdersViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadOrdersCommand.Execute(null);
        }

        private async void OnOrderSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Order selectedOrder)
            {
                await Shell.Current.GoToAsync(nameof(OrderDetailsPage), true, new Dictionary<string, object>
                {
                    { "SelectedOrder", selectedOrder }
                });

                // Reset selection
                if (sender is CollectionView collectionView)
                {
                    collectionView.SelectedItem = null;
                }
            }
        }
    }
} 