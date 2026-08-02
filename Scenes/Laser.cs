using Godot;
using System;

public partial class Laser : Area2D
{
    [Export] public int Speed { get; set; } = 500;

    public override void _Ready()
    {
        var tween = CreateTween();
        tween.TweenProperty(GetNode("Sprite2D"), "scale", new Vector2(1, 1), 0.2f).From(new Vector2(0, 0));
    }

    public override void _Process(double delta)
    {
        Position -= new Vector2(0, Speed * (float)delta);
    }
}
