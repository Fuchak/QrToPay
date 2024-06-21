namespace QrToPay.View;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryViewModel _viewModel;
    public HistoryPage(HistoryViewModel vm)
	{
		InitializeComponent();
		BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadHistoryAsync();
    }
}