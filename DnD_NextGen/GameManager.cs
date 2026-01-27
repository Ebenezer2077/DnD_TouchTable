using System;
using Godot;

public partial class GameManager : Node
{
    public SceneTree tree;
    private Cell[,] Cells;
    private Vector2I Gridsize;
    private Vector2I? grabbedFrom;
    private string RoomName;
    public Action<Vector2I, Vector2I> SwapObjectsAction;
    public Action<string, string, Vector2I> PlaceObjectInUI;

    public void InitCells(Vector2I dimension)
    {
        Gridsize = dimension;
        Cells = new Cell[dimension.X, dimension.Y];
        for (var i = 0; i < dimension.X * dimension.Y; i++)
        {
            Cells[i % dimension.X, i / dimension.X] = new Cell { position = new System.Numerics.Vector2(i % dimension.X, i / dimension.X) };
        }
    }

    public void LoadCells(Cell[,] cells)
    {
        Cells = cells;
    }
    public Cell GetCell(Vector2I position)
    {
        return Cells[position.X, position.Y];
    }

    public void PlaceObject(string name, string basetype, Vector2I position)
    {
        var entity = new Entity(name, basetype);
        PlaceObject(entity, position);
        PlaceObjectInUI?.Invoke(entity.name, entity.basetype, position);
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
    public void SaveRoom(Vector2 position, int Cellsize, string RoomName, Texture2D Background)
    {
        this.RoomName = RoomName;
        var room = new RoomTemplate();
        room.GridPosition = position;
        room.GridSize = Gridsize;
        room.ButtonSize = Cellsize;
        room.Name = RoomName;
        foreach(var cell in Cells) room.Cells.Add(cell);
        LoadRoomTemplatesProvider.SaveRoom(room, Background);
    }
}