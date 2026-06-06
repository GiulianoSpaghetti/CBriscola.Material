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
    internal static InfoPage Instance { get; private set; } = null;
    private static Avalonia.Platform.Storage.ILauncher? launcher=null;
    public static readonly Uri HomePage=new Uri("https://www.opencode.net/numerone/CBriscola-Material");
    public InfoPage()
    {
        DataContext ??= MainViewModel.GetMainViewModelInstance();
        InitializeComponent();
        Instance = this;
    }

    public static void Traduci()
    {
        Instance.TranslatorCredit.Text = $"Translator: {MainView.Dictionary["Autore"]}";
        if (MainView.Dictionary["Revisore"].ToString().Trim() != "")
            Instance.RevisorCredit.Text = $"Revisor: {MainView.Dictionary["Revisore"]}";
    }
    private void OnSito_Click(object sender, RoutedEventArgs e)
    {
        if (launcher==null)
            launcher = TopLevel.GetTopLevel( (Avalonia.Visual) sender).Launcher;
        launcher.LaunchUriAsync(HomePage);
    }
}
