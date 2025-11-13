using Godot;
using System;

public partial class MainMenu : PanelContainer
{
    private Button _playButton;
    private Button _managePlayers;
    private Button _createRoomButton;
    
    public override void _Ready()
    {
        _playButton = GetNode<Button>("MarginContainer2/VBoxContainer/Play");
        _playButton.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/RoomPlayer/RoomPlayer.tscn");
        _managePlayers = GetNode<Button>("MarginContainer2/VBoxContainer/ManagePlayers");
        _managePlayers.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/PlayerManager/PlayerManager.tscn");
        _createRoomButton = GetNode<Button>("MarginContainer2/VBoxContainer/CreateNewRoom");
        _createRoomButton.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/RoomCreator/RoomCreator.tscn");

    }

}
