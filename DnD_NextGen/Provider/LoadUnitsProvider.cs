using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using Godot;

public class LoadUnitsProvider//rename
{
    public static List<(string, ImageTexture)> LoadAllUnits()
    {
        var dir = DirAccess.Open("user://SavedUnits");
        var list = new List<(string, ImageTexture)>();
        
        foreach (var room in dir.GetDirectories())
        {
            var path = Path.Combine("user://SavedUnits", room);
            var file = DirAccess.GetFilesAt(path).First(x => x.Contains(".png") || x.Contains(".jpg"));
            if (Godot.FileAccess.FileExists(Path.Combine(path, file)))                                       //can be optimized
            {
                var image = new Image();
                var err = image.Load(Path.Combine(path, file));
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
        var targetPath = Path.Combine("user://SavedUnits", name);
        var dir = DirAccess.Open("user://");
        dir.MakeDirRecursive(targetPath);
        var image = new Image();
        var err = image.Load(from);
        image.SavePng(Path.Combine(targetPath, name + ".png"));
    }
}