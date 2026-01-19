using Avalonia.Controls;
using CBriscola.Pages;
using System.ComponentModel;
using System.Globalization;

namespace CBriscola.Views;

public partial class MainWindow : Window
{
    internal static MainWindow Instance { get; private set; }
    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
    }
}
