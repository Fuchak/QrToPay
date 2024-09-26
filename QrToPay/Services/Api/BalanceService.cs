using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using QrToPay.Models.Requests;
using System.Text.Json;
using QrToPay.Models.Responses;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QrToPay.Services.Api;
public class BalanceService
{
    private readonly HttpClientHelper _httpClientHelper;

    public BalanceService(HttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public event EventHandler<decimal>? BalanceUpdated;

    public async Task<ServiceResult<decimal>> LoadUserDataAsync()
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.GetAsync($"/api/UserBalance/balance");

            if (response.IsSuccessStatusCode)
            {
                BalanceResponse? balanceResponse = await response.Content.ReadFromJsonAsync<BalanceResponse>();
                if (balanceResponse != null)
                {
                    decimal balance = balanceResponse.AccountBalance;
                    BalanceUpdated?.Invoke(this, balance);
                    return ServiceResult<decimal>.Success(balance);
                }
                return ServiceResult<decimal>.Failure("Błąd podczas odczytywania salda konta.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<decimal>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<decimal>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<decimal>> TopUpAccountAsync(TopUpRequest topUpRequest)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/UserBalance/topUp", topUpRequest);

            if (response.IsSuccessStatusCode)
            {
                BalanceResponse? balanceResponse = await response.Content.ReadFromJsonAsync<BalanceResponse>();
                if (balanceResponse != null)
                {
                    decimal accountBalance = balanceResponse.AccountBalance;
                    BalanceUpdated?.Invoke(this, accountBalance);
                    return ServiceResult<decimal>.Success(accountBalance);
                }
                return ServiceResult<decimal>.Failure("Nie udało się pobrać nowego salda konta.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<decimal>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<decimal>.Failure(HttpError.HandleError(ex));
        }
    }
}