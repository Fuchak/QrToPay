using System.Diagnostics;
using System.Threading.Tasks;
using QrToPay.ViewModels.FunFair;

namespace QrToPay.View;

public partial class FunFairPage : ContentPage
{
    public FunFairPage(FunFairViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}