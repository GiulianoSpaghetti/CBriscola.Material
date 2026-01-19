using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CBriscola.Pages;
using Material.Styles.Controls;
using System.Collections.Generic;
using System.Globalization;

namespace CBriscola.Views;

public partial class MainView : UserControl
{
    public static ResourceDictionary Dictionary { get; private set; }
    internal static MainView Instance { get; private set; }
    public MainView()
    {
        InitializeComponent();
        DrawerList.PointerReleased += DrawerSelectionChanged;
        Instance = this;
        Dictionary = this.FindResource(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) as ResourceDictionary;
        if (Dictionary == null)
            Dictionary = this.FindResource("it") as ResourceDictionary;
        MainView.Traduci();
        HomePage.Traduci();
        OpzioniPage.Traduci();
    }
    public void DrawerSelectionChanged(object? sender, RoutedEventArgs? args)
    {
        if (sender is not ListBox listBox)
            return;

        if (!listBox.IsFocused && !listBox.IsKeyboardFocusWithin)
            return;
        try
        {
            PageCarousel.SelectedIndex = listBox.SelectedIndex;
        }
        catch
        {
            // ignored
        }

        LeftDrawer.OptionalCloseLeftDrawer();

    }

    public static void MakeNotification(string message)
    {
        SnackbarHost.Post(message, null, DispatcherPriority.Normal);
    }

    public static void Traduci()
    {
        Instance.HomeTitle.Content = MainView.Dictionary["Applicazione"] as string;
        Instance.OptionsTitle.Content = MainView.Dictionary["Opzioni"] as string;
        Instance.InfoTitle.Content = MainView.Dictionary["Informazioni"] as string;
    }
}