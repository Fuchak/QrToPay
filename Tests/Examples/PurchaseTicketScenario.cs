using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts;
using System.Text;
using Tests.Static;

namespace Tests.Examples;
public static class PurchaseTicketScenario
{
    public static ScenarioProps GetScenario(HttpClient httpClient, int rate, TimeSpan interval, TimeSpan during)
    {
        return Scenario.Create("purchase_ticket_scenario", async context =>
        {
            StringContent bodyContent = new(
                "{\"serviceId\":1,\"quantity\":1,\"tokens\":1,\"totalPrice\":1}",
                Encoding.UTF8,
                "application/json"
            );

            HttpRequestMessage request = Http.CreateRequest("POST", $"{StaticFields.baseUrl}api/Tickets/purchase")
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