using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

public class LoadUnitsProvider
{
    public static List<(string basetype, ImageTexture icon)> LoadAllUnits()
    {
        var dir = DirAccess.Open("user://SavedUnits");
        var defaultDir = DirAccess.Open("res://Defaults/Units");
        var list = new List<(string, ImageTexture)>();

        foreach (var unit in defaultDir.GetDirectories())
        {
            var path = "res://Defaults/Units/" + unit;
            var file = DirAccess.GetFilesAt(path).First(x => x.Contains(".png") || x.Contains(".jpg"));
            if (Godot.FileAccess.FileExists(path + "/" + file))                                      //can be optimized
            {
                var loadpath = path + "/" + file;
                Texture2D texture2d = ResourceLoader.Load<Texture2D>(loadpath);
                var image = texture2d.GetImage();
                var texture = ImageTexture.CreateFromImage(image);
                texture.SetSizeOverride(new Vector2I(100, 100));
                list.Add((unit, texture));
            }
            else
            {
                var image = Image.CreateEmpty(100, 100, false, Image.Format.Rgba8);
                var texture = ImageTexture.CreateFromImage(image);
                list.Add((unit, texture));
            }
        }
        
        foreach (var unit in dir.GetDirectories())
        {
            var path = "user://SavedUnits/" + unit;
            var file = DirAccess.GetFilesAt(path).First(x => x.Contains(".png") || x.Contains(".jpg"));
            if (Godot.FileAccess.FileExists(path + "/" + file))                                       //can be optimized
            {
                var loadpath = path + "/" + file;
                var image = new Image();
                var err = image.Load(Path.Combine(path, file));
                var texture = ImageTexture.CreateFromImage(image);
                texture.SetSizeOverride(new Vector2I(100, 100));
                list.Add((unit, texture));
            }
            else
            {
                var image = Image.CreateEmpty(100, 100, false, Image.Format.Rgba8);
                var texture = ImageTexture.CreateFromImage(image);
                list.Add((unit, texture));
            }
        }
        return list;
    }

    public static ImageTexture LoadUnit(string basetype)
    {
        var path = "user://SavedUnits/" + basetype;                                      //can be optimized
        if(DirAccess.DirExistsAbsolute(path))
        {
            var image = new Image();
            var file = DirAccess.GetFilesAt(path).FirstOrDefault(x => x.Contains(".png") || x.Contains(".jpg"));
            var err = image.Load(path + "/" + file);
            var texture = ImageTexture.CreateFromImage(image);
            texture.SetSizeOverride(new Vector2I(100, 100));
            return texture;
        }
        var defaultPath = "res://Defaults/Units/" + basetype;
        if(DirAccess.DirExistsAbsolute(defaultPath))
        {
            var defaultFile = DirAccess.GetFilesAt(defaultPath).FirstOrDefault(x => x.Contains(".png") || x.Contains(".jpg"));
            Texture2D texture2d = ResourceLoader.Load<Texture2D>(defaultPath + "/" + defaultFile);
            var image = texture2d.GetImage();
            var texture = ImageTexture.CreateFromImage(image);
            texture.SetSizeOverride(new Vector2I(100, 100));
            return texture;
        }
        else
        {
            var image = Image.CreateEmpty(100, 100, false, Image.Format.Rgba8);
            return ImageTexture.CreateFromImage(image);
        }
    }
    
    public static void SaveUnit(string from, string name)
    {
        var targetPath = "user://SavedUnits/" + name;
        var dir = DirAccess.Open("user://");
        dir.MakeDirRecursive(targetPath);
        var image = new Image();
        var err = image.Load(from);
        image.SavePng(targetPath + "/" + name + ".png");
    }

    public static void DeleteUnit(string name)
    {
        var path = "user://SavedUnits/" + name;
        var dir = DirAccess.Open(path);
        if (dir == null) return;
        while (!dir.GetFiles().IsEmpty())
        {
            dir.Remove(path + dir.GetFiles().First());
        }
        dir.Remove(path);
    }
}