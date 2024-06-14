using System.Collections.ObjectModel;

namespace QrToPay.ViewModels;

public partial class SkiViewModel : ViewModelBase
{
    [ObservableProperty]
    ObservableCollection<SkiResortModel> skiresorts;


    public SkiViewModel()
    {
        var initialSkiResorts = new List<SkiResortModel>
        {
            new () { ResortName = "Resort-1", City = "Zakopane", ImageSource = "stok.png" },
            new () { ResortName = "Resort-2", City = "Zakopane", ImageSource = "stok.png" },
            new () { ResortName = "Resort-1", City = "Szczyrk", ImageSource = "stok.png" },
            new () { ResortName = "Resort-1", City = "Krynica", ImageSource = "stok.png" },
            new () { ResortName = "Resort-1", City = "Białka Tatrzańska", ImageSource = "stok.png" },
            new () { ResortName = "Resort-1", City = "Szklarska Poręba", ImageSource = "stok.png" },
            new () { ResortName = "Resort-1", City = "Wisła", ImageSource = "stok.png" },
            new () { ResortName = "Resort-1", City = "Ustroń", ImageSource = "stok.png" }
        };

        skiresorts = new ObservableCollection<SkiResortModel>(initialSkiResorts);
    }

    [RelayCommand]
    Task GotoSkiResort(SkiResortModel skiresorts) => NavigateAsync($"{nameof(SkiResortPage)}?ResortName={skiresorts.ResortName}&City={skiresorts.City}");
}