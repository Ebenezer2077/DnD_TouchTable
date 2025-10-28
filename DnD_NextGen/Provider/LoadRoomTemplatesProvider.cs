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
            var image = new Image();
            var err = image.Load(path);
            var texture = ImageTexture.CreateFromImage(image);
            texture.SetSizeOverride(new Vector2I(200, 100));
            list.Add((room, texture));
        }
        return list;
    }
    
    public static RoomTemplate LoadRoom(string roomName)
    {
        var file = Godot.FileAccess.Open(Path.Combine(roomName, "data.json"), Godot.FileAccess.ModeFlags.Read);
        var data = file.GetAsText();
        file.Close();
        return JsonSerializer.Deserialize<RoomTemplate>(data, new JsonSerializerOptions { IncludeFields = true });
    }
}