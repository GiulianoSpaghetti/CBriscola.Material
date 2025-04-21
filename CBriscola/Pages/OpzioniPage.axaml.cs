using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Material.Styles.Controls;
using Metsys.Bson;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System;
using CBriscola.Views;
using System.Collections.Generic;
using System.IO;

namespace CBriscola.Pages;

public partial class OpzioniPage : UserControl
{

    public static OpzioniPage Instance { get; private set; }
    public OpzioniPage()
    {
        InitializeComponent();
        Instance = this;
        lsmazzi.Items.Clear();
        txtNomeUtente.Text = HomePage.Instance.o.NomeUtente;
        txtCpu.Text = HomePage.Instance.o.NomeCpu;
        tsCartaBriscola.IsChecked = HomePage.Instance.o.briscolaDaPunti;
        tsAvvisaTallone.IsChecked = HomePage.Instance.o.avvisaTalloneFinito;
        tsStessoSeme.IsChecked = HomePage.Instance.o.stessoSeme;
        List<ListBoxItem> mazzi;
        List<String> path;
        cbLivello.SelectedIndex = HomePage.Instance.o.livello-1;
        ListBoxItem item;
        String s1 = "";
        string dirs = System.IO.Path.Combine(App.Path, "Mazzi");

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
        Instance.lbCartaBriscola.Content = $"{MainWindow.d["BriscolaDaPunti"]}";
        Instance.lbAvvisaTallone.Content = $"{MainWindow.d["AvvisaTallone"]}";
        Instance.opNomeUtente.Content = $"{MainWindow.d["NomeUtente"]}: ";
        Instance.opNomeCpu.Content = $"{MainWindow.d["NomeCpu"]}: ";
        Instance.lbmazzi.Content = $"{MainWindow.d["Mazzo"]}";
        Instance.lbLivello.Content = $"{MainWindow.d["Livello"]}";
        Instance.lbStessoSeme.Content = $"{MainWindow.d["VarianteStessoSeme"]}";

    }
    public void OnOk_Click(Object source, RoutedEventArgs evt)
    {
        String s;
        HomePage.Instance.o.NomeUtente = txtNomeUtente.Text;
        HomePage.Instance.o.NomeCpu = txtCpu.Text;

        if (tsCartaBriscola.IsChecked == true)
            HomePage.Instance.o.briscolaDaPunti = true;
        else
            HomePage.Instance.o.briscolaDaPunti = false;
        if (tsAvvisaTallone.IsChecked == true)
            HomePage.Instance.o.avvisaTalloneFinito = true;
        else
            HomePage.Instance.o.avvisaTalloneFinito = false;
        if (tsStessoSeme.IsChecked == true)
            HomePage.Instance.o.stessoSeme = true;
        else
            HomePage.Instance.o.stessoSeme = false;
        ListBoxItem i = (ListBoxItem)lsmazzi.SelectedItem;
        if (i != null)
            HomePage.Instance.o.nomeMazzo=(string)i.Content;
        HomePage.Instance.o.livello = (UInt16) (cbLivello.SelectedIndex + 1);
        HomePage.GestisciOpzioni(out s);
        MainView.MakeNotification($"{s}{MainWindow.d["RitornaallaHome"]}");
    }
}