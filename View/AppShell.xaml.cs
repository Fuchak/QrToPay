namespace QrToPay.View;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = new ShellViewModel();

        // Obsługa zdarzenia rozpoczęcia nawigacji
        this.Navigating += OnNavigating;
        // Obsługa zdarzenia zakończenia nawigacji
        this.Navigated += OnNavigated;
    }
    private void OnNavigating(object? sender, ShellNavigatingEventArgs e)
    {
        // Wyłącz interakcję z flyout menu podczas nawigacji
        if (e.Target.Location.OriginalString.Contains("SkiPage") || e.Target.Location.OriginalString.Contains("FunfairPage"))
        {
            // Nie zmieniaj zachowania Flyout
            return;
        }
        this.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        // Przywróć standardowe zachowanie flyout po zakończeniu nawigacji
        this.FlyoutBehavior = FlyoutBehavior.Flyout;
    }
}