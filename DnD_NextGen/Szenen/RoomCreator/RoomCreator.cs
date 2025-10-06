using Godot;
using System;
using System.Collections.Generic;

public partial class RoomCreator : PanelContainer
{
    private GridContainer gridContainer;
    private int columns = 20;
    public override void _Ready()
    {
        gridContainer = GetNode<GridContainer>("GridContainer");
        gridContainer.Columns = columns;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Back"))
        {
            GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
        }
        if (@event.IsActionPressed("IncreaseCells"))
        {
            columns++;
            var size = CalculateSize(columns);
            var amount = CalculateAmount(size);
            foreach (var button in gridContainer.GetChildren())
            {
                gridContainer.RemoveChild(button);
            }
            foreach (var button in GenerateGridButtons(amount, size))
            {
                gridContainer.AddChild(button);
            }
        }
        if (@event.IsActionPressed("DecreaseCells"))
        {

        }
    }

    private int CalculateSize(int columns)
    {
        return (int)gridContainer.Size.X / columns;
    }

    private int CalculateAmount(int size)
    {
        return ((int)gridContainer.Size.X / size) * ((int)gridContainer.Size.Y / size);
    }

    private List<Button> GenerateGridButtons(int amount, int size)
    {
        var buttonList = new List<Button>();
        for (int i = 0; i < amount; i++)
        {
            var button = GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
            button.CustomMinimumSize = new Vector2(size, size);
            button.SetSize(new Vector2(size, size));
            buttonList.Add(button);
        }
        return buttonList;
    }
}
