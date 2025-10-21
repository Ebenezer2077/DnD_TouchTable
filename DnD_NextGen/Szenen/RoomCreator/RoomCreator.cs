using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class RoomCreator : Panel
{
    private Button SaveRoom;
    private FileDialog fileDialog;
    private GridContainer gridContainer;
    private Button AddColumn;
    private RichTextLabel ColumnCount;
    private Button RemoveColumn;
    private VSlider GridSize;
    private RichTextLabel GridsizeLabel;
    private RichTextLabel GridSizeCount;
    private Button AddRow;
    private RichTextLabel RowCount;
    private Button RemoveRow;
    private Button PositionGrid;
    private Button LoadMap;
    private TextureRect Map;

    private int columns = 1;
    private int rows = 1;
    private int buttonSize = 50;
    private bool isGridPositioned = false;
    public override void _Ready()
    {
        SaveRoom = GetNode<Button>("MarginContainer/VBoxContainer4/SaveRoom");
        SaveRoom.Pressed += () =>
        {
            var room = new RoomTemplate(gridContainer.GlobalPosition, new Vector2(rows, columns), buttonSize);
            var data = JsonSerializer.Serialize(room);
            var file = FileAccess.Open("res://SavedRooms/Test/data.json", FileAccess.ModeFlags.Write);
            file.StoreLine(data);
        };
        Map = GetNode<TextureRect>("TextureRect");
        fileDialog = GetNode<FileDialog>("FileDialog");
        fileDialog.Access = FileDialog.AccessEnum.Filesystem;
        fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        fileDialog.FileSelected += (path) =>
        {
            var image = new Image();
            var err = image.Load(path);
            Map.Texture = ImageTexture.CreateFromImage(image);
        };
        gridContainer = GetNode<GridContainer>("GridContainer");
        AddColumn = GetNode<Button>("MarginContainer/VBoxContainer/AddColumn");
        AddColumn.Pressed += () => AdjustColumns(1);
        ColumnCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/RichTextLabel");
        RemoveColumn = GetNode<Button>("MarginContainer/VBoxContainer/RemoveColumn");
        RemoveColumn.Pressed += () => AdjustColumns(-1);
        GridSize = GetNode<VSlider>("MarginContainer/VBoxContainer3/HBoxContainer/VSlider");
        GridSize.ValueChanged += (value) => AdjustGridSize((int)value);
        GridsizeLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer3/HBoxContainer/RichTextLabel");
        GridSizeCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer3/RichTextLabel");
        AddRow = GetNode<Button>("MarginContainer/VBoxContainer2/AddRow");
        AddRow.Pressed += () => AdjustRows(1);
        RowCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer2/RichTextLabel");
        RemoveRow = GetNode<Button>("MarginContainer/VBoxContainer2/RemoveRow");
        RemoveRow.Pressed += () => AdjustRows(-1);

        gridContainer.Columns = columns;

        PositionGrid = GetNode<Button>("MarginContainer/VBoxContainer4/Position Grid");
        PositionGrid.Pressed += () => PositionGridFunc();
        LoadMap = GetNode<Button>("MarginContainer/VBoxContainer4/Load Map");
        LoadMap.Pressed += () =>
        {
            fileDialog.Visible = true;
        };
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Back"))
        {
            GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
        }
        if (isGridPositioned && @event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.IsPressed())
        {
            PositionGridFunc();
        }
    }

    public override void _Process(double delta)
    {
        if(isGridPositioned)
        {
            gridContainer.Position = GetViewport().GetMousePosition();
        } else
        {
            //gridContainer.Position = new Vector2(1200, 800);
        }
    }
    private void PositionGridFunc()
    {
        isGridPositioned = !isGridPositioned;
        ToggleControlls(!isGridPositioned);
    }
    
    private void ToggleControlls(bool enable)
    {
        AddColumn.Visible = enable;
        ColumnCount.Visible = enable;
        RemoveColumn.Visible = enable;
        GridSize.Visible = enable;
        GridsizeLabel.Visible = enable;
        GridSizeCount.Visible = enable;
        AddRow.Visible = enable;
        RowCount.Visible = enable;
        RemoveRow.Visible = enable;
        PositionGrid.Visible = enable;
    }
    private void AdjustRows(int amount)
    {
        rows += amount;
        RowCount.Text = rows.ToString();
        RefreshContainer();
    }
    private void AdjustColumns(int amount)
    {
        columns += amount;
        ColumnCount.Text = columns.ToString();
        gridContainer.Columns = columns;
        RefreshContainer();
    }

    private void AdjustGridSize(int size)
    {
        buttonSize = size;
        GridSizeCount.Text = size.ToString();
        foreach (var button in gridContainer.GetChildren())
        {
            ((GridButton)button).CustomMinimumSize = new Vector2(buttonSize, buttonSize);
        }
    }

    private void RefreshContainer()
    {
        foreach (var button in gridContainer.GetChildren())
        {
            gridContainer.RemoveChild(button);
        }
        for (int i = 0; i < rows * columns; i++)
        {
            var button = GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
            button.CustomMinimumSize = new Vector2(buttonSize, buttonSize);
            gridContainer.AddChild(button);
        }
    }
}
