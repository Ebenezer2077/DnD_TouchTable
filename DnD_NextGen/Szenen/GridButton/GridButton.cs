using Godot;
using System;

public partial class GridButton : Button
{

    private Button _gridButton;
    public Vector2I _position;
    public bool Playmode = false;

    public Action<Vector2, Vector2I> onPressed;

    public override void _Ready()
    {
        _gridButton = GetNode<Button>(".");
        _gridButton.Pressed += () =>
        {
            if (Playmode) onPressed?.Invoke(this.GlobalPosition, _position);
        };
    }


    
}
