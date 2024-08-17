﻿using System.Collections.ObjectModel;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;

namespace QrToPay.ViewModels.SkiResort;

public partial class SkiResortViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly IHttpClientFactory _httpClientFactory;

    public SkiResortViewModel(AppState appState, IHttpClientFactory httpClientFactory)
    {
        _appState = appState;
        _httpClientFactory = httpClientFactory;
    }
    [ObservableProperty]
    private ObservableCollection<Ticket> tickets = [];

    [ObservableProperty]
    private string? resortName;

    [ObservableProperty]
    private string? cityName;

    public async Task InitializeAsync()
    {
        ResortName = _appState.ResortName;
        CityName = _appState.CityName;

        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
        HttpResponseMessage response = await client.GetAsync($"/api/SkiResorts/prices?skiResortId={_appState.AttractionId}");
        response.EnsureSuccessStatusCode();

        List<SkiSlopePriceResponse>? skiSlopePrices = await response.Content.ReadFromJsonAsync<List<SkiSlopePriceResponse>>();

        if (skiSlopePrices != null)
        {
            Tickets.Clear();
            foreach (var price in skiSlopePrices)
            {
                Tickets.Add(new Ticket
                {
                    Points = price.Tokens,
                    Price = price.Price
                });
            }
        }
        OnPropertyChanged(nameof(Tickets));
    }

    [RelayCommand]
    public async Task GotoSkiResortBuyAsync(Ticket ticket)
    {
        _appState.Points = ticket.Points;
        _appState.Price = ticket.Price;

        await Shell.Current.GoToAsync($"{nameof(SkiResortBuyPage)}");
    }
}