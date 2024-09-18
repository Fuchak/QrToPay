using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.FlyoutMenu;
public partial class HelpViewModel : ViewModelBase
{
    private readonly HelpService _helpService;

    public HelpViewModel(HelpService helpService)
    {
        _helpService = helpService;
    }

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? subject;

    [ObservableProperty]
    private string? userName;

    [ObservableProperty]
    private string? userEmail;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private string? selectedSubject;

    //TODO można to dać do bazy i z bazy to czytać i tam updatować nie w kodzie
    public ObservableCollection<string> Subjects { get; } =
        [
            "Problem z logowaniem",
            "Błąd płatności",
            "Weryfikacja konta",
            "Inne"
        ];

    [RelayCommand]
    private async Task Submit()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(SelectedSubject))
            {
                ErrorMessage = "Wszystkie pola są wymagane.";
                return;
            }

            if (!ValidationHelper.IsEmail(UserEmail))
            {
                ErrorMessage = "Podaj poprawny adres e-mail.";
                return;
            }

            HelpFormRequest request = new()
            {
                UserName = UserName,
                UserEmail = UserEmail,
                Subject = SelectedSubject,
                Description = Description,
                Status = "Nowe"
            };

            var result = await _helpService.SubmitHelpFormAsync(request);

            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Sukces", "Twoje zgłoszenie zostało wysłane.", "OK");
                UserName = string.Empty;
                UserEmail = string.Empty;
                Description = string.Empty;
                SelectedSubject = null;
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
        //Może wydzielimy serwis pod iconnectivity z maui i to sprawdza czy jest internet będzie to działać lepiej?
        //https://learn.microsoft.com/pl-pl/dotnet/maui/platform-integration/communication/networking?view=net-maui-8.0&tabs=android
        finally
        {
            IsBusy = false;
        }
    }
}