using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CBriscola.ViewModels;
using CBriscola.Views;
using Material.Styles.Controls;
using Metsys.Bson;
using org.altervista.numerone.framework;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;

namespace CBriscola.Pages;

public partial class HomePage : UserControl
{
    private static Giocatore g, cpu, primo, secondo, temp;
    private static Mazzo m;
    private static Carta c, c1, briscola;
    private static Image cartaCpu = new Image();
    private static Image i, i1;
    private static UInt16 puntiUtente = 0, puntiCpu = 0;
    private static UInt128 partite = 0;
    private static bool avvisaTalloneFinito = true, briscolaDaPunti = false, primaUtente = true, stessoSeme = false;
    private static GiocatoreHelperCpu helper;

    private static ILauncher? launcher = null;
    private ElaboratoreCarteBriscola e;
    private Stream asset;

    private static org.altervista.numerone.framework.briscola.CartaHelper cartaHelper;


    internal static HomePage Instance { get; private set; }
    public HomePage()
    {
        DataContext ??= MainViewModel.GetMainViewModelInstance();
        InitializeComponent();
        Instance = this;
        briscolaDaPunti = ((MainViewModel) DataContext).BriscolaDaPunti;

        e = new ElaboratoreCarteBriscola(briscolaDaPunti);
        m = new Mazzo(e);


        m.SetNome(((MainViewModel)DataContext).NomeMazzo);        
    }

