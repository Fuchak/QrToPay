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
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            int userId = Preferences.Get("UserId", 0);
            var result = await _ticketService.GetHistoryAsync(userId);

            if (result.IsSuccess && result.Data != null)
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
}