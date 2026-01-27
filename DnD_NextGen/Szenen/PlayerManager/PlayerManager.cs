using Godot;
using System.Linq;

public partial class PlayerManager : PanelContainer
{
    private ItemList itemList;
    private Button createNew;
    private Button delete;
    private Button exitButton;
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
        delete.Pressed += () =>
        {
            var index = itemList.GetSelectedItems().First();
            var unit = itemList.GetItemText(index);
            LoadUnitsProvider.DeleteUnit(unit);
            itemList.RemoveItem(index);
        };
        itemList = GetNode<ItemList>("MarginContainer/VBoxContainer/ItemList");
        textFieldPopup = GetNode<TextFieldPopup>("TextFieldPopup");
        textFieldPopup.onConfirm += (name) =>
        {
            LoadUnitsProvider.SaveUnit(resourcePath, name);
            InitUnitList();
        };
        fileDialog = GetNode<FileDialog>("FileDialog");
        fileDialog.Access = FileDialog.AccessEnum.Filesystem;
        fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        fileDialog.FileSelected += async (path) =>
        {
            textFieldPopup.Visible = true;
            resourcePath = path;
        };
        exitButton = GetNode<Button>("MarginContainer/VBoxContainer/Exit");
        exitButton.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
        InitUnitList();
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

    private void InitUnitList()
    {
        itemList.Clear();
        var unitList = LoadUnitsProvider.LoadAllUnits();
        foreach (var unit in unitList)
        {
            itemList.AddItem(unit.basetype, unit.icon);
        }
    }
}
