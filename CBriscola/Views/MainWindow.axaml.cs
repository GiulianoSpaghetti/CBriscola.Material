using Avalonia.Controls;
using CBriscola.Pages;
using System.Globalization;

namespace CBriscola.Views;

public partial class MainWindow : Window
{
    public static ResourceDictionary Dictionary { get; private set; }
    public static MainWindow Instance { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
        Dictionary = this.FindResource(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) as ResourceDictionary;
        if (Dictionary == null)
            Dictionary = this.FindResource("it") as ResourceDictionary;
        MainView.Traduci();
        HomePage.Traduci();
        OpzioniPage.Traduci();
        WindowState = WindowState.Maximized;
    }
}
