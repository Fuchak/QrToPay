using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace QrToPay.ViewModels;

public partial class HistoryViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<HistoryItem> historyItems = [];

    public async Task LoadHistoryAsync()
    {
        var userId = Preferences.Get("UserId", 0);
        var history = await GetTicketHistoryAsync(userId);

        Debug.WriteLine($"Received history data: {JsonSerializer.Serialize(history)}");

        foreach (var item in history)
        {
            var historyItem = new HistoryItem
            {
                Date = item.Date,
                Type = item.Type,
                TotalPrice = item.TotalPrice
            };
            HistoryItems.Add(historyItem);
        }
    }

    private async Task<List<HistoryItem>> GetTicketHistoryAsync(int userId)
    {
        var client = httpClientFactory.CreateClient("ApiHttpClient");
        Debug.WriteLine($"Sending request to get history for UserID: {userId}");
        var response = await client.GetAsync($"/tickets/getHistory/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine($"Failed to retrieve ticket history: {response.ReasonPhrase}");
            return new List<HistoryItem>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<HistoryItem>>();
        Debug.WriteLine($"History data from API: {JsonSerializer.Serialize(result)}");
        return result ?? new List<HistoryItem>();
    }
}