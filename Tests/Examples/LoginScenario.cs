using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts;
using System.Text;
using Tests.Static;

namespace Tests.Examples;
public static class LoginScenario
{
    public static ScenarioProps GetScenario(HttpClient httpClient, int rate, TimeSpan interval, TimeSpan during)
    {
        return Scenario.Create("login_scenario", async context =>
        {
            //test data account valid until 2025-01-21
            StringContent bodyContent = new(
                "{\"email\":\"fufutrzak@vp.pl\",\"passwordHash\":\"Kargo2001?\"}",
                Encoding.UTF8,
                "application/json"
            );

            HttpRequestMessage request = Http.CreateRequest("POST", $"{StaticFields.baseUrl}api/Auth/login")
                .WithHeader("Content-Type", "application/json")
                .WithBody(bodyContent);

            Response<HttpResponseMessage> response = await Http.Send(httpClient, request);

            return response.StatusCode == StaticFields.Ok
                ? Response.Ok()
                : Response.Fail();
        })
        .WithLoadSimulations(Simulation.Inject(rate: rate, interval: interval, during: during));
    }
}