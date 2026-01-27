using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Godot;

public class LoadRoomTemplatesProvider
{
    public static List<(string roomName, ImageTexture background)> LoadAllRoomPreview()
    {
        var list = new List<(string roomName, ImageTexture background)>();
        var defaultDir = DirAccess.Open("res://Defaults/Rooms");
        foreach (var room in defaultDir.GetDirectories())
        {
            var path = "res://Defaults/Rooms/" + room + "/map.png";
            Texture2D texture2d = ResourceLoader.Load<Texture2D>(path);
            var image = texture2d.GetImage();
            var texture = ImageTexture.CreateFromImage(image);
            texture.SetSizeOverride(new Vector2I(100, 100));
            list.Add((room, texture));
        }
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
        if(DirAccess.DirExistsAbsolute("user://SavedRooms/" + roomName))
        {
            var file = Godot.FileAccess.Open("user://SavedRooms/" + Path.Combine(roomName, "data.json"), Godot.FileAccess.ModeFlags.Read);
            var data = file.GetAsText();
            file.Close();
            var path = "user://SavedRooms/" + Path.Combine(roomName, "map.png");
            ImageTexture texture = null;
            if(Godot.FileAccess.FileExists(path))
            {
                var image = new Image();
                var err = image.Load(path);
                texture = ImageTexture.CreateFromImage(image);
            }
            var RoomTemplate = JsonSerializer.Deserialize<RoomTemplate>(data, new JsonSerializerOptions { IncludeFields = true });
            var Cells = new Cell[RoomTemplate.GridSize.X, RoomTemplate.GridSize.Y];
            foreach(var Cell in RoomTemplate.Cells)
            {
                Cells[(int)Cell.position.X, (int)Cell.position.Y] = Cell;
            }
            return (new Room(new Vector2(RoomTemplate.GridPosition.X, RoomTemplate.GridPosition.Y), RoomTemplate.GridSize, RoomTemplate.ButtonSize, RoomTemplate.Name, Cells), texture);
        } else
        {
            var defaultpath = "res://Defaults/Rooms/" + roomName;
            var file = Godot.FileAccess.Open(defaultpath + "/data.json", Godot.FileAccess.ModeFlags.Read);
            var data = file.GetAsText();
            file.Close();
            var mapPath = defaultpath + "/map.png";
            ImageTexture texture = null;
            if(Godot.FileAccess.FileExists(mapPath))
            {
                Texture2D texture2d = ResourceLoader.Load<Texture2D>(mapPath);
                var image = texture2d.GetImage();
                texture = ImageTexture.CreateFromImage(image);
            }
            var RoomTemplate = JsonSerializer.Deserialize<RoomTemplate>(data, new JsonSerializerOptions { IncludeFields = true });
            var Cells = new Cell[RoomTemplate.GridSize.X, RoomTemplate.GridSize.Y];
            foreach(var Cell in RoomTemplate.Cells)
            {
                Cells[(int)Cell.position.X, (int)Cell.position.Y] = Cell;
            }
            return (new Room(new Vector2(RoomTemplate.GridPosition.X, RoomTemplate.GridPosition.Y), RoomTemplate.GridSize, RoomTemplate.ButtonSize, RoomTemplate.Name, Cells), texture);
        }
    }

    public static void DeleteRoom(string roomName)
    {
        if(DirAccess.DirExistsAbsolute("user://SavedRooms/" + roomName))
        {
            ClearDirectory("user://SavedRooms/" + roomName);
            DirAccess.RemoveAbsolute("user://SavedRooms/" + roomName);
        }
    }

    private static void ClearDirectory(string path)
    {
        var dir = DirAccess.Open(path);
        while(!DirAccess.GetFilesAt(path).IsEmpty())
        {
            dir.Remove(Path.Combine(path, DirAccess.GetFilesAt(path).First()));
        }
    }

    public static void SaveRoom(RoomTemplate room, Texture2D Background)
    {
        var path = "user://SavedRooms/" + room.Name;
        var dir = DirAccess.Open("user://");
        dir.MakeDirRecursive("SavedRooms/" + room.Name);
        var data = JsonSerializer.Serialize(room, new JsonSerializerOptions { IncludeFields = true });
        Background?.GetImage()?.SavePng(path + "/map.png");
        var file = Godot.FileAccess.Open(path + "/data.json", Godot.FileAccess.ModeFlags.Write);
        file.StoreLine(data);
        file.Close();
    }
}