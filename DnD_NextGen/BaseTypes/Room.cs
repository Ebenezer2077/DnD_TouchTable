using Godot;

public class Room(Vector2 gridPosition, Vector2 gridSize, int buttonSize, string name)
{
    public Vector2 GridPosition { get; } = gridPosition;
    public Vector2 GridSize { get; } = gridSize;
    public int ButtonSize { get; } = buttonSize;
    public string Name { get; } = name;
    public Cell[,] Cells { get; }
}