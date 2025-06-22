using BiznesApp.ViewModels;
using BiznesApp.Views;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using System.Net.Http.Json;

namespace BiznesApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
            builder.Services.AddSingleton<IFilePicker>(FilePicker.Default);

            // Rejestracja usług
            builder.Services.AddSingleton<Services.DataService>();
            builder.Services.AddSingleton<Services.AuthService>();
            builder.Services.AddSingleton<Services.DatabaseService>();

            // Rejestracja ViewModeli
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<OrdersViewModel>();
            builder.Services.AddSingleton<OffersViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<ReportsViewModel>();
            builder.Services.AddTransient<EditOrderViewModel>();
            builder.Services.AddTransient<EditOfferViewModel>();
            builder.Services.AddTransient<OfferDetailsViewModel>();
            builder.Services.AddTransient<OrderDetailsViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();

            // Rejestracja Widoków (Stron)
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<OrdersPage>();
            builder.Services.AddSingleton<OffersPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<ReportsPage>();
            builder.Services.AddTransient<EditOrderPage>();
            builder.Services.AddTransient<EditOfferPage>();
            builder.Services.AddTransient<OfferDetailsPage>();
            builder.Services.AddTransient<OrderDetailsPage>();
            builder.Services.AddTransient<RegisterPage>();

            return builder.Build();
        }
    }
}
