using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Material.Styles.Controls;

namespace CBriscola.Views;

public partial class MainView : UserControl
{

    public static MainView Instance;
    public MainView()
    {
        InitializeComponent();
        DrawerList.PointerReleased += DrawerSelectionChanged;
        Instance = this;
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
        Instance.HomeTitle.Content = MainWindow.d["Applicazione"] as string;
        Instance.OptionsTitle.Content = MainWindow.d["Opzioni"] as string;
        Instance.InfoTitle.Content = MainWindow.d["Informazioni"] as string;
    }
}