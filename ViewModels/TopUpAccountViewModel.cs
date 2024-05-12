using Microsoft.Maui.Controls;
using System.Xml.Linq;

namespace QrToPay.ViewModels;
public partial class TopUpAccountViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? amount;

    [RelayCommand]
    private async Task NavigateToTopUp()
    {
        Amount = string.Empty;

        await Shell.Current.GoToAsync("TransferPage");
    }
}