using BiznesApp.Views;

namespace BiznesApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(EditOrderPage), typeof(EditOrderPage));
        Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(OfferDetailsPage), typeof(OfferDetailsPage));
        Routing.RegisterRoute(nameof(EditOfferPage), typeof(EditOfferPage));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var authToken = await SecureStorage.GetAsync("auth_token");
        if (!string.IsNullOrEmpty(authToken))
        {
            // Użytkownik jest już zalogowany, przejdź do głównej strony
            await GoToAsync("//MainFlow/OrdersPage");
        }
        // Jeśli nie, aplikacja domyślnie pozostanie na LoginPage zdefiniowanej w AppShell.xaml
    }
}
