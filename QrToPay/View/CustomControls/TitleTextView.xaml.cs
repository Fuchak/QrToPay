namespace QrToPay.View.CustomControls;

public partial class TitleTextView : ContentView
{
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(TitleTextView), string.Empty);

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public TitleTextView()
	{
		InitializeComponent();
	}
}