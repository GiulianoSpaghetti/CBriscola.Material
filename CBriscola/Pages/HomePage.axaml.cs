using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CBriscola.Views;
using Material.Styles.Controls;
using Metsys.Bson;
using org.altervista.numerone.framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

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
    private static readonly string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CBriscola.Avalonia");
    private static ILauncher? launcher = null;
    private ElaboratoreCarteBriscola e;
    private Stream asset;
    public Opzioni o;
    private static org.altervista.numerone.framework.briscola.CartaHelper cartaHelper;


    public static HomePage Instance;
    public HomePage()
    {
        InitializeComponent();
        Instance = this;
        o = LeggiOpzioni();
        briscolaDaPunti = o.briscolaDaPunti;

        e = new ElaboratoreCarteBriscola(briscolaDaPunti);
        m = new Mazzo(e);


        m.SetNome(o.nomeMazzo);        
    }

    public static void Traduci()
    {
        if (!Carta.Inizializza(App.path, m, 40, cartaHelper = new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()), MainWindow.d["bastoni"] as string, MainWindow.d["coppe"] as string, MainWindow.d["denari"] as string, MainWindow.d["spade"] as string, MainWindow.d["Fiori"] as string, MainWindow.d["Quadri"] as string, MainWindow.d["Cuori"] as string, MainWindow.d["Picche"] as string, "CBriscola"))
        {
            try
            {
                MainView.MakeNotification($"{MainWindow.d["MazzoNonTrovatoTesto"]}");
            }
            catch (InvalidOperationException ex)
            {
                m.SetNome("Napoletano");
                Carta.Inizializza(App.path, m, 40, cartaHelper = new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()), MainWindow.d["bastoni"] as string, MainWindow.d["coppe"] as string, MainWindow.d["denari"] as string, MainWindow.d["spade"] as string, MainWindow.d["Fiori"] as string, MainWindow.d["Quadri"] as string, MainWindow.d["Cuori"] as string, MainWindow.d["Picche"] as string, "CBriscola");
            }
     
        }
        if (Instance.o.nomeMazzo == "Napoletano")
        {
            Instance.asset = AssetLoader.Open(new Uri($"avares://CBriscola/Assets/retro_carte_pc.png"));
            cartaCpu.Source = new Bitmap(Instance.asset);

        }
        else
            try
            {
                cartaCpu.Source = new Bitmap(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(App.path, "Mazzi"), m.GetNome()), "retro carte pc.png"));
            }
            catch (Exception ex)
            {
                Instance.asset = AssetLoader.Open(new Uri($"avares://CBriscola/Assets/retro_carte_pc.png"));
                cartaCpu.Source = new Bitmap(Instance.asset);
            }
        g = new Giocatore(new GiocatoreHelperUtente(), Instance.o.NomeUtente, 3);
        switch (Instance.o.livello)
        {
            case 1: helper = new GiocatoreHelperCpu0(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
            case 2: helper = new GiocatoreHelperCpu1(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
            default: helper = new GiocatoreHelperCpu2(ElaboratoreCarteBriscola.GetCartaBriscola()); break;

        }
        cpu = new Giocatore(helper, Instance.o.NomeCpu, 3);
        avvisaTalloneFinito = Instance.o.avvisaTalloneFinito;
        stessoSeme = Instance.o.stessoSeme;
        primo = g;

        secondo = cpu;
        briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
        Image[] img = new Image[3];
        for (UInt16 i = 0; i < 3; i++)
        {
            g.AddCarta(m);
            cpu.AddCarta(m);

        }
        Instance.NomeUtente.Content = g.GetNome();
        Instance.NomeCpu.Content = cpu.GetNome();
        Instance.Utente0.Source = g.GetImmagine(0);


        Instance.Utente1.Source = g.GetImmagine(1);
        Instance.Utente2.Source = g.GetImmagine(2);
        Instance.Cpu0.Source = cartaCpu.Source;
        Instance.Cpu1.Source = cartaCpu.Source;
        Instance.Cpu2.Source = cartaCpu.Source;
        Instance.Briscola.Source = briscola.GetImmagine();

        Instance.PuntiCpu.Content = $"{MainWindow.d["PuntiDiPrefisso"]} {cpu.GetNome()} {MainWindow.d["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
        Instance.PuntiUtente.Content = $"{MainWindow.d["PuntiDiPrefisso"]} {g.GetNome()} {MainWindow.d["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
        Instance.NelMazzoRimangono.Content = $"{MainWindow.d["NelMazzoRimangono"]} {m.GetNumeroCarte()} {MainWindow.d["carte"]}";
        Instance.CartaBriscola.Content = $"{MainWindow.d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
        Instance.fpOk.Content = $"{MainWindow.d["Si"]}";
        Instance.fpCancel.Content = $"{MainWindow.d["No"]}";
        Instance.fpShare.Content = $"{MainWindow.d["Condividi"]}";
        Instance.btnGiocata.Content = $"{MainWindow.d["giocataVista"]}";

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
            MainView.MakeNotification(MainWindow.d["MossaNonConsentitaTesto"] as string);
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

        PuntiCpu.Content = $"{MainWindow.d["PuntiDiPrefisso"]} {cpu.GetNome()} {MainWindow.d["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
        PuntiUtente.Content = $"{MainWindow.d["PuntiDiPrefisso"]} {g.GetNome()} {MainWindow.d["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
        if (AggiungiCarte())
        {

            NelMazzoRimangono.Content = $"{MainWindow.d["NelMazzoRimangono"]} {m.GetNumeroCarte()} {MainWindow.d["carte"]}";
            CartaBriscola.Content = $"{MainWindow.d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
            if (Briscola.IsVisible)
            {
                switch (m.GetNumeroCarte())
                {
                    case 2:
                        if (avvisaTalloneFinito)
                        {
                            MainView.MakeNotification($"{MainWindow.d["TalloneFinito"]}");
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
                    MainView.MakeNotification($"{MainWindow.d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {MainWindow.d["di"]} {MainWindow.d["Briscola"]}");

                }
                else if (cpu.GetCartaGiocata().GetPunteggio() > 0)
                {
                    MainView.MakeNotification($"{MainWindow.d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {MainWindow.d["di"]} {cpu.GetCartaGiocata().GetSemeStr()}");
                }
            }
        }
        else
        {
            String s = "";
            puntiUtente += g.GetPunteggio();
            puntiCpu += cpu.GetPunteggio();
            if (puntiUtente == puntiCpu)
                s = $"{MainWindow.d["PartitaPatta"]}";
            else
            {
                if (puntiUtente > puntiCpu)
                    s = $"{MainWindow.d["HaiVinto"]}";
                else
                    s = $"{MainWindow.d["HaiPerso"]}";
                s = $"{s} {MainWindow.d["per"]} {Math.Abs(puntiUtente - puntiCpu)} {MainWindow.d["punti"]}";
            }
            if (partite % 2 == 1)
            {
                fpRisultrato.Content = $"{MainWindow.d["PartitaFinita"]}. {s}. {MainWindow.d["NuovaPartita"]}?";
                fpShare.IsVisible = true;
                fpShare.IsEnabled = true;
            }
            else
            {
                fpRisultrato.Content = $"{MainWindow.d["PartitaFinita"]}. {s}. {MainWindow.d["EffettuaSecondaPartita"]}?";
                fpShare.IsVisible = false;
            }
            Applicazione.IsVisible = false;
            FinePartita.IsVisible = true;
            fpShare.IsEnabled = helper.GetLivello() == 3;
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
       MainWindow.Instance.Close();
    }

    private void OnFPShare_Click(object sender, RoutedEventArgs e)
    {
        string s = "";
        if (stessoSeme)
            s = "bussata%20";

        if (launcher == null)
            launcher = TopLevel.GetTopLevel(this).Launcher;
        launcher.LaunchUriAsync(new Uri($"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola.avalonia%20{s}la%20partita%20numero%20{partite}%20{g.GetNome()}%20contro%20{cpu.GetNome()}%20%C3%A8%20finita%20{puntiUtente}%20a%20{puntiCpu}%20col%20mazzo%20{m.GetNome()}%20su%20piattaforma%20{App.SistemaOperativo}&url=https%3A%2F%2Fgithub.com%2Fnumerunix%2Fcbriscola.Avalonia"));
        fpShare.IsEnabled = false;
    }
    private Opzioni CaricaOpzioni()
    {
        Opzioni o;

        o = LeggiOpzioni();
        return o;
    }
    private Opzioni LeggiOpzioni()
    {
        Opzioni o;
        if (!System.IO.Path.Exists(folder))
            Directory.CreateDirectory(folder);
        StreamReader file = null;
        try
        {
            file = new StreamReader(System.IO.Path.Combine(folder, "opzioni.json"));
            try
            {
                o = Newtonsoft.Json.JsonConvert.DeserializeObject<Opzioni>(file.ReadToEnd());
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                o = null;
                file.Close();
            }
            catch (Newtonsoft.Json.JsonSerializationException ex1)
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
            o.NomeUtente = "numerone";
            o.NomeCpu = "numerona";
            o.briscolaDaPunti = false;
            o.avvisaTalloneFinito = true;
            o.nomeMazzo = "Napoletano";
            o.livello = 3;
            o.stessoSeme = false;
            SalvaOpzioni(o);
            return o;
        }
        file.Close();
        return o;
    }

    private void SalvaOpzioni(Opzioni o)
    {
        StreamWriter w = new StreamWriter($"{System.IO.Path.Combine(folder, "opzioni.json")}");
        w.Write(Newtonsoft.Json.JsonConvert.SerializeObject(o));
        w.Close();
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
            g.Gioca(quale, primo, o.stessoSeme);

        Giocata0.IsVisible = true;
        Giocata0.Source = img1.Source;
        img1.IsVisible = false;
        return img1;
    }
    private void NuovaPartita(bool vecchioStessoSeme, out String s)
    {
        s = "";
        if (o.livello != helper.GetLivello())
        {
            s += $"{MainWindow.d["PartitaRiavviata"]}\r\n";

            puntiCpu = puntiUtente = 0;
            partite = 0;
        }
        if (stessoSeme != vecchioStessoSeme)
        {
            if (vecchioStessoSeme)
            {
                s += $"{MainWindow.d["VarianteBussataTesto"]}\r\n";
            }
            else
            {
                s+=$"{MainWindow.d["VarianteNormaleTesto"]}\r\n";

            }
            puntiCpu = puntiUtente = 0;
            partite = 0;
            stessoSeme = Instance.o.stessoSeme;
        }
        if (partite % 2 == 0)
            puntiUtente = puntiCpu = 0;
        e = new ElaboratoreCarteBriscola(o.briscolaDaPunti);
        m = new Mazzo(e);
        Carta.SetHelper(cartaHelper = new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()));
        m.SetNome(o.nomeMazzo);
        briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
        switch (o.livello)
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
        PuntiCpu.Content = $"{MainWindow.d["PuntiDiPrefisso"]} {cpu.GetNome()} {MainWindow.d["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
        PuntiUtente.Content = $"{MainWindow.d["PuntiDiPrefisso"]} {g.GetNome()} {MainWindow.d["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
        NelMazzoRimangono.Content = $"{MainWindow.d["NelMazzoRimangono"]} {m.GetNumeroCarte()} {MainWindow.d["carte"]}";
        NelMazzoRimangono.IsVisible = true;
        CartaBriscola.Content = $"{MainWindow.d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
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
            cpu.Gioca(0);
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
        g.SetNome(Instance.o.NomeUtente);
        cpu.SetNome(Instance.o.NomeCpu);
        Instance.NomeUtente.Content = g.GetNome();
        Instance.NomeCpu.Content = cpu.GetNome();
        if (Instance.o.nomeMazzo != m.GetNome())
        { 
            m.SetNome(Instance.o.nomeMazzo);
            if (!Carta.CaricaImmagini(App.path, m, 40, MainWindow.d["bastoni"] as string, MainWindow.d["coppe"] as string, MainWindow.d["denari"] as string, MainWindow.d["spade"] as string, MainWindow.d["Fiori"] as string, MainWindow.d["Quadri"] as string, MainWindow.d["Cuori"] as string, MainWindow.d["Picche"] as string, "CBriscola")
)
            {
                s1=$"{MainWindow.d["MazzoNonTrovatoTesto"]}\r\n";
                Instance.o.nomeMazzo=m.GetNome();
 
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
                    cartaCpu.Source = new Bitmap(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(App.path, "Mazzi"), m.GetNome()), "retro carte pc.png"));
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
            Instance.CartaBriscola.Content = $"{MainWindow.d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
        }
        Instance.SalvaOpzioni(Instance.o);
        if (Instance.o.livello != helper.GetLivello() || stessoSeme != Instance.o.stessoSeme)
        {
            Instance.NuovaPartita(Instance.o.stessoSeme, out s);
        }
        s = s1 + s;
    }

}