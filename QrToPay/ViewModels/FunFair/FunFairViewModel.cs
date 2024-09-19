using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using QrToPay.View.FunFair;

namespace QrToPay.ViewModels.FunFair;
public partial class FunFairViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly FunFairService _funFairService;

    public FunFairViewModel(AppState appState, FunFairService funFairService)
    {
        _appState = appState;
        _funFairService = funFairService;
    }
    [ObservableProperty]
    private ObservableCollection<FunFairData> funFairs = [];

    [RelayCommand]
    public async Task LoadFunFairsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            var result = await _funFairService.GetFunFairsAsync(_appState.CityName!);

            if (result.IsSuccess && result.Data != null)
            {
                FunFairs.Clear();
                foreach (var skiResort in result.Data)
                {
                    FunFairs.Add(new FunFairData
                    {
                        FunFairId = skiResort.FunFairId,
                        ResortName = skiResort.ResortName,
                        ImageSource = "miasteczko.png"
                    });
                }
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GotoFunFairAsync(FunFairData funFair)
    {
        _appState.UpdateAttractionId(funFair.FunFairId);
        _appState.UpdateResortName(funFair.ResortName!);

        await NavigateAsync(nameof(FunFairPricesPage));
    }
}