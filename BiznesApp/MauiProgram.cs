using BiznesApp.ViewModels;
using BiznesApp.Views;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using System.Net.Http.Json;
using Microsoft.Maui.Devices;

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

            builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
            builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
            builder.Services.AddSingleton<IFilePicker>(FilePicker.Default);

            // --- Konfiguracja Klienta HTTP i Serwisów ---

            var baseAddress = "https://biznesapp-api-dk123-awf4dqcbctfjfzb8.westeurope-01.azurewebsites.net";

            // Konfiguracja HttpClient dla AuthService
            builder.Services.AddHttpClient<Services.AuthService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });

            // Konfiguracja HttpClient dla DataService
            builder.Services.AddHttpClient<Services.DataService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });

            // Konfiguracja HttpClient dla geokodowania (EditOfferViewModel)
            builder.Services.AddHttpClient("GeocodingClient", client =>
            {
                client.BaseAddress = new Uri("https://nominatim.openstreetmap.org");
                client.DefaultRequestHeaders.Add("User-Agent", "BiznesApp/1.0");
            });

            // Rejestracja usług
            builder.Services.AddSingleton<Services.DatabaseService>();

            // Rejestracja ViewModeli
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<OrdersViewModel>();
            builder.Services.AddSingleton<OffersViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<ReportsViewModel>();
            builder.Services.AddTransient<EditOrderViewModel>(provider =>
            {
                var dataService = provider.GetRequiredService<Services.DataService>();
                var mediaPicker = provider.GetRequiredService<IMediaPicker>();
                var geolocation = provider.GetRequiredService<IGeolocation>();
                var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("GeocodingClient");
                return new EditOrderViewModel(dataService, mediaPicker, geolocation, httpClient);
            });
            builder.Services.AddTransient<EditOfferViewModel>(provider =>
            {
                var dataService = provider.GetRequiredService<Services.DataService>();
                var geolocation = provider.GetRequiredService<IGeolocation>();
                var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("GeocodingClient");
                return new EditOfferViewModel(dataService, geolocation, httpClient);
            });
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
