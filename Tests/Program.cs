using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts;
using System.Text;
using Tests.Static;
using Tests.Helpers;
using Tests.Examples;

namespace Tests;
class Program
{
    static void Main()
    {
        
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(StaticFields.baseUrl)
        };

        // Globalne parametry symulacji
        int rate = 5; // Liczba żądań na sekundę
        TimeSpan interval = TimeSpan.FromSeconds(1); // Odstęp między symulacjami
        TimeSpan during = TimeSpan.FromSeconds(60); // Czas trwania testu

        // Pobieranie scenariuszy z oddzielnych plików
        ScenarioProps loginScenario = LoginScenario.GetScenario(httpClient, rate, interval, during);
        ScenarioProps badLoginScenario = BadLoginScenario.GetScenario(httpClient, rate, interval, during);
        ScenarioProps ticketsScenario = TicketsScenario.GetScenario(httpClient, rate, interval, during);
        ScenarioProps userBalanceScenario = UserBalanceScenario.GetScenario(httpClient, rate, interval, during);
        ScenarioProps activateTicketScenario = ActivateTicketScenario.GetScenario(httpClient, rate, interval, during);
        ScenarioProps citiesScenario = CitiesScenario.GetScenario(httpClient, rate, interval, during);
        ScenarioProps scanQRCodeScenario = ScanQRCodeScenario.GetScenario(httpClient, rate, interval, during);
        ScenarioProps purchaseTicketScenario = PurchaseTicketScenario.GetScenario(httpClient, rate, interval, during);

        NBomberRunner
            .RegisterScenarios(
                loginScenario,
                badLoginScenario, 
                ticketsScenario, 
                userBalanceScenario,
                activateTicketScenario,
                citiesScenario,
                scanQRCodeScenario,
                purchaseTicketScenario
                )
            .Run();
    }
}
