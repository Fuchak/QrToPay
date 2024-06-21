namespace QrToPay.View;

public partial class CityPage : ContentPage
{
    private readonly CityViewModel _viewModel;
    public string? AttractionType { get; set; }
    public CityPage(CityViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        _viewModel.AttractionType = AttractionType;
        await _viewModel.LoadCitiesAsync();
    }
}