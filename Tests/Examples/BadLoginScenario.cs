﻿using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts;
using System.Text;
using Tests.Static;

namespace Tests.Examples;
public static class BadLoginScenario
{
    public static ScenarioProps GetScenario(HttpClient httpClient, int rate, TimeSpan interval, TimeSpan during)
    {
        return Scenario.Create("bad_login_scenario", async context =>
        {
            StringContent bodyContent = new(
                "{\"email\":\"fufutrzak@vp.pl\",\"passwordHash\":\"wrongPassword\"}",
                Encoding.UTF8,
                "application/json"
            );

            HttpRequestMessage request = Http.CreateRequest("POST", $"{StaticFields.baseUrl}api/Auth/login")
                .WithHeader("Content-Type", "application/json")
                .WithBody(bodyContent);

            Response<HttpResponseMessage> response = await Http.Send(httpClient, request);

            return response.StatusCode == StaticFields.BadRequest
                ? Response.Fail()
                : Response.Ok();
        })
        .WithLoadSimulations(Simulation.Inject(rate: rate, interval: interval, during: during));
    }
}