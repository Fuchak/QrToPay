using System.Collections.ObjectModel;
using System.Diagnostics;

namespace QrToPay.ViewModels;

public partial class SkiResortViewModel : ViewModelBase, IQueryAttributable
{
    [ObservableProperty]
    private ObservableCollection<Ticket> tickets;

    [ObservableProperty]
    private string? resortName;

    [ObservableProperty]
    private string? city;

    public string Title => $"{ResortName}\n{City}";

    public SkiResortViewModel()
    {
        Tickets=[
                new() { BiletType = "Normalny ", Points = "30 pkt", Price = "45 zł" },
                new() { BiletType = "Ulgowy ", Points = "30 pkt", Price = "42 zł" }
            ];
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ResortName = Uri.UnescapeDataString(query["ResortName"] as string ?? string.Empty);
        City = Uri.UnescapeDataString(query["City"] as string ?? string.Empty);

        OnPropertyChanged(nameof(Title));
    }

    [RelayCommand]
    Task GotoSkiResortBuy(Ticket tickets) => 
        NavigateAsync($"{nameof(SkiResortBuyPage)}?ResortName={ResortName}&City={City}&BiletType={tickets.BiletType}&Points={tickets.Points}&Price={tickets.Price}&Title={Title}");
}
