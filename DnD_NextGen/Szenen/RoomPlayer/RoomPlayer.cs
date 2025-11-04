using System;
using Godot;

public partial class RoomPlayer : Panel
{
    private TextureRect _map;
    private GridContainer _gridcontainer;
    private LoadRoomMenu _loadRoomMenu;
    private PopupMenu _popupMenu;
    public override void _Ready()
    {
        _popupMenu = GetNode<PopupMenu>("PopupMenu");
        _map = GetNode<TextureRect>("Map");
        _gridcontainer = GetNode<GridContainer>("GridContainer");
        _loadRoomMenu = GetNode<LoadRoomMenu>("LoadRoomMenu");
        _loadRoomMenu.InitRoomList();
        _loadRoomMenu.LoadRoom += (name) =>
        {
            LoadRoom(name);
            _loadRoomMenu.Visible = false;
        };
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Back")) GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
    }

    private void LoadRoom(string name)
    {
        var roomData = LoadRoomTemplatesProvider.LoadRoom(name);
        var buttonSize = roomData.Item1.ButtonSize;
        var GridPosition = roomData.Item1.GridPosition;
        var columns = roomData.Item1.GridSize.X;
        var rows = roomData.Item1.GridSize.Y;
        _map.Texture = roomData.Item2;
        _gridcontainer.Columns = (int)roomData.Item1.GridSize.X;
        _gridcontainer.Position = GridPosition;
        for (var i = 0; i < columns * rows; i++)
        {
            var button = GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
            button.onPressed += OpenButtonpopup;
            button.CustomMinimumSize = new Vector2(buttonSize, buttonSize);
            button.Playmode = true;
            button._position = new Vector2I((int)(i % rows), (int)(i / columns));
            _gridcontainer.AddChild(button);
        }
    }
    
    private void OpenButtonpopup(Vector2 globalPosition, Vector2I gridPosition)
    {
        _popupMenu.Visible = true;
        _popupMenu.Position = new Vector2I((int)globalPosition.X, (int)globalPosition.Y);
    }

}