    public static void Traduci()
    {
        if (!Carta.Inizializza(App.Path, m, 40, cartaHelper = new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()), MainView.Dictionary["bastoni"] as string, MainView.Dictionary["coppe"] as string, MainView.Dictionary["denari"] as string, MainView.Dictionary["spade"] as string, MainView.Dictionary["Fiori"] as string, MainView.Dictionary["Quadri"] as string, MainView.Dictionary["Cuori"] as string, MainView.Dictionary["Picche"] as string, "CBriscola"))
        {
            try
            {
                MainView.MakeNotification($"{MainView.Dictionary["MazzoNonTrovatoTesto"]}");
            }
            catch (InvalidOperationException ex)
            {
                m.SetNome("Napoletano");
                Carta.Inizializza(App.Path, m, 40, cartaHelper = new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()), MainView.Dictionary["bastoni"] as string, MainView.Dictionary["coppe"] as string, MainView.Dictionary["denari"] as string, MainView.Dictionary["spade"] as string, MainView.Dictionary["Fiori"] as string, MainView.Dictionary["Quadri"] as string, MainView.Dictionary["Cuori"] as string, MainView.Dictionary["Picche"] as string, "CBriscola");
            }

        }

        if (MainViewModel.GetMainViewModelInstance().NomeMazzo == "Napoletano")
        {
            Instance.asset = AssetLoader.Open(new Uri($"avares://CBriscola/Assets/retro_carte_pc.png"));
            cartaCpu.Source = new Bitmap(Instance.asset);

        }
        else
            try
            {
                cartaCpu.Source = new Bitmap(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(App.Path, "Mazzi"), m.GetNome()), "retro carte pc.png"));
            }
            catch (Exception ex)
            {
                Instance.asset = AssetLoader.Open(new Uri($"avares://CBriscola/Assets/retro_carte_pc.png"));
                cartaCpu.Source = new Bitmap(Instance.asset);
            }
        g = new Giocatore(new GiocatoreHelperUtente(), MainViewModel.GetMainViewModelInstance().NomeUtente, 3);
        switch (MainViewModel.GetMainViewModelInstance().Livello)
        {
            case 1: helper = new GiocatoreHelperCpu0(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
            case 2: helper = new GiocatoreHelperCpu1(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
            default: helper = new GiocatoreHelperCpu2(ElaboratoreCarteBriscola.GetCartaBriscola()); break;

        }
        cpu = new Giocatore(helper, MainViewModel.GetMainViewModelInstance().NomeCpu, 3);
        avvisaTalloneFinito = MainViewModel.GetMainViewModelInstance().AvvisaTalloneFinito;
        stessoSeme = MainViewModel.GetMainViewModelInstance().StessoSeme;
        primo = g;

        secondo = cpu;
        briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
        Image[] img = new Image[3];
        for (UInt16 i = 0; i < 3; i++)
        {
            g.AddCarta(m);
            cpu.AddCarta(m);

        }
        Instance.NomeUtente.Text = g.GetNome();
        Instance.NomeCpu.Text = cpu.GetNome();
        Instance.Utente0.Source = g.GetImmagine(0);


        Instance.Utente1.Source = g.GetImmagine(1);
        Instance.Utente2.Source = g.GetImmagine(2);
        Instance.Cpu0.Source = cartaCpu.Source;
        Instance.Cpu1.Source = cartaCpu.Source;
        Instance.Cpu2.Source = cartaCpu.Source;
        Instance.Briscola.Source = briscola.GetImmagine();

        Instance.PuntiCpu.Text = $"{MainView.Dictionary["PuntiDiPrefisso"]} {cpu.GetNome()} {MainView.Dictionary["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
        Instance.PuntiUtente.Text = $"{MainView.Dictionary["PuntiDiPrefisso"]} {g.GetNome()} {MainView.Dictionary["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
        Instance.NelMazzoRimangono.Text = $"{MainView.Dictionary["NelMazzoRimangono"]} {m.GetNumeroCarte()} {MainView.Dictionary["carte"]}";
        Instance.CartaBriscola.Text = $"{MainView.Dictionary["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
        Instance.fpOk.Content = $"{MainView.Dictionary["Si"]}";
        Instance.fpCancel.Content = $"{MainView.Dictionary["No"]}";
        Instance.fpShare.Content = $"{MainView.Dictionary["Condividi"]}";
        Instance.btnGiocata.Content = $"{MainView.Dictionary["giocataVista"]}";

    }
    private void Image_Tapped(object Sender, RoutedEventArgs arg)
    {
        if (btnGiocata.IsVisible)
            return;
        Image img = (Image)((Button)Sender).Content;
        try
        {
            i = GiocaUtente(img);

        }
        catch (Exception ex)
        {
            MainView.MakeNotification(MainView.Dictionary["MossaNonConsentitaTesto"] as string);
            return;
        }
        if (secondo == cpu)
            i1 = GiocaCpu();
        btnGiocata.IsVisible = true;
    }
    private void Gioca_Click(object sender, RoutedEventArgs e)
    {
        c = primo.GetCartaGiocata();
        c1 = secondo.GetCartaGiocata();

        if ((c.CompareTo(c1) > 0 && c.StessoSeme(c1)) || (c1.StessoSeme(briscola) && !c.StessoSeme(briscola)))
        {
            temp = secondo;
            secondo = primo;
            primo = temp;
        }

        primo.AggiornaPunteggio(secondo);

        PuntiCpu.Text = $"{MainView.Dictionary["PuntiDiPrefisso"]} {cpu.GetNome()} {MainView.Dictionary["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
        PuntiUtente.Text = $"{MainView.Dictionary["PuntiDiPrefisso"]} {g.GetNome()} {MainView.Dictionary["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
        if (AggiungiCarte())
        {

            NelMazzoRimangono.Text = $"{MainView.Dictionary["NelMazzoRimangono"]} {m.GetNumeroCarte()} {MainView.Dictionary["carte"]}";
            CartaBriscola.Text = $"{MainView.Dictionary["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
            if (Briscola.IsVisible)
            {
                switch (m.GetNumeroCarte())
                {
                    case 2:
                        if (avvisaTalloneFinito)
                        {
                            MainView.MakeNotification($"{MainView.Dictionary["TalloneFinito"]}");
                        }
                        break;
                    case 0:
                        NelMazzoRimangono.IsVisible = false;
                        Briscola.IsVisible = false;
                        break;

                }
            }
            Utente0.Source = g.GetImmagine(0);
            if (cpu.GetNumeroCarte() > 1)
                Utente1.Source = g.GetImmagine(1);
            if (cpu.GetNumeroCarte() > 2)
                Utente2.Source = g.GetImmagine(2);
            i.IsVisible = true;
            i1.IsVisible = true;
            Giocata0.IsVisible = false;
            Giocata1.IsVisible = false;
            switch (cpu.GetNumeroCarte())
            {
                case 2:
                    Utente2.IsVisible = false;
                    Cpu2.IsVisible = false;
                    break;
                case 1:
                    Utente1.IsVisible = false;
                    Cpu1.IsVisible = false;
                    break;
            }
            if (primo == cpu)
            {
                i1 = GiocaCpu();
                if (cpu.GetCartaGiocata().StessoSeme(briscola))
                {
                    MainView.MakeNotification($"{MainView.Dictionary["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {MainView.Dictionary["di"]} {MainView.Dictionary["Briscola"]}");

                }
                else if (cpu.GetCartaGiocata().GetPunteggio() > 0)
                {
                    MainView.MakeNotification($"{MainView.Dictionary["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {MainView.Dictionary["di"]} {cpu.GetCartaGiocata().GetSemeStr()}");
                }
            }
        }
        else
        {
            String s = "";
            puntiUtente += g.GetPunteggio();
            puntiCpu += cpu.GetPunteggio();
            if (puntiUtente == puntiCpu)
                s = $"{MainView.Dictionary["PartitaPatta"]}";
            else
            {
                if (puntiUtente > puntiCpu)
                    s = $"{MainView.Dictionary["HaiVinto"]}";
                else
                    s = $"{MainView.Dictionary["HaiPerso"]}";
                s = $"{s} {MainView.Dictionary["per"]} {Math.Abs(puntiUtente - puntiCpu)} {MainView.Dictionary["punti"]}";
            }
            if (partite % 2 == 1)
            {
                fpRisultrato.Text = $"{MainView.Dictionary["PartitaFinita"]}. {s}. {MainView.Dictionary["NuovaPartita"]}?";
                fpShare.IsVisible = true;
                fpShare.IsEnabled = true;
            }
            else
            {
                fpRisultrato.Text = $"{MainView.Dictionary["PartitaFinita"]}. {s}. {MainView.Dictionary["EffettuaSecondaPartita"]}?";
                fpShare.IsVisible = false;
            }
            Applicazione.IsVisible = false;
            FinePartita.IsVisible = true;
            fpShare.IsEnabled = helper.GetLivello() == 3;
            fpCancel.IsEnabled = true;
            partite++;
        }
        btnGiocata.IsVisible = false;
    }
    private void OnOkFp_Click(object sender, RoutedEventArgs evt)
    {
        String s="";
        FinePartita.IsVisible = false;
        NuovaPartita(stessoSeme, out s);
        Applicazione.IsVisible = true;


    }
    private void OnCancelFp_Click(object sender, RoutedEventArgs e)
    {
        if (App.PuoChiudersi)
            MainWindow.Instance.Close();
        else
        {
            fpRisultrato.Text = $"{MainView.Dictionary["ChiudiApp"]}";
            fpCancel.IsEnabled = false;
        }
    }

    private void OnFPShare_Click(object sender, RoutedEventArgs e)
    {
        string s = "";
        if (stessoSeme)
            s = "bussata%20";

        if (launcher == null)
            launcher = TopLevel.GetTopLevel(this).Launcher;
        launcher.LaunchUriAsync(new Uri($"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola.avalonia%20{s}la%20partita%20numero%20{partite}%20{g.GetNome()}%20contro%20{cpu.GetNome()}%20%C3%A8%20finita%20{puntiUtente}%20a%20{puntiCpu}%20col%20mazzo%20{m.GetNome()}%20su%20piattaforma%20{App.SistemaOperativo}%26url=https%3A%2F%2Fopencode.net%2Fnumerone%2FCBriscola-Material"));
        fpShare.IsEnabled = false;
    }





    private Image GiocaUtente(Image img)
    {
        UInt16 quale = 0;

        Image img1 = Utente0;
        if (img == Utente1)
        {
            quale = 1;
            img1 = Utente1;
        }
        if (img == Utente2)
        {

            quale = 2;
            img1 = Utente2;
        }
        if (primo == g)
            g.Gioca(quale);
        else
            g.Gioca(quale, primo, ((MainViewModel) DataContext).StessoSeme);

        Giocata0.IsVisible = true;
        Giocata0.Source = img1.Source;
        img1.IsVisible = false;
        return img1;
    }
    private void NuovaPartita(bool vecchioStessoSeme, out String s)
    {
        s = "";
        if (((MainViewModel) DataContext).Livello == helper.GetLivello())
        {
            s += $"{MainView.Dictionary["PartitaRiavviata"]}\r\n";

            puntiCpu = puntiUtente = 0;
            partite = 0;
        }
        if (stessoSeme != vecchioStessoSeme)
        {
            if (vecchioStessoSeme)
            {
                s += $"{MainView.Dictionary["VarianteBussataTesto"]}\r\n";
            }
            else
            {
                s+=$"{MainView.Dictionary["VarianteNormaleTesto"]}\r\n";

            }
            puntiCpu = puntiUtente = 0;
            partite = 0;
            stessoSeme = ((MainViewModel) DataContext).StessoSeme;
        }
        if (partite % 2 == 0)
            puntiUtente = puntiCpu = 0;
        e = new ElaboratoreCarteBriscola(((MainViewModel) DataContext).BriscolaDaPunti);
        m = new Mazzo(e);
        Carta.SetHelper(cartaHelper = new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()), m);
        m.SetNome(((MainViewModel) DataContext).NomeMazzo);
        briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
        switch (((MainViewModel) DataContext).Livello)
        {
            case 1: helper = new GiocatoreHelperCpu0(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
            case 2: helper = new GiocatoreHelperCpu1(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
            default: helper = new GiocatoreHelperCpu2(ElaboratoreCarteBriscola.GetCartaBriscola()); break;

        }
        cpu = new Giocatore(helper, cpu.GetNome(), 3);
        g = new Giocatore(new GiocatoreHelperUtente(), g.GetNome(), 3);
        for (UInt16 i = 0; i < 3; i++)
        {
            g.AddCarta(m);
            cpu.AddCarta(m);

        }
        Utente0.Source = g.GetImmagine(0);
        Utente0.IsVisible = true;
        Utente1.Source = g.GetImmagine(1);
        Utente1.IsVisible = true;
        Utente2.Source = g.GetImmagine(2);
        Utente2.IsVisible = true;
        Cpu0.Source = cartaCpu.Source;
        Cpu0.IsVisible = true;
        Cpu1.Source = cartaCpu.Source;
        Cpu1.IsVisible = true;
        Cpu2.Source = cartaCpu.Source;
        Cpu2.IsVisible = true;
        Giocata0.IsVisible = false;
        Giocata1.IsVisible = false;
        PuntiCpu.Text = $"{MainView.Dictionary["PuntiDiPrefisso"]} {cpu.GetNome()} {MainView.Dictionary["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
        PuntiUtente.Text = $"{MainView.Dictionary["PuntiDiPrefisso"]} {g.GetNome()} {MainView.Dictionary["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
        NelMazzoRimangono.Text = $"{MainView.Dictionary["NelMazzoRimangono"]} {m.GetNumeroCarte()} {MainView.Dictionary["carte"]}";
        NelMazzoRimangono.IsVisible = true;
        CartaBriscola.Text = $"{MainView.Dictionary["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
        CartaBriscola.IsVisible = true;
        Briscola.Source = briscola.GetImmagine();
        Briscola.IsVisible = true;
        primaUtente = !primaUtente;
        btnGiocata.IsVisible = false;
        if (primaUtente)
        {
            primo = g;
            secondo = cpu;
        }
        else
        {

            primo = cpu;
            secondo = g;
            i1 = GiocaCpu();
        }
        Briscola.Source = briscola.GetImmagine();

    }


    private Image GiocaCpu()

    {
        UInt16 quale = 0;
        Image img1 = Cpu0;
        if (primo == cpu)
            cpu.Gioca(0, stessoSeme);
        else
            cpu.Gioca(0, g, stessoSeme);
        quale = cpu.GetICartaGiocata();
        if (quale == 1)
            img1 = Cpu1;
        if (quale == 2)
            img1 = Cpu2;
        Giocata1.IsVisible = true;
        Giocata1.Source = cpu.GetCartaGiocata().GetImmagine();
        img1.IsVisible = false;
        return img1;
    }
    private static bool AggiungiCarte()
    {
        try
        {
            primo.AddCarta(m);
            secondo.AddCarta(m);
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
        return true;
    }

    public static void GestisciOpzioni(out string s)
    {
        s = "";
        string s1 = "";
        g.SetNome(MainViewModel.GetMainViewModelInstance().NomeUtente);
        cpu.SetNome(MainViewModel.GetMainViewModelInstance().NomeCpu);
        Instance.NomeUtente.Text = g.GetNome();
        Instance.NomeCpu.Text = cpu.GetNome();
        if (MainViewModel.GetMainViewModelInstance().NomeMazzo != m.GetNome())
        { 
            m.SetNome(MainViewModel.GetMainViewModelInstance().NomeMazzo);
            if (!Carta.CaricaImmagini(App.Path, m, 40, MainView.Dictionary["bastoni"] as string, MainView.Dictionary["coppe"] as string, MainView.Dictionary["denari"] as string, MainView.Dictionary["spade"] as string, MainView.Dictionary["Fiori"] as string, MainView.Dictionary["Quadri"] as string, MainView.Dictionary["Cuori"] as string, MainView.Dictionary["Picche"] as string, "CBriscola")
)
            {
                s1=$"{MainView.Dictionary["MazzoNonTrovatoTesto"]}\r\n";
                MainViewModel.GetMainViewModelInstance().NomeMazzo =m.GetNome();
 
            }
            Instance.Utente0.Source = g.GetImmagine(0);
            Instance.Utente1.Source = g.GetImmagine(1);
            Instance.Utente2.Source = g.GetImmagine(2);
            try
            {
                Instance.Giocata0.Source = g.GetCartaGiocata().GetImmagine();
            }
            catch (System.IndexOutOfRangeException ex) {; }
            try
            {
                Instance.Giocata1.Source = cpu.GetCartaGiocata().GetImmagine();
            }
            catch (System.IndexOutOfRangeException ex) {; }

            briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
            Instance.Briscola.Source = briscola.GetImmagine();
            if (m.GetNome() != "Napoletano")
                try
                {
                    cartaCpu.Source = new Bitmap(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(App.Path, "Mazzi"), m.GetNome()), "retro carte pc.png"));
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Instance.asset = AssetLoader.Open(new Uri($"avares://CBriscola/Assets/retro_carte_pc.png"));
                    cartaCpu.Source = new Bitmap(Instance.asset);

                }
            else
            {
                Instance.asset = AssetLoader.Open(new Uri($"avares://CBriscola/Assets/retro_carte_pc.png"));
                cartaCpu.Source = new Bitmap(Instance.asset);
            }
            Instance.Cpu0.Source = cartaCpu.Source;
            Instance.Cpu1.Source = cartaCpu.Source;
            Instance.Cpu2.Source = cartaCpu.Source;
            Instance.CartaBriscola.Text = $"{MainView.Dictionary["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
        }
        MainViewModel.GetMainViewModelInstance().SalvaOpzioni();
        if (MainViewModel.GetMainViewModelInstance().Livello != helper.GetLivello() || stessoSeme != MainViewModel.GetMainViewModelInstance().StessoSeme)
        {
            Instance.NuovaPartita(MainViewModel.GetMainViewModelInstance().StessoSeme, out s);
        }
        s = s1 + s;
    }
}
