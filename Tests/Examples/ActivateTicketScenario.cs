using NBomber.Contracts;
using NBomber.Http.CSharp;
using NBomber.CSharp;
using System.Text;
using Tests.Helpers;
using Tests.Static;

namespace Tests.Examples;

public static class ActivateTicketScenario
{
    public static ScenarioProps GetScenario(HttpClient httpClient, int rate, TimeSpan interval, TimeSpan during)
    {
        return Scenario.Create("activate_ticket_scenario", async context =>
        {
            string token = DataGenerator.GetRandomActivationToken();

            StringContent bodyContent = new(
                $"{{\"token\":\"{token}\"}}",
                Encoding.UTF8,
                "application/json"
            );

            HttpRequestMessage request = Http.CreateRequest("POST", $"{StaticFields.baseUrl}api/Tickets/activate")
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Authorization", $"Bearer {StaticFields.jwtToken}")
                .WithBody(bodyContent);

            Response<HttpResponseMessage> response = await Http.Send(httpClient, request);

            return response.StatusCode == StaticFields.Ok
                ? Response.Ok()
                : Response.Fail();
        })
        .WithLoadSimulations(Simulation.Inject(rate: rate, interval: interval, during: during));
    }
}