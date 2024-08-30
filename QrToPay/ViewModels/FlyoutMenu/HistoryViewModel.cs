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
    private ObservableCollection<HistoryItemRequest> historyItems = [];

    [ObservableProperty]
    private bool hasHistory = true;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBusy;

    public async Task LoadHistoryAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            int userId = Preferences.Get("UserId", 0);
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            List<HistoryItemResponse>? response = await client.GetFromJsonAsync<List<HistoryItemResponse>>($"/api/Tickets/getHistory?userId={userId}");

            if (response != null && response.Count > 0)
            {
                foreach (var item in response)
                {
                    HistoryItems.Add(new HistoryItemRequest
                    {
                        Date = item.Date,
                        Type = item.Type.ToString(),
                        Name = item.Name,
                        TotalPrice = item.TotalPrice
                    });
                }
                HasHistory = true;
            }
            else
            {
                ErrorMessage = "Błąd: Nie udało się pobrać historii biletów.";
                HasHistory = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
            HasHistory = false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}