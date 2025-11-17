using Godot;
using System;

public partial class TextFieldPopup : PanelContainer
{
    private Button Cancel;
    private Button Confirm;
    private LineEdit lineEdit;
    public Action<string> onConfirm;
    public override void _Ready()
    {
        Cancel = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/Cancel");
        Cancel.Pressed += () => {
            Visible = false;
        };
        Confirm = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/Confirm");
        Confirm.Pressed += () => {
            onConfirm?.Invoke(lineEdit.Text);
            Visible = false;
        };
        lineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/LineEdit");
    }

    public void SetText(string text)
    {
        lineEdit.Text = text;
    }
}
