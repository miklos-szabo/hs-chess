namespace HSC.Mobile.Pages.ChessPage;

public partial class ChessClock : ContentView
{
    public static readonly BindableProperty MinutesProperty = BindableProperty.Create(nameof(Minutes), typeof(int), typeof(ChessClock), 0);
    public static readonly BindableProperty IncrementProperty = BindableProperty.Create(nameof(Increment), typeof(int), typeof(ChessClock), 0);
    public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(ChessClock), TimeSpan.Zero);

    public int Minutes
    {
        get => (int)GetValue(MinutesProperty);
        set => SetValue(MinutesProperty, value);
    }

    public int Increment
    {
        get => (int)GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    public TimeSpan Time
    {
        get => (TimeSpan)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public ChessClock()
	{
		InitializeComponent();
	}
}