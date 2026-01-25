using Godot;
using System;

public partial class LoadRoomMenu : PanelContainer
{
    private Button Cancel;
    private ItemList RoomList;
    public Action<string> LoadRoom;
    public override void _Ready()
    {
        Cancel = GetNode<Button>("VBoxContainer/Cancel");
        Cancel.Pressed += () => Visible = false;
        RoomList = GetNode<ItemList>("VBoxContainer/ItemList");
        RoomList.ItemSelected += (i) => LoadRoom?.Invoke(RoomList.GetItemText((int)i));
    }

    public void InitRoomList()
    {
        RoomList.Clear();
        var roomList = LoadRoomTemplatesProvider.LoadAllRoomPreview();
        foreach (var room in roomList)
        {
            RoomList.AddItem(room.roomName, room.background);
        }
    }
}
