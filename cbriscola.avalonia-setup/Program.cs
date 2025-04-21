using System;
using WixSharp;

namespace cbriscola.avalonia_setup
{
    internal class Program
    {
        static void Main()
        {
            Project project = new Project("CBriscola.Avalonia",
                              new Dir(@"[ProgramFiles64Folder]\\CBriscola.Avalonia",
                                  new DirFiles(@"*.*"),
                                  new Dir("runtimes",
                                      new Dir("win-x64",
                                            new Dir("native",
                                                new File("runtimes\\win-x64\\native\\av_libglesv2.dll"),
                                                new File("runtimes\\win-x64\\native\\libHarfBuzzSharp.dll"),
                                                new File("runtimes\\win-x64\\native\\libSkiaSharp.dll")
                                            )
                                        )
                                   )
                        ),
                        new Dir(@"%ProgramMenu%",
                         new ExeFileShortcut("CBriscola.Avalonia", "[ProgramFiles64Folder]\\CBriscola.Avalonia\\CBriscola.Desktop.exe", "") { WorkingDirectory = "[INSTALLDIR]" }
                      )//,
                       //new Property("ALLUSERS","0")
            );

            project.GUID = new Guid("68B61DE0-07A0-499E-B3FB-F15873641EB4");
            project.Version = new Version("1.1.1.0");
            project.Platform = Platform.x64;
            project.SourceBaseDir = "F:\\source\\CBriscola.Material\\CBriscola.Desktop\\bin\\Release\\net9.0-windows10.0.26100.0";
            project.LicenceFile = "LICENSE.rtf";
            project.OutDir = "f:\\";
            project.ControlPanelInfo.Manufacturer = "Giulio Sorrentino";
            project.ControlPanelInfo.Name = "CBriscola.Material";
            project.ControlPanelInfo.HelpLink = "https://github.com/GiulianoSpaghetti/CBriscola.Material/issues";
            project.Description = "Simulatore del gioco della briscola in avalonia a due giocatori senza multiplayer in dialetto Material di Google di avalonia";
            //            project.Properties.SetValue("ALLUSERS", 0);
            project.BuildMsi();
        }
    }
}
