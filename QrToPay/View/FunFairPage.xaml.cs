using System.Diagnostics;
using System.Threading.Tasks;

namespace QrToPay.View;

public partial class FunFairPage : ContentPage
{
    public FunFairPage(FunFairViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}