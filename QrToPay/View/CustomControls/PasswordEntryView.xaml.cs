namespace QrToPay.View.CustomControl;

public partial class PasswordEntryView : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(PasswordEntryView), string.Empty, defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public PasswordEntryView()
    {
        InitializeComponent();
    }
}
