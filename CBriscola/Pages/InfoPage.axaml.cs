using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;

namespace CBriscola.Pages;

public partial class InfoPage : UserControl
{
    private static Avalonia.Platform.Storage.ILauncher? launcher=null;
    private static readonly Uri HomePage=new Uri("https://github.com/GiulianoSpaghetti/cbriscola.material");
    public InfoPage()
    {
        InitializeComponent();
        
    }
    private void OnSito_Click(object sender, RoutedEventArgs e)
    {
        if (launcher==null)
            launcher = TopLevel.GetTopLevel( (Avalonia.Visual) sender).Launcher;

        launcher.LaunchUriAsync(HomePage);
    }
}