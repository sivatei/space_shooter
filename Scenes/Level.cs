using Godot;
using System;

public partial class Level : Node2D
{
    private int _health = 3;

    private PackedScene _meteorScene = GD.Load<PackedScene>("res://Scenes/meteor.tscn");
    private PackedScene _laserScene = GD.Load<PackedScene>("res://Scenes/laser.tscn");

    public override void _Ready()
    {
        GetTree().CallGroup("ui", "set_health", _health);

        var size = GetViewport().GetVisibleRect().Size;
        var rng = new RandomNumberGenerator();
        foreach (AnimatedSprite2D star in GetNode("Stars").GetChildren())
        {
            var randomX = rng.RandiRange(0, (int)size.X);
            var randomY = rng.RandiRange(0, (int)size.Y);
            star.Position = new Vector2(randomX, randomY);
            
            var randomScale = rng.RandfRange(1f, 2f);
            star.Scale = new Vector2(randomScale, randomScale);
            
            star.SpeedScale = rng.RandfRange(0.6f, 1.4f);
        }
    }

    public void _on_meteor_timer_timeout()
    {
        var meteor = _meteorScene.Instantiate<Area2D>();
        GetNode("Meteors").AddChild(meteor);
        meteor.Connect(Meteor.SignalName.Collision, new Callable(this, MethodName._on_meteor_collision));
    }

    public void _on_meteor_collision()
    {
        GetNode<Player>("Player").ApplyShake();
        _health -= 1;
        GetTree().CallGroup("ui", "set_health", _health);
        
        if (_health <= 0)
        {
            GetTree().CallDeferred("change_scene_to_file", "res://Scenes/game_over.tscn");
        }
    }

    public void _on_player_laser(Vector2 pos)
    {
        GD.Print("Shoooooot");
        var laser = _laserScene.Instantiate<Area2D>();
        GetNode("Lasers").AddChild(laser);
        laser.GlobalPosition = pos;
    }
}
