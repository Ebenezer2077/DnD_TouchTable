using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RoomCreator : Panel
{
    private Button SaveRoom;
    private FileDialog fileDialog;
    private LoadRoomMenu loadRoomMenu;
    private TextFieldPopup textFieldPopup;
    private TextFieldPopup LoadUnitTextfieldPopup;
    private GridContainer gridContainer;
    private Button AddColumn;
    private RichTextLabel ColumnCount;
    private Button RemoveColumn;
    private VSlider GridSize;
    private RichTextLabel GridsizeLabel;
    private RichTextLabel GridSizeCount;
    private Button AddRow;
    private RichTextLabel RowCount;
    private Button RemoveRow;
    private Button PositionGrid;
    private Button LoadMap;
    private Button LoadRoom;
    private Button ConfirmPlace;
    private Button ExitButton;
    private Button CancleLoadUnit;
    private TextureRect Map;
    private PanelContainer _loadUnit;
    private ItemList _itemList;
    private GridButton activeButton;

    private int columns = 1;
    private int rows = 1;
    private int buttonSize = 50;
    private bool isGridPositioned = false;
    private bool StickGridToCursor = true;
    private string RoomName = "";
    private int StartingDistance = -1;
    private int StartingSize;
    private Vector2I StretchPositionOne;
    private Vector2I StretchPositionTwo;
    private Dictionary<int, Vector2I> Fingers = new Dictionary<int, Vector2I>();
    public Func<Vector2I, bool> IsCellFreeFunc;
    public Action<Vector2I> DeleteObjectAction;
    public Action<string, string, Vector2I> ParsePlacedObject;
    public Action<Vector2I> ParseGridData;
    public Action<Vector2, int, string, Texture2D> SaveRoomAction;
    public Action<Cell[,]> UpdateCells;
    public override void _Ready()
    {
        _loadUnit = GetNode<PanelContainer>("LoadUnit");
        _itemList = GetNode<ItemList>("LoadUnit/ItemList");
        LoadUnitTextfieldPopup = GetNode<TextFieldPopup>("LoadUnitTextfieldPopup");
        _itemList.ItemSelected += (index) =>
        {
            _loadUnit.Visible = false;
            var basetype = _itemList.GetItemText((int)index);
            var texture = _itemList.GetItemIcon((int)index);
            LoadUnitTextfieldPopup.SetTitel("Load Unit As");
            LoadUnitTextfieldPopup.Visible = true;
            LoadUnitTextfieldPopup.SetText(basetype);
            LoadUnitTextfieldPopup.onConfirm += (name) =>
            {
                ParsePlacedObject?.Invoke(name, basetype, activeButton._position);
                _itemList.DeselectAll();
            };
        };
        loadRoomMenu = GetNode<LoadRoomMenu>("LoadRoomMenu");
        loadRoomMenu.LoadRoom += LoadRoomFunc;
        textFieldPopup = GetNode<TextFieldPopup>("TextFieldPopup");
        Map = GetNode<TextureRect>("TextureRect");
        SaveRoom = GetNode<Button>("MarginContainer/VBoxContainer4/SaveRoom");
        SaveRoom.Pressed += () =>
        {
            if (RoomName.Equals("")) textFieldPopup.Visible = true;
            else SaveRoomAction?.Invoke(gridContainer.GlobalPosition, buttonSize, RoomName, Map.Texture);
        };
        fileDialog = GetNode<FileDialog>("FileDialog");
        fileDialog.Access = FileDialog.AccessEnum.Filesystem;
        fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        fileDialog.FileSelected += (path) =>
        {
            var image = new Image();
            var err = image.Load(path);
            Map.Texture = ImageTexture.CreateFromImage(image);
        };
        gridContainer = GetNode<GridContainer>("GridContainer");
        AddColumn = GetNode<Button>("MarginContainer/VBoxContainer/AddColumn");
        AddColumn.Pressed += () => AdjustColumns(1);
        ColumnCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/RichTextLabel");
        RemoveColumn = GetNode<Button>("MarginContainer/VBoxContainer/RemoveColumn");
        RemoveColumn.Pressed += () => AdjustColumns(-1);
        GridSize = GetNode<VSlider>("MarginContainer/VBoxContainer3/HBoxContainer/VSlider");
        GridSize.ValueChanged += (value) => AdjustGridSize((int)value);
        GridsizeLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer3/HBoxContainer/RichTextLabel");
        GridSizeCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer3/RichTextLabel");
        AdjustGridSize(20);
        AddRow = GetNode<Button>("MarginContainer/VBoxContainer2/AddRow");
        AddRow.Pressed += () => AdjustRows(1);
        RowCount = GetNode<RichTextLabel>("MarginContainer/VBoxContainer2/RichTextLabel");
        RemoveRow = GetNode<Button>("MarginContainer/VBoxContainer2/RemoveRow");
        RemoveRow.Pressed += () => AdjustRows(-1);
        gridContainer.Columns = columns;
        PositionGrid = GetNode<Button>("MarginContainer/VBoxContainer4/Position Grid");
        PositionGrid.Pressed += PositionGridFunc;
        LoadMap = GetNode<Button>("MarginContainer/VBoxContainer4/Load Map");
        LoadMap.Pressed += () =>
        {
            fileDialog.Visible = true;
        };
        LoadRoom = GetNode<Button>("MarginContainer/VBoxContainer4/LoadRoom");
        LoadRoom.Pressed += () => {
            loadRoomMenu.Visible = true;
            loadRoomMenu.InitRoomList();
        };
        textFieldPopup.onConfirm += (s) => {
            RoomName = s;
            SaveRoomAction?.Invoke(gridContainer.GlobalPosition, buttonSize, RoomName, Map.Texture);
        };
        ConfirmPlace = GetNode<Button>("MarginContainer/VBoxContainer4/Confirm");
        ConfirmPlace.Pressed += PositionGridFunc;
        ExitButton = GetNode<Button>("MarginContainer/Exit");
        ExitButton.Pressed += () => GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
        CancleLoadUnit = GetNode<Button>("LoadUnit/CancelLoadUnit");
        CancleLoadUnit.Pressed += () => _loadUnit.Visible = false;
        InitLoadUnit();
        ParseGridData?.Invoke(new Vector2I(1,1));
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Back")) GetTree().ChangeSceneToFile("res://Szenen/MainMenu/MainMenu.tscn");
        
        if (isGridPositioned && @event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.IsPressed()) StickGridToCursor = false;

        if(@event is InputEventScreenTouch touch && isGridPositioned) {
            if(touch.IsPressed())
            {
                Fingers[touch.Index] = (Vector2I)touch.Position;
                if(touch.Index == 1)
                {
                    StartingDistance = (int)Fingers[0].DistanceTo(Fingers[1]);
                    StartingSize = buttonSize;
                }
            }
            if(touch.IsReleased()) 
            {
                Fingers.Remove(touch.Index);
                PositionGridFunc();
            }
        }
        if(@event is InputEventScreenDrag drag && isGridPositioned)
        {
            if(drag.Index == 0)
            {
                StretchPositionOne = new ((int)drag.Position.X, (int)drag.Position.Y);
                AdjustGridPosition(StretchPositionOne);
            }
            if(drag.Index == 1)
            {
                StretchPositionTwo = new ((int)drag.Position.X, (int)drag.Position.Y);
            }
            if(Fingers.ContainsKey(1) && Fingers.ContainsKey(0))
            {
                var newGridSize = StartingSize*(StretchPositionOne.DistanceTo(StretchPositionTwo) / StartingDistance);
                if(newGridSize == 0) newGridSize = 1;                   //minimum size of 1 pixel
                AdjustGridSize((int)newGridSize);
            }
        }
    }
    private GridButton GetButtonFromPosition(Vector2I position)
    {
        return (GridButton)gridContainer.GetChildren().First(x => ((GridButton)x)._position.Equals(position));
    }

    public void PlaceObjectInUI(string name, string basetype, Vector2I position)
    {
        var texture = LoadUnitsProvider.LoadUnit(basetype);
        var button = GetButtonFromPosition(position);
        ChangeUnitHelper.PlaceObject(button, name, texture);
    }
    public override void _Process(double delta)
    {
        if (isGridPositioned && StickGridToCursor)
        {
            AdjustGridPosition(GetViewport().GetMousePosition());
        }
    }
    private void AdjustGridPosition(Vector2 Position)
    {
        gridContainer.Position = Position;
    }
    private void LoadRoomFunc(string name)//maybe push loading to gameManager?
    {
        loadRoomMenu.Visible = false;
        var roomData = LoadRoomTemplatesProvider.LoadRoom(name);
        SaveRoom.Disabled = roomData.isDefault;
        var room = roomData.room;
        RoomName = room.Name;
        InitColumns((int)room.GridSize.X);
        InitRows((int)room.GridSize.Y);
        AdjustGridSize(room.ButtonSize);
        AdjustGridPosition(room.GridPosition);
        Map.Texture = roomData.background;
        RefreshContainer(gridContainer);
        LoadUnitUI(room.Cells);
        UpdateCells?.Invoke(room.Cells);
    }

    private void LoadUnitUI(Cell[,] cells)
    {
        foreach(var cell in cells)
        {
            if(cell.Object != null)
            {
                PlaceObjectInUI(cell.Object.name, cell.Object.basetype, new Vector2I((int)cell.position.X, (int)cell.position.Y));
            }
        }
    }
    private void PositionGridFunc()
    {
        isGridPositioned = !isGridPositioned;
        StickGridToCursor = true;
        ToggleControlls(!isGridPositioned);
        ToggleGridButtonInput(gridContainer, isGridPositioned);
    }

    private void ToggleGridButtonInput(GridContainer gridContainer, bool enable)
    {
        foreach(var button in gridContainer.GetChildren())
        {
            ((GridButton)button).MouseFilter = enable ? MouseFilterEnum.Ignore : MouseFilterEnum.Pass;
        }
    }

    private void ToggleControlls(bool enable)
    {
        AddColumn.Visible = enable;
        ColumnCount.Visible = enable;
        RemoveColumn.Visible = enable;
        GridSize.Visible = enable;
        GridsizeLabel.Visible = enable;
        GridSizeCount.Visible = enable;
        AddRow.Visible = enable;
        RowCount.Visible = enable;
        RemoveRow.Visible = enable;
        PositionGrid.Visible = enable;
        ConfirmPlace.Visible = !enable;
    }
    private void InitRows(int amount)
    {
        rows = amount;
        RowCount.Text = rows.ToString();
    }
    private void InitColumns(int amount)
    {
        columns = amount;
        ColumnCount.Text = columns.ToString();
        gridContainer.Columns = columns;
    }
    private void AdjustRows(int amount)
    {
        rows += amount;
        RowCount.Text = rows.ToString();
        RefreshContainer(gridContainer);
    }
    private void AdjustColumns(int amount)
    {
        columns += amount;
        ColumnCount.Text = columns.ToString();
        gridContainer.Columns = columns;
        RefreshContainer(gridContainer);
    }

    private void AdjustGridSize(int size)
    {
        buttonSize = size;
        GridSize.Value = size;
        GridSizeCount.Text = size.ToString();
        foreach (var button in gridContainer.GetChildren())
        {
            ((GridButton)button).CustomMinimumSize = new Vector2(buttonSize, buttonSize);
        }
    }

    private void RefreshContainer(GridContainer gridContainer)
    {
        ParseGridData?.Invoke(new Vector2I(columns, rows));
        foreach (var button in gridContainer.GetChildren())
        {
            gridContainer.RemoveChild(button);
        }
        for (int i = 0; i < rows * columns; i++)
        {
            var button = GD.Load<PackedScene>("res://Szenen/GridButton/GridButton.tscn").Instantiate<GridButton>();
            button.Playmode = false;
            button._loadUnit = _loadUnit;
            button.DeleteObjectAction += (GridButton gbutton) =>
            {
                ChangeUnitHelper.DeleteObject(gbutton);
                DeleteObjectAction?.Invoke(gbutton._position);
            };
            button.onPressed += (position) =>
            {
                activeButton = button;
                var isCellFree = (bool)IsCellFreeFunc?.Invoke(button._position);
                button.OpenButtonpopup(position, isCellFree);
            };
            button._position = new Vector2I(i % columns, i / columns);
            button.CustomMinimumSize = new Vector2(buttonSize, buttonSize);
            gridContainer.AddChild(button);
        }
    }
    private void InitLoadUnit()
    {
        var list = LoadUnitsProvider.LoadAllUnits();
        foreach(var unit in list)
        {
            _itemList.AddItem(unit.basetype, unit.icon);
        }
    }
}
