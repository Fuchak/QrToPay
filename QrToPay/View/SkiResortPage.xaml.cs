namespace QrToPay.View;

public partial class SkiResortPage : ContentPage
{
    private readonly SkiResortViewModel _viewModel;
    public SkiResortPage(SkiResortViewModel vm)
	{
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        await _viewModel.InitializeAsync();
    }
}