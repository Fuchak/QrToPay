using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts;
using Tests.Static;

namespace Tests.Examples;
public static class UserBalanceScenario
{
    public static ScenarioProps GetScenario(HttpClient httpClient, int rate, TimeSpan interval, TimeSpan during)
    {
        return Scenario.Create("user_balance_scenario", async context =>
        {
            HttpRequestMessage request = Http.CreateRequest("GET", $"{StaticFields.baseUrl}api/UserBalance/balance")
                .WithHeader("Authorization", $"Bearer {StaticFields.jwtToken}");

            Response<HttpResponseMessage> response = await Http.Send(httpClient, request);

            return response.StatusCode == StaticFields.Ok
                ? Response.Ok()
                : Response.Fail();
        })
        .WithLoadSimulations(Simulation.Inject(rate: rate, interval: interval, during: during));
    }
}