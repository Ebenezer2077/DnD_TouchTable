using Godot;
using System;

public partial class TextFieldPopup : PanelContainer
{
    private Button Cancel;
    private Button Confirm;
    private LineEdit lineEdit;
    public Action onCancel;
    public Action<string> onConfirm;
    public override void _Ready()
    {
        Cancel = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/Cancel");
        Cancel.Pressed += onCancel;
        Confirm = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/Confirm");
        Confirm.Pressed += () => onConfirm?.Invoke(lineEdit.Text);
        lineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/LineEdit");
    }

}
