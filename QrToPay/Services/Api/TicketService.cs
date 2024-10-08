using System.Diagnostics;
using System.Net.Http.Json;
using Org.Apache.Http.Protocol;
using QrToPay.Models.Common;
using QrToPay.Models.Requests;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;

public class TicketService
{
    private readonly HttpClientHelper _httpClientHelper;

    public TicketService(HttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public async Task<ServiceResult<string>> GenerateAndUpdateTicketAsync(UpdateTicketRequest updateRequest)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Tickets/purchase", updateRequest);

            if (response.IsSuccessStatusCode)
            {
                UpdateTicketResponse? result = await response.Content.ReadFromJsonAsync<UpdateTicketResponse>();
                if (result != null)
                {
                    return ServiceResult<string>.Success(result.QrCode);
                }
                return ServiceResult<string>.Failure("Nie udało się przetworzyć płatności");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<string>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<List<ActiveTicketsResponse>>> GetActiveTicketsAsync()
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.GetAsync($"/api/Tickets/active");

            if (response.IsSuccessStatusCode)
            {
                List<ActiveTicketsResponse>? tickets = await response.Content.ReadFromJsonAsync<List<ActiveTicketsResponse>>();
                if (tickets != null)
                {
                    return ServiceResult<List<ActiveTicketsResponse>>.Success(tickets);
                }
                return ServiceResult<List<ActiveTicketsResponse>>.Failure("Błąd podczas odczytywania listy biletów.");
            }
            else 
            { 
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<List<ActiveTicketsResponse>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<ActiveTicketsResponse>>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<List<HistoryItemResponse>>> GetHistoryAsync(HistoryRequest request)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.GetAsync($"/api/Tickets/history?pageNumber={request.PageNumber}&pageSize={request.PageSize}");

            var responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("Response Content: " + responseContent);

            if (response.IsSuccessStatusCode)
            {
                List<HistoryItemResponse>? historyItems = await response.Content.ReadFromJsonAsync<List<HistoryItemResponse>>();
                if (historyItems != null)
                {
                    return ServiceResult<List<HistoryItemResponse>>.Success(historyItems);
                }
                return ServiceResult<List<HistoryItemResponse>>.Failure("Nie udało się odczytać historii biletów.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<List<HistoryItemResponse>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
            return ServiceResult<List<HistoryItemResponse>>.Failure(HttpError.HandleError(ex));
        }
    }
}