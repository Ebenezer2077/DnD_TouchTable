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

    public static void LoadObjectInButton(Entity entity, GridButton button)
    {
        var type = entity.basetype;
        var texture = LoadUnitsProvider.LoadUnit(type);
        PlaceObject(button, entity.name, texture);
    }
}