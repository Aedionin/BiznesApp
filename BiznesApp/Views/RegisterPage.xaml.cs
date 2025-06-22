using BiznesApp.ViewModels;
using System.Threading.Tasks;

namespace BiznesApp.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		TitleStack.Opacity = 0;
		FormContainer.Opacity = 0;
		TitleStack.TranslationY = -50;
		FormContainer.TranslationY = 50;

		await Task.WhenAll(
			TitleStack.FadeTo(1, 600, Easing.CubicIn),
			TitleStack.TranslateTo(0, 0, 600, Easing.CubicIn)
		);

		await Task.WhenAll(
			FormContainer.FadeTo(1, 800, Easing.SinOut),
			FormContainer.TranslateTo(0, 0, 800, Easing.SinOut)
		);
	}
} 