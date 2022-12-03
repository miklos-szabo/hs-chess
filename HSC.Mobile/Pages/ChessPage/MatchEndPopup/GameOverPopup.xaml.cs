using CommunityToolkit.Maui.Views;
using HSC.Mobile.Resources.Translation;
using HSCApi;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.ChessPage.MatchEndPopup;

public partial class GameOverPopup : Popup
{
    private readonly IStringLocalizer<Translation> _localizer;

    public string Main { get; set; }
    public string Details { get; set; }
    public string MoneyChange { get; set; }
    public Color MoneyChangeColor { get; set; }

    public GameOverPopup(SearchSimpleResult simpleResult, Result result, decimal amount, IStringLocalizer<Translation> localizer)
    {
        _localizer = localizer;
        Application.Current.Resources.TryGetValue("TextColor", out var baseTextColor);

        if (simpleResult == SearchSimpleResult.Draw)
        {
            Main = _localizer["Result.Draw"];
            MoneyChange = $"{_localizer["Result.Nochange"]}";
            MoneyChangeColor = (Color)baseTextColor;
        }
        else if (simpleResult == SearchSimpleResult.Victory)
        {
            Main = _localizer["Result.Victory"];
            MoneyChange = $"{_localizer["Result.Won"]} {amount:F2}$";
            MoneyChangeColor = Color.FromArgb("#00c414");
        }
        else
        {
            Main = _localizer["Result.Defeat"];
            MoneyChange = $"{_localizer["Result.Lost"]} {amount:F2}$";
            MoneyChangeColor = Color.FromArgb("#de0404");
        }

        Details = _localizer[$"Result.{result}"];
        InitializeComponent();
    }
}