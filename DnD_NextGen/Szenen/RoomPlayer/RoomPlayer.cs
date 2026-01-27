using System;
using System.Linq;
using Godot;

public partial class RoomPlayer : Panel
{
    private bool _isMovingActionActive = false;
    private TextureRect _map;
    private GridContainer _gridcontainer;
    private LoadRoomMenu _loadRoomMenu;
    private PopupMenu _popupMenu;
    private TextFieldPopup _textFieldPopup;
    private PanelContainer _loadUnit;
    private ItemList _itemList;
    private GridButton _activeButton;
    public Action<Vector2I> DeleteObjectAction;
    public Action<Vector2I> ParseGridData;
    public Action<Entity, Vector2I> ParsePlacedObject;
    public Action<Vector2I> MoveObject;
    public Func<Vector2I, bool> IsCellFreeFunc;
    public Action<Cell[,]> UpdateCells;
    public override void _Ready()
    {
        _textFieldPopup = GetNode<TextFieldPopup>("TextFieldPopup");
        _loadUnit = GetNode<PanelContainer>("LoadUnit");
        _itemList = GetNode<ItemList>("LoadUnit/ItemList");
        _itemList.ItemSelected += (index) =>
        {
            _loadUnit.Visible = false;
            var basetype = _itemList.GetItemText((int)index);
            var texture = _itemList.GetItemIcon((int)index);
            _textFieldPopup.Visible = true;
            _textFieldPopup.SetText(basetype);
            _textFieldPopup.onConfirm += (name) =>
            {
                var entity = new Entity(name, basetype);
                ChangeUnitHelper.PlaceObject(_activeButton, entity.name, texture);
                ParsePlacedObject?.Invoke(entity, _activeButton._position);
                _itemList.DeselectAll();
            };
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
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Back")) GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
    }

    private void LoadRoom(string name)//need to load objects 
    {
        var roomData = LoadRoomTemplatesProvider.LoadRoom(name);
        var buttonSize = roomData.room.ButtonSize;
        var GridPosition = roomData.room.GridPosition;
        var columns = (int)roomData.room.GridSize.X;
        var rows = (int)roomData.room.GridSize.Y;
        var dimension = new Vector2I(rows, columns);
        ParseGridData?.Invoke(dimension);
        UpdateCells?.Invoke(roomData.room.Cells);
        _map.Texture = roomData.background;
        _gridcontainer.Columns = (int)roomData.room.GridSize.X;
        _gridcontainer.Position = GridPosition;
        for (var i = 0; i < columns * rows; i++)
        {
            var button = GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
            var cell = roomData.room.Cells[i % dimension.X, i / dimension.X];
            if(cell.Object != null) ChangeUnitHelper.LoadObjectInButton(cell.Object, button);
            button._loadUnit = _loadUnit;
            button.MoveObject += (Vector2I position) =>
            {
                MoveObject?.Invoke(position);
                _isMovingActionActive = !_isMovingActionActive;
            };
            button.DeleteObjectAction += (GridButton gButton) =>
            {
                ChangeUnitHelper.DeleteObject(gButton);
                DeleteObjectAction?.Invoke(gButton._position);
            };
            button.onPressed += (position) =>
            {
                _activeButton = button;
                _activeButton._position = new Vector2I(button._position.X, button._position.Y);
                if (_isMovingActionActive)
                {
                    MoveObject?.Invoke(_activeButton._position);
                    _isMovingActionActive = !_isMovingActionActive;
                    return;
                }
                var isCellFree = (bool)IsCellFreeFunc?.Invoke(button._position);
                button.OpenButtonpopup(position, isCellFree);
            };
            button.CustomMinimumSize = new Vector2(buttonSize, buttonSize);
            button.Playmode = true;
            button._position = new Vector2I(i % rows, i / rows);
            _gridcontainer.AddChild(button);
        }
    }

    public void SwapObjects(Vector2I from, Vector2I to)
    {
        var fromButton = _gridcontainer.GetChildren().OfType<GridButton>().Where(x => x._position.X == from.X && x._position.Y == from.Y).First();
        var toButton = _gridcontainer.GetChildren().OfType<GridButton>().Where(x => x._position.X == to.X && x._position.Y == to.Y).First();

        var holder = (name: fromButton.TooltipText, texture: fromButton.Icon);
        ChangeUnitHelper.PlaceObject(fromButton, toButton.TooltipText, toButton.Icon);
        ChangeUnitHelper.PlaceObject(toButton, holder.name, holder.texture);
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
