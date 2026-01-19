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
                                      new Dir("win-arm64",
                                            new Dir("native",
                                                new File("runtimes\\win-arm64\\native\\av_libglesv2.dll"),
                                                new File("runtimes\\win-arm64\\native\\libHarfBuzzSharp.dll"),
                                                new File("runtimes\\win-arm64\\native\\libSkiaSharp.dll")
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
            project.Version = new Version("2.0");
            project.Platform = Platform.arm64;
            project.SourceBaseDir = "E:\\10\\CBriscola\\CBriscola.Desktop\\bin\\Release\\net10.0-windows10.0.26100.0";
            project.LicenceFile = "LICENSE.rtf";
            project.OutDir = "C:\\Users\\numer\\Downloads";
            project.ControlPanelInfo.Manufacturer = "Giulio Sorrentino";
            project.ControlPanelInfo.Name = "CBriscola.Material";
            project.ControlPanelInfo.HelpLink = "https://www.opencode.net/numerone/CBriscola-Material/-/issues";
            project.Description = "Simulatore del gioco della briscola in avalonia a due giocatori senza multiplayer in dialetto Material di Google di avalonia";
            //            project.Properties.SetValue("ALLUSERS", 0);
            project.BuildMsi();
        }
    }
}
