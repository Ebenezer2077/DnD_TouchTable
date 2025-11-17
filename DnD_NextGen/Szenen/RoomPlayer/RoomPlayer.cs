using System;
using System.Reflection;
using Godot;

public partial class RoomPlayer : Panel
{
    private TextureRect _map;
    private GridContainer _gridcontainer;
    private LoadRoomMenu _loadRoomMenu;
    private PopupMenu _popupMenu;
    private PanelContainer _loadUnit;
    private ItemList _itemList;
    private (Vector2I, GridButton) _activeButton;
    public Action<Vector2I> ParseGridData;
    public Action<string, Vector2I> ParsePlacedObject;
    public Func<Vector2I, bool> IsCellFreeFunc;
    public override void _Ready()
    {
        _loadUnit = GetNode<PanelContainer>("LoadUnit");
        _itemList = GetNode<ItemList>("LoadUnit/ItemList");
        _itemList.ItemSelected += (index) =>
        {
            _loadUnit.Visible = false;
            var name = _itemList.GetItemText((int)index);
            var texture = _itemList.GetItemIcon((int)index);
            PlaceObject(name, texture);
            _itemList.DeselectAll();
        };
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
        InitLoadUnit();
        InitPopupMenuHandlíng();
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
        var columns = (int)roomData.Item1.GridSize.X;
        var rows = (int)roomData.Item1.GridSize.Y;
        ParseGridData?.Invoke(new Vector2I(rows, columns));
        _map.Texture = roomData.Item2;
        _gridcontainer.Columns = (int)roomData.Item1.GridSize.X;
        _gridcontainer.Position = GridPosition;
        for (var i = 0; i < columns * rows; i++)
        {
            var button = GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
            button.onPressed += (position) =>
            {
                _activeButton.Item2 = button;
                _activeButton.Item1 = new Vector2I(button._position.X, button._position.Y);
                var isCellFree = (bool)IsCellFreeFunc?.Invoke(button._position);
                OpenButtonpopup(position, isCellFree);
            };
            button.CustomMinimumSize = new Vector2(buttonSize, buttonSize);
            button.Playmode = true;
            button._position = new Vector2I(i % rows, i / rows);
            _gridcontainer.AddChild(button);
        }
    }

    private void OpenButtonpopup(Vector2 globalPosition, bool isCellFree)
    {
        InitPopupMenu(isCellFree);
        _popupMenu.Visible = true;
        _popupMenu.Position = new Vector2I((int)globalPosition.X, (int)globalPosition.Y);
    }
    
    private void InitPopupMenu(bool isCellFree)
    {
        _popupMenu.Clear();
        if(isCellFree) _popupMenu.AddItem("PlaceObject", 0);
        else _popupMenu.AddItem("MoveObject", 1);
        _popupMenu.AddItem("PlaceItem", 2);
    }

    private void InitPopupMenuHandlíng()
    {
        _popupMenu.IdPressed += (id) =>
        {
            switch (id)
            {
                case 0:
                    _loadUnit.Visible = true;
                    break;
                case 1:
                    break;
            }

        };
    }

    private void PlaceObject(string name, Texture2D texture)
    {
        _activeButton.Item2.TooltipText = name;
        _activeButton.Item2.Icon = texture;
        ParsePlacedObject?.Invoke(name, _activeButton.Item1);
    }

    private void InitLoadUnit()
    {
        var list = LoadUnitsProvider.LoadAllUnits();
        foreach(var unit in list)
        {
            _itemList.AddItem(unit.Item1, unit.Item2);
        }
    }
}
