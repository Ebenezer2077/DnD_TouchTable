using Godot;
using System;

public partial class MainMenu : PanelContainer
{
    private Button _playButton;
    private Button _createRoomButton;
    private Button NewRoom;
    private Button LoadRoom;
    private PanelContainer RoomOptions;
    private LoadRoomMenu loadRoomMenu;
    public override void _Ready()
    {
        loadRoomMenu = GetNode<LoadRoomMenu>("LoadRoomMenu");
        RoomOptions = GetNode<PanelContainer>("RoomOptions");
        NewRoom = GetNode<Button>("RoomOptions/MarginContainer/VBoxContainer/HBoxContainer/NewRoom");
        NewRoom.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/RoomCreator/RoomCreator.tscn");
        LoadRoom = GetNode<Button>("RoomOptions/MarginContainer/VBoxContainer/HBoxContainer/LoadRoom");
        LoadRoom.Pressed += () => loadRoomMenu.Visible = true;
        _playButton = GetNode<Button>("MarginContainer2/VBoxContainer/Play");
        _playButton.Pressed += () =>
        {

        };
        _createRoomButton = GetNode<Button>("MarginContainer2/VBoxContainer/CreateNewRoom");
        _createRoomButton.Pressed += () => RoomOptions.Visible = true;

    }

}
