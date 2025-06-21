using BiznesApp.ViewModels;
using BiznesApp.Models;

namespace BiznesApp.Views;

public partial class OffersPage : ContentPage
{
	private readonly OffersViewModel _viewModel;
	public OffersPage(OffersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadOffersCommand.Execute(null);
	}

	private async void OnOfferSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is Offer selectedOffer)
		{
			await _viewModel.GoToOfferDetailsCommand.ExecuteAsync(selectedOffer);

			// Reset selection
			if (sender is CollectionView collectionView)
			{
				collectionView.SelectedItem = null;
			}
		}
	}
} 