using Godot;
using System;

public partial class GameOver : Control
{
    [Export] public PackedScene LevelScene { get; set; }

    public override void _Ready()
    {
        var label = GetNode<Label>("CenterContainer/VBoxContainer/Label2");
        label.Text = label.Text + Global.Score.ToString();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("shoot"))
        {
            GetTree().ChangeSceneToPacked(LevelScene);
        }
    }
}
