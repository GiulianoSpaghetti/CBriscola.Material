using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;

namespace CBriscola.ViewModels
{
    public class MyCarta
    {
        public Image Img { get; private set; }
        public String Nome { get; private set; }

        public MyCarta(int img, String nome, String path)
        {
            if (nome=="Napoletano")
                Img = new Image { Source = new Bitmap(AssetLoader.Open(new Uri($"avares://CBriscola/Assets/{img}.png"))) }; 
            else
                Img = new Image { Source = new Bitmap(Path.Combine(Path.Combine(path, nome), $"{img}.png") )};
            Nome = nome;
        }
    }
}
