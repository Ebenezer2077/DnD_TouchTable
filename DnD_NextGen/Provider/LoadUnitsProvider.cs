using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

public class LoadUnitsProvider//rename
{
    public static List<(string, ImageTexture)> LoadAllUnits()
    {
        var list = new List<(string, ImageTexture)>();
        foreach (var room in Directory.GetDirectories("SavedUnits/"))
        {
            var file = Directory.GetFiles(room).First(x => x.Contains(".png") || x.Contains(".jpg"));
            if (File.Exists(file))                                       //can be optimized
            {
                var image = new Image();
                var err = image.Load(file);
                var texture = ImageTexture.CreateFromImage(image);
                texture.SetSizeOverride(new Vector2I(100, 100));
                list.Add((room, texture));
            }
            else
            {
                var image = Image.CreateEmpty(100, 100, false, Image.Format.Rgba8);
                var texture = ImageTexture.CreateFromImage(image);
                list.Add((room, texture));
            }
        }
        return list;
    }
    
    public static void SaveUnit(string from, string name)
    {
        var targetPath = Path.Combine("res://SavedUnits", name);
        var image = new Image();
        var err = image.Load(from);
        image.SavePng(Path.Combine(targetPath, name + ".png"));
    }
}