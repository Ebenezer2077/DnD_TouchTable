using Godot;
using System;

public partial class GridButton : Button
{

    private Button _gridButton;
    private PopupMenu _popupMenu;
    public Vector2I _position;
    public bool Playmode = false;
    public Action<Vector2> onPressed;
    public Action LoadUnitAction;
    public Action<Vector2I> MoveObject;
    public Action<GridButton> DeleteObjectAction;
    public PanelContainer _loadUnit;

    public override void _Ready()
    {
        _popupMenu = GetNode<PopupMenu>("PopupMenu");
        InitPopupMenuHandlíng();
        _gridButton = GetNode<Button>(".");
        _gridButton.Pressed += () =>
        {
            onPressed?.Invoke(this.GlobalPosition);
        };
    }

    public void OpenButtonpopup(Vector2 globalPosition, bool isCellFree)
    {
        InitPopupMenu(isCellFree);
        _popupMenu.Visible = true;
        _popupMenu.Position = new Vector2I((int)globalPosition.X, (int)globalPosition.Y);
    }

    private void InitPopupMenu(bool isCellFree)
    {
        _popupMenu.Clear();
        if(isCellFree) _popupMenu.AddItem("PlaceObject", 0);
        else
        {
            if(Playmode) _popupMenu.AddItem("MoveObject", 1);
            _popupMenu.AddItem("DeleteObject", 2);
        }
    }
    
    private void InitPopupMenuHandlíng()
    {
        _popupMenu.IdPressed += (id) =>
        {
            switch (id)
            {
                case 0:
                    _loadUnit.Visible = true;
                    _popupMenu.Visible = false;
                    break;
                case 1:
                    MoveObject?.Invoke(_position);
                    break;
                case 2:
                    DeleteObjectAction?.Invoke(this);
                    break;
            }

        };
    }
}
