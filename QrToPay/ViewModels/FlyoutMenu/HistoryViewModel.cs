using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Collections.ObjectModel;
using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.ViewModels.FlyoutMenu;

public partial class HistoryViewModel : ViewModelBase
{
    private readonly TicketService _ticketService;
    private readonly CacheService _cacheService;

    private int pageNumber = 1;
    private const int pageSize = 50;
    private bool hasMoreItems = true;

    public HistoryViewModel(TicketService ticketService, CacheService cacheService)
    {
        _ticketService = ticketService;
        _cacheService = cacheService;
    }

    [ObservableProperty]
    private ObservableCollection<HistoryItemRequest> historyItems = [];

    [ObservableProperty]
    private bool hasHistory = true;


    public async Task LoadHistoryAsync()
    {
        if (IsBusy || !hasMoreItems) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var cachedHistory = await _cacheService.LoadFromCacheAsync<IEnumerable<HistoryItemRequest>>(AppDataConst.TicketHistory);
            if (cachedHistory != null && pageNumber == 1)
            {
                CacheService.UpdateCollection(HistoryItems, cachedHistory);
                hasMoreItems = cachedHistory.Count() >= pageSize;
            }

            HistoryRequest request = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var result = await _ticketService.GetHistoryAsync(request);

            if (result.IsSuccess && result.Data != null && result.Data.Count > 0)
            {
                var newHistoryItems = result.Data.Select(item => new HistoryItemRequest
                {
                    Date = item.Date,
                    Type = item.Type.ToString(),
                    Name = item.Name,
                    TotalPrice = item.TotalPrice
                });

                if (!CacheService.AreDataEqual(HistoryItems, newHistoryItems))
                {
                    await _cacheService.SaveToCacheAsync(AppDataConst.TicketHistory, newHistoryItems);
                    CacheService.UpdateCollection(HistoryItems, newHistoryItems);
                }


                if (result.Data.Count < pageSize)
                {
                    hasMoreItems = false;
                }
                else
                {
                    pageNumber++;
                }
                HasHistory = true;
            }
            else
            {
                if (HistoryItems.Count == 0)
                {
                    ErrorMessage = result.ErrorMessage;
                    HasHistory = false;
                }
                hasMoreItems = false;
            }
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