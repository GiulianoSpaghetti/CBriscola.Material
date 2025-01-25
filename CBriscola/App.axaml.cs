using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using CBriscola.ViewModels;
using CBriscola.Views;
using System;
using System.IO;
using System.Text;

namespace CBriscola;

public partial class App : Application
{
    public static string SistemaOperativo;
    public static string path;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (OperatingSystem.IsWindows())
            {
                path = "C:\\Program Files\\wxBriscola";
                SistemaOperativo = Environment.OSVersion.ToString();
            }
            else if (OperatingSystem.IsLinux())
            {
                StreamReader streamReader;
                try
                {
                    streamReader = new StreamReader(File.OpenRead("/sys/devices/virtual/dmi/id/product_sku"), Encoding.UTF8, true, 128);
                    SistemaOperativo = streamReader.ReadLine();
                    streamReader.Close();
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    SistemaOperativo = "SKU";
                }
                catch (System.IO.FileNotFoundException)
                {
                    SistemaOperativo = "SKU";
                }
                if (SistemaOperativo=="SKU")
                	SistemaOperativo = "GNU/Linux";
                path = "/usr/share/wxBriscola";
            }


            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
