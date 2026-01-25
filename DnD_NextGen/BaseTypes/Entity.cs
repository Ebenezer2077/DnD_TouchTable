using Godot;

public class Entity(string Name, string Basetype, Texture2D Icon)
{
    public string name {get;} = Name;
    public string basetype {get;} = Basetype;
    public Texture2D icon {get;} = Icon;
}