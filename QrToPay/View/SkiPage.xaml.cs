using System.Diagnostics;
using System.Threading.Tasks;

namespace QrToPay.View;

public partial class SkiPage : ContentPage
{
    private readonly SkiViewModel _viewModel;
    public SkiPage(SkiViewModel vm)
	{
		InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadSkiResortsAsync();
    }
}