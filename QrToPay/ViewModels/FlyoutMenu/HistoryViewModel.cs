using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.FlyoutMenu;

public partial class HistoryViewModel : ViewModelBase
{
    private readonly TicketService _ticketService;
    private int pageNumber = 1;
    private const int pageSize = 5;

    public HistoryViewModel(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [ObservableProperty]
    private ObservableCollection<HistoryItemRequest> historyItems = [];

    [ObservableProperty]
    private bool hasHistory = true;

    public async Task LoadHistoryAsync()
    {
        if (IsBusy) return; // Jeśli jest w trakcie ładowania lub nie ma więcej danych, zakończ

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            // Tworzenie zapytania z danymi paginacji
            int userId = Preferences.Get("UserId", 0);
            HistoryRequest request = new()
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var result = await _ticketService.GetHistoryAsync(request);

            if (result.IsSuccess && result.Data != null && result.Data.Count > 0)
            {
                foreach (var item in result.Data)
                {
                    HistoryItems.Add(new HistoryItemRequest
                    {
                        Date = item.Date,
                        Type = item.Type.ToString(),
                        Name = item.Name,
                        TotalPrice = item.TotalPrice
                    });
                }

                if (result.Data.Count < pageSize)
                {
                    HasHistory = HistoryItems.Count > 0;
                }
                else
                {
                    pageNumber++; // Inkrementacja numeru strony tylko, gdy są dane
                }
                HasHistory = true;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
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

    [RelayCommand]
    private async Task LoadMoreAsync()
    {
        if (!IsBusy)
        {
            await LoadHistoryAsync();
        }
    }
}