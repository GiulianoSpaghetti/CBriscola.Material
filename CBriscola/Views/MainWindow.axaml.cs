using Avalonia.Controls;
using CBriscola.Pages;
using System.Globalization;

namespace CBriscola.Views;

public partial class MainWindow : Window
{
    private static ResourceDictionary dic;
    public static ResourceDictionary d {  get => dic; }
    private static MainWindow instance;
    public static MainWindow Instance { get => instance; }

    public MainWindow()
    {
        InitializeComponent();
        instance = this;
        dic = this.FindResource(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) as ResourceDictionary;
        if (dic == null)
            dic = this.FindResource("it") as ResourceDictionary;
        MainView.Traduci();
        HomePage.Traduci();
        OpzioniPage.Traduci();
        WindowState = WindowState.Maximized;
    }
}
