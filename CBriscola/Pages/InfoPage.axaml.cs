using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

namespace CBriscola.Pages;

public partial class InfoPage : UserControl
{
    public InfoPage()
    {
        InitializeComponent();

    }
    private void OnSito_Click(object sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "https://github.com/GiulianoSpaghetti/cbriscola.material",
            UseShellExecute = true
        };
        Process.Start(psi);
    }
}