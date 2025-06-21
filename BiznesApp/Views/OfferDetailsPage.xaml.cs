using BiznesApp.ViewModels;

namespace BiznesApp.Views;

public partial class OfferDetailsPage : ContentPage
{
	public OfferDetailsPage(OfferDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
} 