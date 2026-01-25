using System;
using Godot;

public partial class GameManager : Node
{
    public SceneTree tree;
    private Cell[,] Cells;
    private Vector2I? grabbedFrom;
    public Action<Vector2I, Vector2I> SwapObjectsAction;

    public void InitCells(Vector2I dimension)
    {
        Cells = new Cell[dimension.X, dimension.Y];
        for (var i = 0; i < dimension.X * dimension.Y; i++)
        {
            Cells[i % dimension.X, i / dimension.X] = new Cell { position = new Vector2I(i % dimension.X, i / dimension.X) };
        }
    }

    public void PlaceObject(Entity entity, Vector2I position)
    {
        Cells[position.X, position.Y].Object = entity;
    }
    public bool IsCellFree(Vector2I position)
    {
        return Cells[position.X, position.Y].Object == null;
    }

    public void MoveObject(Vector2I position)
    {
        if(grabbedFrom == null)
        {
            grabbedFrom = position;
        } else
        {
            SwapObjects(position, grabbedFrom.Value);
            grabbedFrom = null;
        }
    }

    private void SwapObjects(Vector2I from, Vector2I to)
    {
        var holder = Cells[to.X, to.Y].Object;
        PlaceObject(Cells[from.X, from.Y].Object, to);
        PlaceObject(holder, from);
        SwapObjectsAction?.Invoke(from, to);
    }

    public void DeleteObject(Vector2I position)
    {
        PlaceObject(null, position);
    }
}