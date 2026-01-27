using System.Collections.Generic;
using Godot;

public class RoomTemplate
{
    public Vector2 GridPosition { get; set; }
    public Vector2I GridSize { get; set; }
    public int ButtonSize { get; set; }
    public string Name { get; set; }
    public List<Cell> Cells { get; set; } = [];
}