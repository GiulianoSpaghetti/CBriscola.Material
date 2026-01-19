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
    private static string sistemaOperativo;
    private static string path;
    public static string Path { get => path; }
    public static string SistemaOperativo { get => sistemaOperativo; }
    internal static bool PuoChiudersi { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            PuoChiudersi = true;
            if (OperatingSystem.IsWindows())
            {
                path = "C:\\Program Files\\wxBriscola";
                sistemaOperativo = Environment.OSVersion.ToString();
            }
            else if (OperatingSystem.IsLinux())
            {
                StreamReader streamReader;
                try
                {
                    streamReader = new StreamReader(File.OpenRead("/sys/devices/virtual/dmi/id/product_sku"), Encoding.ASCII, true, 128);
                    sistemaOperativo = streamReader.ReadLine().ToString();
                    streamReader.Close();
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    sistemaOperativo = "SKU";
                }
                catch (System.IO.FileNotFoundException)
                {
                    sistemaOperativo = "SKU";
                }
                if (SistemaOperativo == "SKU")
                    sistemaOperativo = "GNU/Linux";
                path = "/usr/share/wxBriscola";
            }


            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            sistemaOperativo = Environment.OSVersion.ToString();
            PuoChiudersi = false;
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }

}
