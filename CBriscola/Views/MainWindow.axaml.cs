using Avalonia.Controls;
using CBriscola.Pages;
using System.Globalization;

namespace CBriscola.Views;

public partial class MainWindow : Window
{
    public static ResourceDictionary d;
    public static MainWindow Instance;

    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
        d = this.FindResource(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) as ResourceDictionary;
        if (d == null)
            d = this.FindResource("it") as ResourceDictionary;
        MainView.Traduci();
        HomePage.Traduci();
        OpzioniPage.Traduci();
        WindowState = WindowState.Maximized;
    }
}
