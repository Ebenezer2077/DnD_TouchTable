using Godot;

public partial class GameManager : Node
{
    public SceneTree tree;
    private Cell[,] Cells;

    public void InitCells(Vector2I dimension)
    {
        Cells = new Cell[dimension.X, dimension.Y];
        for (var i = 0; i < dimension.X * dimension.Y - 1; i++)
        {
            Cells[i % dimension.X, i / dimension.X] = new Cell { position = new Vector2I(i % dimension.X, i / dimension.X) };
        }
    }

    public void PlaceObject(string name, Vector2I position)
    {
        Cells[position.X, position.Y].Object = name;
    }
    public bool IsCellFree(Vector2I position)
    {
        return Cells[position.X, position.Y].Object == null;
    }

    public void MoveObject(Vector2I position)
    {
        if(Cells[position.X, position.Y].Object == "")
        {
            
        }
    }
}