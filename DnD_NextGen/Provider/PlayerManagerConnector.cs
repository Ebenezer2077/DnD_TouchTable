using System.Data;

public static class PlayerManagerConnector
{
    public static void ConnectPlayRoom(RoomPlayer roomPlayer, GameManager gameManager)
    {
        roomPlayer.ParseGridData += gameManager.InitCells;
        roomPlayer.ParsePlacedObject += gameManager.PlaceObject;
        roomPlayer.IsCellFreeFunc += gameManager.IsCellFree;
        roomPlayer.MoveObject += gameManager.MoveObject;
        roomPlayer.DeleteObjectAction += gameManager.DeleteObject;
        roomPlayer.UpdateCells += gameManager.LoadCells;
        gameManager.SwapObjectsAction += roomPlayer.SwapObjects;
    }

    public static void ConnectCreateRoom(RoomCreator roomCreator, GameManager gameManager)
    {
        roomCreator.ParseGridData += gameManager.InitCells;
        roomCreator.IsCellFreeFunc += gameManager.IsCellFree;
        roomCreator.DeleteObjectAction += gameManager.DeleteObject;
        roomCreator.ParsePlacedObject += gameManager.PlaceObject;
        roomCreator.SaveRoomAction += gameManager.SaveRoom;
    }
}