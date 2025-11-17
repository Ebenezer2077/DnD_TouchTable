using System.Data;

public static class PlayerManagerConnector
{
    public static void Connect(RoomPlayer roomPlayer, GameManager gameManager)
    {
        roomPlayer.ParseGridData += gameManager.InitCells;
        roomPlayer.ParsePlacedObject += gameManager.PlaceObject;
        roomPlayer.IsCellFreeFunc += gameManager.IsCellFree;
        roomPlayer.MoveObject += gameManager.MoveObject;
        roomPlayer.DeleteObjectAction += gameManager.DeleteObject;
        gameManager.SwapObjectsAction += roomPlayer.SwapObjects;
    }
}