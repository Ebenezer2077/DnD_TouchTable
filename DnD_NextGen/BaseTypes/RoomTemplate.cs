using Godot;

public class RoomTemplate(Vector2 gridPosition, Vector2 gridSize, int buttonSize)
{
    public Vector2 GridPosition { get; } = gridPosition;
    public Vector2 GridSize { get; } = gridSize;
    public int ButtonSize { get; } = buttonSize;
}