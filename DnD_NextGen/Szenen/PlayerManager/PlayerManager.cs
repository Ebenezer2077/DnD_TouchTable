using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class PlayerManager : PanelContainer
{
    private ItemList itemList;
    private Button createNew;
    private Button delete;
    private FileDialog fileDialog;
    private TextFieldPopup textFieldPopup;
    private string resourcePath = "";

    public override void _Ready()
    {
        createNew = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/CreateNew");
        createNew.Pressed += () =>
        {
            fileDialog.Visible = true;
        };
        delete = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/Delete");
        itemList = GetNode<ItemList>("MarginContainer/VBoxContainer/ItemList");
        textFieldPopup = GetNode<TextFieldPopup>("TextFieldPopup");
        textFieldPopup.onConfirm += (name) =>
        {
            LoadUnitsProvider.SaveUnit(resourcePath, name);
            InitRoomList();
        };
        fileDialog = GetNode<FileDialog>("FileDialog");
        fileDialog.Access = FileDialog.AccessEnum.Filesystem;
        fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        fileDialog.FileSelected += async (path) =>
        {
            textFieldPopup.Visible = true;
            resourcePath = path;
        };
        InitRoomList();
    }

    public override void _Process(double delta)
    {
        
        if (itemList.IsAnythingSelected()) delete.Visible = true;
        else delete.Visible = false;
    }


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Back")) GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
    }

    private void InitRoomList()
    {
        itemList.Clear();
        var unitList = LoadUnitsProvider.LoadAllUnits();
        foreach (var room in unitList)
        {
            itemList.AddItem(room.Item1, room.Item2);
        }
    }
}
