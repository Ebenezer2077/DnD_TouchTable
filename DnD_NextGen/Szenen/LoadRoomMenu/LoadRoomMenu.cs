using Godot;
using System;

public partial class LoadRoomMenu : PanelContainer
{
    private Button Cancel;
    private ItemList RoomList;
    public override void _Ready()
    {
        Cancel = GetNode<Button>("VBoxContainer/Cancel");
        Cancel.Pressed += () => Visible = false;
        RoomList = GetNode<ItemList>("VBoxContainer/ItemList");
        InitRoomList();
        //RoomList.ItemSelected += (a) => LoadRoomTemplatesProvider.LoadAllRoomPreview();//eigentlich falsch
    }

    private void InitRoomList()
    {
        var roomList = LoadRoomTemplatesProvider.LoadAllRoomPreview();
        foreach (var room in roomList)
        {
            RoomList.AddItem(room.Item1, room.Item2);
        }
    }
}
