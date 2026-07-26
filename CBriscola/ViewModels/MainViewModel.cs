using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using CBriscola.ViewModels;
using org.altervista.numerone.framework;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CBriscola.ViewModels;

public class MainViewModel : ViewModelBase
{
    internal static MainViewModel Instance { get; private set; } = null;
    private static readonly string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CBriscola.Avalonia");
    internal Opzioni o;
    private string _nomeUtente;
    private string _nomeCpu;
    private string _nomeMazzo;
    private bool _briscolaDaPunti;
    private bool _avvisaTalloneFinito;
    private ushort _livello;
    private MyCarta _selectedCarta;
    public MyCarta SelectedCarta
    {
        get => _selectedCarta;
        set => this.RaiseAndSetIfChanged(ref _selectedCarta, value);
    }
    private Random r = new Random();

    private bool _stessoSeme;
    public String NomeUtente { get => _nomeUtente; set { o.NomeUtente = value; this.RaiseAndSetIfChanged(ref _nomeUtente, value); } }
    public String NomeCpu { get => _nomeCpu; set { o.NomeCpu = value; this.RaiseAndSetIfChanged(ref _nomeCpu, value); } }
    public String NomeMazzo { get => _nomeMazzo; set { o.nomeMazzo = value; this.RaiseAndSetIfChanged(ref _nomeMazzo, value); } }
    public bool BriscolaDaPunti { get => _briscolaDaPunti; set { o.briscolaDaPunti = value; this.RaiseAndSetIfChanged(ref _briscolaDaPunti, value); } }
    public bool AvvisaTalloneFinito { get => _avvisaTalloneFinito; set { o.avvisaTalloneFinito = value; this.RaiseAndSetIfChanged(ref _avvisaTalloneFinito, value); } }

    public ushort Livello { get => _livello; set { o.livello = value; this.RaiseAndSetIfChanged(ref _livello, value); } }
    public bool StessoSeme { get => _stessoSeme; set { o.stessoSeme = value; this.RaiseAndSetIfChanged(ref _stessoSeme, value); } }
    public ObservableCollection<MyCarta> Mazzi { get; } = new ObservableCollection<MyCarta>();
    public MainViewModel()
    {
        o = new Opzioni();
        LeggiOpzioni();
   }

    public static MainViewModel GetMainViewModelInstance()
    {
        if (Instance == null)
        {
            Instance = new MainViewModel();
        }   
        return Instance;
    }
    internal void LeggiOpzioni()
    {
        if (!System.IO.Path.Exists(folder))
            Directory.CreateDirectory(folder);
        StreamReader file = null;
        try
        {
            file = new StreamReader(System.IO.Path.Combine(folder, "opzioni.json"));
            try
            {
                o = JsonSerializer.Deserialize<Opzioni>(file.ReadToEnd());
            }
            catch (JsonException ex)
            {
                o = null;
                file.Close();
            }
            catch (NotSupportedException ex1)
            {
                o = null;
                file.Close();
            }
            catch (ArgumentNullException ex2)
            {
                o = null;
                file.Close();
            }
            if (o == null)
                throw new FileNotFoundException();

        }
        catch (FileNotFoundException ex)
        {
            o = new Opzioni();
            NomeUtente = "numerone";
            NomeCpu = "numerona";
            BriscolaDaPunti = false;
            AvvisaTalloneFinito = true;
            NomeMazzo = "Napoletano";
            Livello = 3;
            StessoSeme = false;
            SalvaOpzioni();
            return;
        }
        file.Close();
        NomeUtente = o.NomeUtente;
        NomeCpu = o.NomeCpu;
        BriscolaDaPunti = o.briscolaDaPunti;
        AvvisaTalloneFinito = o.avvisaTalloneFinito;
        NomeMazzo = o.nomeMazzo;
        Livello = o.livello;
        StessoSeme = o.stessoSeme;
    }
    internal void SalvaOpzioni()
    {
        StreamWriter w = new StreamWriter($"{System.IO.Path.Combine(folder, "opzioni.json")}");
        w.Write(JsonSerializer.Serialize(o));
        w.Close();
    }

    public void CaricaMazzi(String dirs) {
        List<String> path;
        try
        {
            path = Directory.EnumerateDirectories(dirs).ToList();
        }
        catch (System.IO.DirectoryNotFoundException ex)
        {
            path = new List<string>();
        }
        for (UInt16 i = 0; i < path.Count; i++)
            path[i] = path[i].Substring(path[i].LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
        if (!path.Contains("Napoletano"))
            path.Add("Napoletano");
        path.Sort();
        foreach (String s in path)
        {
            Mazzi.Add(new MyCarta(r.Next(0, 39), s, dirs));
        }
    }
}
