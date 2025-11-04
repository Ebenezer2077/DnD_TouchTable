using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public class LoadRoomTemplatesProvider
{
    public static List<(string, ImageTexture)> LoadAllRoomPreview()
    {
        var list = new List<(string, ImageTexture)>();
        foreach (var room in Directory.GetDirectories("SavedRooms/"))
        {
            var path = Path.Combine(room, "map.png");
            if(File.Exists(path))                                       //can be optimized
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
    
    public static (RoomTemplate, ImageTexture) LoadRoom(string roomName)
    {
        var file = Godot.FileAccess.Open(Path.Combine(roomName, "data.json"), Godot.FileAccess.ModeFlags.Read);
        var data = file.GetAsText();
        file.Close();
        var path = Path.Combine(roomName, "map.png");
        if(File.Exists(path))
        {
            var image = new Image();
            var err = image.Load(path);
            var texture = ImageTexture.CreateFromImage(image);
            return (JsonSerializer.Deserialize<RoomTemplate>(data, new JsonSerializerOptions { IncludeFields = true }), texture);
        }
        return (JsonSerializer.Deserialize<RoomTemplate>(data, new JsonSerializerOptions { IncludeFields = true }), null);
    }
}