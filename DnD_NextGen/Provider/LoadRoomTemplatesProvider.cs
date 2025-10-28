using System;
using System.Collections.Generic;
using System.IO;
using Godot;

public class LoadRoomTemplatesProvider
{
    public static List<(string, ImageTexture)> LoadAllRoomPreview()
    {
        var list = new List<(string, ImageTexture)>();
        foreach(var room in Directory.GetDirectories("SavedRooms/"))
        {
            var path = Path.Combine(room, "map.png");
            var image = new Image();
            var err = image.Load(path);
            var texture = ImageTexture.CreateFromImage(image);
            texture.SetSizeOverride(new Vector2I(200, 100));
            list.Add((room, texture));
        }
        return list;
    }
}