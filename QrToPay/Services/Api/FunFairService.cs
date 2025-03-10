﻿using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;

public class FunFairService
{
    private readonly HttpClientHelper _httpClientHelper;

    public FunFairService(HttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public async Task<ServiceResult<List<FunFairPriceResponse>>> GetFunFairPricesAsync(int funFairId)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.GetAsync($"/api/FunFairs/prices?funFairId={funFairId}");

            if (response.IsSuccessStatusCode)
            {
                List<FunFairPriceResponse>? skiResortPrices = await response.Content.ReadFromJsonAsync<List<FunFairPriceResponse>>();
                if (skiResortPrices != null)
                {
                    return ServiceResult<List<FunFairPriceResponse>>.Success(skiResortPrices);
                }
                return ServiceResult<List<FunFairPriceResponse>>.Failure("Nie udało się pobrać cen biletów wesołego miasteczka.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Błąd podczas pobierania cen biletów wesołego miasteczka.";
                return ServiceResult<List<FunFairPriceResponse>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<FunFairPriceResponse>>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<List<FunFairsResponse>>> GetFunFairsAsync(string cityName)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.GetAsync($"/api/FunFairs/city?cityName={cityName}");

            if (response.IsSuccessStatusCode)
            {
                List<FunFairsResponse>? skiResorts = await response.Content.ReadFromJsonAsync<List<FunFairsResponse>>();
                if (skiResorts != null)
                {
                    return ServiceResult<List<FunFairsResponse>>.Success(skiResorts);
                }
                return ServiceResult<List<FunFairsResponse>>.Failure("Nie udało się pobrać danych dla wesołego miasteczka.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Błąd podczas pobierania danych dla wesołego miasteczka.";
                return ServiceResult<List<FunFairsResponse>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<FunFairsResponse>>.Failure(HttpError.HandleError(ex));
        }
    }
}