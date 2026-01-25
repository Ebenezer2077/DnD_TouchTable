using Godot;

public static class ChangeUnitHelper                                                            //Changes only the frontend
{
        public static void PlaceObject(GridButton targetbutton, string name, Texture2D texture)
    {
        targetbutton.TooltipText = name;
        targetbutton.Icon = texture;
    }

    public static void DeleteObject(GridButton targetButton)
    {
        targetButton.TooltipText = null;
        targetButton.Icon = null;
    }
}