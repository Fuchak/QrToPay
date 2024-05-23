namespace QrToPay.ViewModels;

public partial class MainViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    [RelayCommand]
    private Task NavigateToSkiPage() => NavigateAsync(nameof(SkiPage));

    [RelayCommand]
    private Task NavigateToFunfair() => NavigateAsync(nameof(FunFairPage));

    [RelayCommand]
    private Task NavigateToScanPage() => NavigateAsync(nameof(ScanQrCodePage));

    // Można dodać metodę do obsługi wywołań API, jeśli jest potrzebna
    /*[RelayCommand]
    private async Task CallApi()
    {
        var client = _httpClientFactory.CreateClient("ApiHttpClient");
        var response = await client.GetAsync("/weatherforecast/");

        if (response.IsSuccessStatusCode)
        {
            var resultJson = await response.Content.ReadAsStringAsync();
            // Obsługa danych JSON
        }
        else
        {
            // Obsługa błędu
        }
    }*/
}
