using System.Numerics;
using System.Threading;
using Godot;
using Godot.Collections;

public partial class GameManager : Node
{
    public SceneTree tree;
    private Cell[,] Cells;

    public void InitCells(Vector2I dimension)
    {
        Cells = new Cell[dimension.X, dimension.Y];
        for (var i = 0; i < dimension.X * dimension.Y - 1; i++)
        {
            Cells[i % dimension.X, i / dimension.X] = new Cell();
        }
    }
}