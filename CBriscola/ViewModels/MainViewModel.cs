using Avalonia.Controls;
using org.altervista.numerone.framework;
using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace CBriscola.ViewModels;

public class MainViewModel : ViewModelBase
{
    internal static MainViewModel Instance { get; private set; } = null;

    public static MainViewModel GetMainViewModelInstance()
    {
        if (Instance == null)
        {
            Instance = new MainViewModel();
        }   
        return Instance;
    }
}
