using Godot;

public partial class MainMenu : PanelContainer
{
    private Button _playButton;
    private Button _managePlayers;
    private Button _createRoomButton;
    private GameManager _gameManager;

    public override void _Ready()
    {
        _playButton = GetNode<Button>("MarginContainer2/VBoxContainer/Play");
        _playButton.Pressed += () =>
        {
            var playrom = GD.Load<PackedScene>("res://Szenen/RoomPlayer/RoomPlayer.tscn").Instantiate<RoomPlayer>();
            InitGameManager(playrom);
            GetTree().Root.AddChild(playrom);
            GetTree().CurrentScene.QueueFree();
            GetTree().CurrentScene = playrom;
        };
        
        


        _managePlayers = GetNode<Button>("MarginContainer2/VBoxContainer/ManagePlayers");
        _managePlayers.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/PlayerManager/PlayerManager.tscn");
        _createRoomButton = GetNode<Button>("MarginContainer2/VBoxContainer/CreateNewRoom");
        _createRoomButton.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/RoomCreator/RoomCreator.tscn");
        InitUserDirectory();
    }

    private void InitUserDirectory()
    {
        var dir = DirAccess.Open("user://");
        if(!dir.DirExists("SavedRooms"))dir.MakeDir("SavedRooms");
        if(!dir.DirExists("SavedUnits"))dir.MakeDir("SavedUnits");
    }

    private void InitGameManager(RoomPlayer roomPlayer)
    {
        _gameManager = new GameManager();
        roomPlayer.ParseGridData += _gameManager.InitCells;
        roomPlayer.ParsePlacedObject += _gameManager.PlaceObject;
        roomPlayer.IsCellFreeFunc += _gameManager.IsCellFree;
        roomPlayer.MoveObject += _gameManager.MoveObject;
        _gameManager.SwapObjectsAction += roomPlayer.SwapObjects;
    }
}
