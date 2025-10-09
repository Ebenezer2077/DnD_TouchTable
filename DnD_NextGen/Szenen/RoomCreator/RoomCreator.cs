using Godot;
using System;
using System.Collections.Generic;

public partial class RoomCreator : PanelContainer
{
    private GridContainer gridContainer;
    private Button AddColumn;
    private RichTextLabel ColumnCount;
    private Button RemoveColumn;
    private VSlider GridSize;
    private RichTextLabel GridSizeCount;
    private Button AddRow;
    private RichTextLabel RowCount;
    private Button RemoveRow;

    private int columns = 1;
    private int rows = 1;
    private int buttonSize = 50;
    public override void _Ready()
    {
        gridContainer = GetNode<GridContainer>("GridContainer");
        AddColumn = GetNode<Button>("MarginContainer/VBoxContainer/AddColumn");
        AddColumn.Pressed += () => AdjustColumns(1);
        ColumnCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/RichTextLabel");
        RemoveColumn = GetNode<Button>("MarginContainer/VBoxContainer/RemoveColumn");
        RemoveColumn.Pressed += () => AdjustColumns(-1);
        GridSize = GetNode<VSlider>("MarginContainer/VBoxContainer3/HBoxContainer/VSlider");
        GridSizeCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer3/RichTextLabel");
        AddRow = GetNode<Button>("MarginContainer/VBoxContainer2/AddRow");
        AddRow.Pressed += () => AdjustRows(1);
        RowCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer2/RichTextLabel");
        RemoveRow = GetNode<Button>("MarginContainer/VBoxContainer2/RemoveRow");
        RemoveRow.Pressed += () => AdjustRows(-1);

        gridContainer.Columns = columns;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Back"))
        {
            GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
        }
    }

    private void AdjustRows(int amount)
    {
        rows += amount;
        foreach (var button in gridContainer.GetChildren())
        {
            gridContainer.RemoveChild(button);
        }
        for (int i = 0; i < rows * columns; i++)
        {
            var button = GenerateGridButton();
            gridContainer.AddChild(button);
        }
    }
    private void AdjustColumns(int amount)
    {
        columns += amount;
        gridContainer.Columns = columns;
        foreach (var button in gridContainer.GetChildren())
        {
            gridContainer.RemoveChild(button);
        }
        for (int i = 0; i < rows * columns; i++)
        {
            var button = GenerateGridButton();
            gridContainer.AddChild(button);
        }
    }
    
    private Button GenerateGridButton()
    {
        return GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
    }
}
