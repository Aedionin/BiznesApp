using BiznesApp.ViewModels;

namespace BiznesApp.Views;

public partial class EditOfferPage : ContentPage
{
	public EditOfferPage(EditOfferViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is EditOfferViewModel vm)
		{
			await vm.OnAppearing();
		}
	}

	private async void OnCancelClicked(object sender, System.EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
} 