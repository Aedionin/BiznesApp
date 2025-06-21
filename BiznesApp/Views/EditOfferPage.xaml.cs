using BiznesApp.ViewModels;

namespace BiznesApp.Views;

public partial class EditOfferPage : ContentPage
{
	public EditOfferPage(EditOfferViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void OnCancelClicked(object sender, System.EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
} 