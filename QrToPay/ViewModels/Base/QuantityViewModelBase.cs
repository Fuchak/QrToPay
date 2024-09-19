namespace QrToPay.ViewModels.Base;

public abstract partial class QuantityViewModelBase : ViewModelBase
{
    private bool _isButtonPressed;
    private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(200);
    private int _buttonDirection;

    [ObservableProperty]
    private int quantity = 1;

    [ObservableProperty]
    private decimal price;

    [ObservableProperty]
    private int points;

    protected abstract void UpdatePriceAndPoints();

    [RelayCommand]
    public void IncreaseQuantity()
    {
        if (Quantity < 999999)
        {
            Quantity++;
            UpdatePriceAndPoints();
        }
        else
        {
            ErrorMessage = "Osiągnięto maksymalna ilość biletów.";
        }
    }

    [RelayCommand]
    public void DecreaseQuantity()
    {
        if (Quantity > 1)
        {
            Quantity--;
            UpdatePriceAndPoints();
        }
    }

    public void StartRepeatingAction(int direction)
    {
        _buttonDirection = direction;
        _isButtonPressed = true;

        Shell.Current.Dispatcher.StartTimer(_interval, () =>
        {
            if (!_isButtonPressed) { return false; }

            if (_buttonDirection == 1) { IncreaseQuantity(); }

            else if (_buttonDirection == -1) { DecreaseQuantity(); }

            return true;
        });
    }

    public void StopRepeatingAction()
    {
        _isButtonPressed = false;
    }
}
