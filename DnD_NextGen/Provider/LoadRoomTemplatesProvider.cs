using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Godot;

public class LoadRoomTemplatesProvider
{
    public static List<(string, ImageTexture)> LoadAllRoomPreview()
    {
        var list = new List<(string, ImageTexture)>();
        foreach (var room in DirAccess.GetDirectoriesAt("user://SavedRooms/"))
        {
            var path = "user://SavedRooms/" + Path.Combine(room, "map.png");
            if(Godot.FileAccess.FileExists(path))                                       //can be optimized
            {
                var image = new Image();
                var err = image.Load(path);
                var texture = ImageTexture.CreateFromImage(image);
                texture.SetSizeOverride(new Vector2I(200, 100));
                list.Add((room, texture));
            } else {
                var image = Image.CreateEmpty(200, 100, false, Image.Format.Rgba8);
                var texture = ImageTexture.CreateFromImage(image);
                list.Add((room, texture));
            }
        }
        return list;
    }
    
    public static (Room room, ImageTexture background) LoadRoom(string roomName)//only name not full directory
    {
        var file = Godot.FileAccess.Open("user://SavedRooms/" + Path.Combine(roomName, "data.json"), Godot.FileAccess.ModeFlags.Read);
        var data = file.GetAsText();
        file.Close();
        var path = "user://SavedRooms/" + Path.Combine(roomName, "map.png");
        if(Godot.FileAccess.FileExists(path))
        {
            var image = new Image();
            var err = image.Load(path);
            var texture = ImageTexture.CreateFromImage(image);
            return (JsonSerializer.Deserialize<Room>(data, new JsonSerializerOptions { IncludeFields = true }), texture);
        }
        return (JsonSerializer.Deserialize<Room>(data, new JsonSerializerOptions { IncludeFields = true }), null);
    }
}