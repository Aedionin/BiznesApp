using BiznesApp.ViewModels;

namespace BiznesApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// Ustawienie początkowego stanu
		LogoStack.Opacity = 0;
		FormContainer.IsVisible = false; // Całkowicie ukryj na początku

		// Animacja logo
		await LogoStack.FadeTo(1, 400, Easing.CubicIn);
		
		// Przygotowanie i animacja formularza
		FormContainer.Opacity = 0;
		FormContainer.IsVisible = true;
		await FormContainer.FadeTo(1, 500, Easing.SinOut);
	}
} 