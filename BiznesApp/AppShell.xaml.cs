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
}
