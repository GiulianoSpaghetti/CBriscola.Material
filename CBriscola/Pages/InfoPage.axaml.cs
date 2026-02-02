using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CBriscola.ViewModels;
using CBriscola.Views;
using System;
using System.Diagnostics;

namespace CBriscola.Pages;

public partial class InfoPage : UserControl
{
    private static Avalonia.Platform.Storage.ILauncher? launcher=null;
    public static readonly Uri HomePage=new Uri("https://www.opencode.net/numerone/CBriscola-Material");
    public InfoPage()
    {
        DataContext ??= MainViewModel.GetMainViewModelInstance();
        InitializeComponent();
        TranslatorCredit.Content = $"Translator: {MainView.Dictionary["Autore"]}";
        if (MainView.Dictionary["Traduttore"].ToString().Trim()!="")
            TranslatorCredit.Content += $"Revisor: {MainView.Dictionary["Traduttore"]}";

    }
    private void OnSito_Click(object sender, RoutedEventArgs e)
    {
        if (launcher==null)
            launcher = TopLevel.GetTopLevel( (Avalonia.Visual) sender).Launcher;

        launcher.LaunchUriAsync(HomePage);
    }
}
