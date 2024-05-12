namespace QrToPay.ViewModels;
public partial class HelpViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? problem;

    [ObservableProperty]
    private string? subject;

    [RelayCommand]
    private async Task Confirm()
    {
        Email = string.Empty;
        Name = string.Empty;
        Problem = string.Empty;
        Subject = string.Empty;

        await Shell.Current.DisplayAlert("Potwierdzenie", "Twoja wiadomość została wysłana. Oczekiewany czas odpowiedzi 1-2 dni", "OK");
    }
}