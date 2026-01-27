using Godot;
using System;
using System.Linq;

public partial class LoadRoomMenu : PanelContainer
{
    private Button Cancel;
    private Button Load;
    private Button Delete;
    private ItemList RoomList;
    public Action<string> LoadRoom;
    private long index = 0;
    public override void _Ready()
    {
        Cancel = GetNode<Button>("VBoxContainer/HBoxContainer/Cancel");
        Cancel.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
        RoomList = GetNode<ItemList>("VBoxContainer/ItemList");
        RoomList.ItemSelected += (i) => {
            index = i;
            Delete.Visible = true;
            Load.Visible = true;
        };
        Load = GetNode<Button>("VBoxContainer/HBoxContainer/Load");
        Load.Pressed += () => {
            if(RoomList.IsAnythingSelected()) LoadRoom?.Invoke(RoomList.GetItemText((int)index));
        };
        Delete = GetNode<Button>("VBoxContainer/HBoxContainer/Delete");
        Delete.Pressed += () => {
            if(RoomList.IsAnythingSelected())
            {
                LoadRoomTemplatesProvider.DeleteRoom(RoomList.GetItemText(RoomList.GetSelectedItems().First()));
                InitRoomList();
            }
        };
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
