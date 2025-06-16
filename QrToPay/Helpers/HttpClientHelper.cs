using System.Net.Http.Headers;

namespace QrToPay.Helpers;

public class HttpClientHelper
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientHelper(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        string? jwtToken = await SecureStorage.Default.GetAsync(AppDataConst.AuthToken);
        if (string.IsNullOrWhiteSpace(jwtToken))
            throw new UnauthorizedAccessException("Token JWT jest pusty lub nie został znaleziony.");

        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        return client;
    }
}