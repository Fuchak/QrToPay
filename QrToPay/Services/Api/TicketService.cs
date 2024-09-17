﻿using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Requests;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;

public class TicketService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GenerateAndUpdateTicketAsync(UpdateTicketRequest updateRequest)
    {
        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        HttpResponseMessage response = await client.PostAsJsonAsync("/api/Tickets/purchase", updateRequest);

        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        UpdateTicketResponse? result = await response.Content.ReadFromJsonAsync<UpdateTicketResponse>();
        return result?.QrCode ?? string.Empty;
    }

    public async Task<ServiceResult<List<Ticket>>> GetActiveTicketsAsync(int userId)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Tickets/active?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                List<Ticket>? tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
                if (tickets != null)
                {
                    return ServiceResult<List<Ticket>>.Success(tickets);
                }
                return ServiceResult<List<Ticket>>.Failure("Błąd podczas odczytywania listy biletów.");
            }
            else 
            { 
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<List<Ticket>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<Ticket>>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<List<HistoryItemResponse>>> GetHistoryAsync(HistoryRequest request)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Tickets/history?userId={request.UserId}&pageNumber={request.PageNumber}&pageSize={request.PageSize}");

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
            return ServiceResult<List<HistoryItemResponse>>.Failure(HttpError.HandleError(ex));
        }
    }
}