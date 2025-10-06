using Godot;
using System;

public partial class MainMenu : PanelContainer
{
    private Button _playButton;
    private Button _createRoomButton;
    public override void _Ready()
    {
        _playButton = GetNode<Button>("MarginContainer2/VBoxContainer/Play");
        _playButton.Pressed += () =>
        {

        };
        _createRoomButton = GetNode<Button>("MarginContainer2/VBoxContainer/CreateNewRoom");
        _createRoomButton.Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Szenen/RoomCreator/RoomCreator.tscn");
        };
    }
}
