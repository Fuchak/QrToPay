﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Enums;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.FunFair;
public partial class FunFairCityViewModel : ViewModelBase
{
    private readonly CityService _cityService;
    private readonly AppState _appState;

    public FunFairCityViewModel(CityService cityService, AppState appState)
    {
        _cityService = cityService;
        _appState = appState;
    }

    [ObservableProperty]
    private ObservableCollection<City> cities = [];

    [RelayCommand]
    public async Task LoadCitiesAsync(int serviceType)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var result = await _cityService.GetCitiesAsync(serviceType);

            if (result.IsSuccess && result.Data != null)
            {
                Cities.Clear();
                foreach (var city in result.Data)
                {
                    Cities.Add(city);
                }
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage ?? "Nie udało się pobrać listy miast.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SelectCityAsync(City city)
    {
        _appState.UpdateServiceId(city.ServiceId);
        _appState.UpdateCityName(city.CityName);

        await NavigateAsync(nameof(FunFairPage));
    }
}