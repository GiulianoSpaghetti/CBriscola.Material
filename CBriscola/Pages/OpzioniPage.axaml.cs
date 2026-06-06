using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CBriscola.ViewModels;
using CBriscola.Views;
using Material.Styles.Controls;
using Metsys.Bson;
using org.altervista.numerone.framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace CBriscola.Pages;

public partial class OpzioniPage : UserControl
{
    internal static OpzioniPage Instance { get; private set; }
    private readonly string dirs = System.IO.Path.Combine(App.Path, "Mazzi");
    public OpzioniPage()
    {
        DataContext ??= MainViewModel.GetMainViewModelInstance();
        InitializeComponent();
        Instance = this;
        lsmazzi.Items.Clear();
        txtNomeUtente.Text = ((MainViewModel)DataContext).NomeUtente;
        txtCpu.Text = ((MainViewModel)DataContext).NomeCpu;
        tsCartaBriscola.IsChecked = ((MainViewModel)DataContext).BriscolaDaPunti;
        tsAvvisaTallone.IsChecked = ((MainViewModel)DataContext).AvvisaTalloneFinito;
        tsStessoSeme.IsChecked = ((MainViewModel)DataContext).StessoSeme;
        cbLivello.SelectedIndex = ((MainViewModel)DataContext).Livello - 1;
        List<ListBoxItem> mazzi;
        List<String> path;
        cbLivello.SelectedIndex = ((MainViewModel)DataContext).Livello - 1;
        ListBoxItem item;
        String s1 = "";

        try
        {
            path = new List<String>(Directory.EnumerateDirectories(dirs));
        }
        catch (System.IO.DirectoryNotFoundException ex)
        {
            path = new List<string>();
        }
        for (UInt16 i = 0; i < path.Count; i++)
        {
            path[i] = path[i].Substring(path[i].LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);

        }
        if (!path.Contains("Napoletano"))
            path.Add("Napoletano");
        path.Sort();
        foreach (String s in path)
        {
            item = new ListBoxItem();
            item.Content = s;
            lsmazzi.Items.Add(item);

        }
    }

    public static void Traduci()
    {
        Instance.lbCartaBriscola.Text = $"{MainView.Dictionary["BriscolaDaPunti"]}";
        Instance.lbAvvisaTallone.Text = $"{MainView.Dictionary["AvvisaTallone"]}";
        Instance.opNomeUtente.Text = $"{MainView.Dictionary["NomeUtente"]}: ";
        Instance.opNomeCpu.Text = $"{MainView.Dictionary["NomeCpu"]}: ";
        Instance.lbmazzi.Text = $"{MainView.Dictionary["Mazzo"]}";
        Instance.lbLivello.Text = $"{MainView.Dictionary["Livello"]}";
        Instance.lbStessoSeme.Text = $"{MainView.Dictionary["VarianteStessoSeme"]}";

    }
    public void OnOk_Click(Object source, RoutedEventArgs evt)
    {
        String s;
        ((MainViewModel)DataContext).NomeUtente = txtNomeUtente.Text;
        ((MainViewModel)DataContext).NomeCpu = txtCpu.Text;

        if (tsCartaBriscola.IsChecked == true)
            ((MainViewModel)DataContext).BriscolaDaPunti = true;
        else
            ((MainViewModel)DataContext).BriscolaDaPunti = false;
        if (tsAvvisaTallone.IsChecked == true)
            ((MainViewModel)DataContext).AvvisaTalloneFinito = true;
        else
            ((MainViewModel)DataContext).AvvisaTalloneFinito = false;
        if (tsStessoSeme.IsChecked == true)
            ((MainViewModel)DataContext).StessoSeme = true;
        else
            ((MainViewModel)DataContext).StessoSeme = false;
        ListBoxItem i = (ListBoxItem)lsmazzi.SelectedItem;
        if (i != null)
            ((MainViewModel)DataContext).NomeMazzo =(string)i.Content;
        ((MainViewModel)DataContext).Livello = (UInt16) (cbLivello.SelectedIndex + 1);
        HomePage.GestisciOpzioni(out s);
        MainView.MakeNotification($"{s}{MainView.Dictionary["RitornaallaHome"]}");
    }

}