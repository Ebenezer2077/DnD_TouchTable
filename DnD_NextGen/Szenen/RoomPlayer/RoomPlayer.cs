using Godot;

public partial class RoomPlayer : Panel
{
    private TextureRect _map;
    private GridContainer _gridcontainer;
    private LoadRoomMenu _loadRoomMenu;

    public override void _Ready()
    {
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
        _map.Texture = roomData.Item2;
        _gridcontainer.Columns = (int)roomData.Item1.GridSize.X;
        _gridcontainer.Position = GridPosition;
        for(var i = 0; i < roomData.Item1.GridSize.X * roomData.Item1.GridSize.Y; i++)
        {
            var button = GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
            button.CustomMinimumSize = new Vector2(buttonSize, buttonSize);
            _gridcontainer.AddChild(button);
        }
    }

}
