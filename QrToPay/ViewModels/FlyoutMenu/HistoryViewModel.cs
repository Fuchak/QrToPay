using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace QrToPay.ViewModels.FlyoutMenu;

public partial class HistoryViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HistoryViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [ObservableProperty]
    private ObservableCollection<HistoryItemRequest> historyItems = new();

    public async Task LoadHistoryAsync()
    {
        int userId = Preferences.Get("UserId", 0);
        List<HistoryItemResponse> history = await GetTicketHistoryAsync(userId);

        Debug.WriteLine($"Received history data: {JsonSerializer.Serialize(history)}");

        foreach (var item in history)
        {
            HistoryItemRequest historyItem = new()
            {
                Date = item.Date,
                Type = item.Type,
                Name = item.Name,
                TotalPrice = item.TotalPrice
            };
            HistoryItems.Add(historyItem);
        }
    }

    private async Task<List<HistoryItemResponse>> GetTicketHistoryAsync(int userId)
    {
        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
        Debug.WriteLine($"Sending request to get history for UserID: {userId}");
        HttpResponseMessage response = await client.GetAsync($"/api/Tickets/getHistory/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine($"Failed to retrieve ticket history: {response.ReasonPhrase}");
            return new List<HistoryItemResponse>();
        }

        List<HistoryItemResponse>? result = await response.Content.ReadFromJsonAsync<List<HistoryItemResponse>>();
        Debug.WriteLine($"History data from API: {JsonSerializer.Serialize(result)}");
        return result ?? new List<HistoryItemResponse>();
    }
}