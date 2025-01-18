using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts;
using Tests.Helpers;
using Tests.Static;

namespace Tests.Examples;
public static class CitiesScenario
{
    public static ScenarioProps GetScenario(HttpClient httpClient, int rate, TimeSpan interval, TimeSpan during)
    {
        return Scenario.Create("cities_scenario", async context =>
        {
            string serviceType = DataGenerator.GetRandomServiceType();

            HttpRequestMessage request = Http.CreateRequest("GET", $"{StaticFields.baseUrl}api/Cities?ServiceType={serviceType}")
                .WithHeader("Authorization", $"Bearer {StaticFields.jwtToken}");

            Response<HttpResponseMessage> response = await Http.Send(httpClient, request);

            return response.StatusCode == StaticFields.Ok
                ? Response.Ok()
                : Response.Fail();
        })
        .WithLoadSimulations(Simulation.Inject(rate: rate, interval: interval, during: during));
    }
}