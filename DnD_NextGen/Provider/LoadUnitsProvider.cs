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
            var path = $"res://Defaults/Units/{unit}/{unit}.png";
            if (ResourceLoader.Exists(path))                                      //can be optimized
            {
                Texture2D texture2d = ResourceLoader.Load<Texture2D>(path);
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
            var path = $"user://SavedUnits/{unit}/{unit}.png";
            if (Godot.FileAccess.FileExists(path))                                       //can be optimized
            {
                var image = new Image();
                var err = image.Load(path);
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
            var err = image.Load($"user://SavedUnits/{basetype}/{basetype}.png");
            var texture = ImageTexture.CreateFromImage(image);
            texture.SetSizeOverride(new Vector2I(100, 100));
            return texture;
        }
        var defaultPath = "res://Defaults/Units/" + basetype;
        if(DirAccess.DirExistsAbsolute(defaultPath))
        {
            Texture2D texture2d = ResourceLoader.Load<Texture2D>($"res://Defaults/Units/{basetype}/{basetype}.png");
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