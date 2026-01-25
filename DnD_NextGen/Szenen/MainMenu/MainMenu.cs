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
            InitPlayGameManager(playrom);
            GetTree().Root.AddChild(playrom);
            GetTree().CurrentScene.QueueFree();
            GetTree().CurrentScene = playrom;
        };
        _managePlayers = GetNode<Button>("MarginContainer2/VBoxContainer/ManagePlayers");
        _managePlayers.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/PlayerManager/PlayerManager.tscn");
        _createRoomButton = GetNode<Button>("MarginContainer2/VBoxContainer/CreateNewRoom");
        _createRoomButton.Pressed += () => {
            var createRoom = GD.Load<PackedScene>("res://Szenen/RoomCreator/RoomCreator.tscn").Instantiate<RoomCreator>();
            InitCreateGameManager(createRoom);
            GetTree().Root.AddChild(createRoom);
            GetTree().CurrentScene.QueueFree();
            GetTree().CurrentScene = createRoom;
        };
        InitUserDirectory();
    }

    private void InitUserDirectory()
    {
        var dir = DirAccess.Open("user://");
        if(!dir.DirExists("SavedRooms"))dir.MakeDir("SavedRooms");
        if(!dir.DirExists("SavedUnits"))dir.MakeDir("SavedUnits");
    }

    private void InitPlayGameManager(RoomPlayer roomPlayer)
    {
        _gameManager = new GameManager();
        PlayerManagerConnector.ConnectPlayRoom(roomPlayer, _gameManager);
    }

    private void InitCreateGameManager(RoomCreator roomCreator)
    {
        _gameManager = new GameManager();
        PlayerManagerConnector.ConnectCreateRoom(roomCreator, _gameManager);
    }
}
