using Godot;
using System;

public partial class UI : CanvasLayer
{
    private static Texture2D _image = GD.Load<Texture2D>("res://Space shooter asset/kenney_space-shooter-remastered/PNG/UI/playerLife1_blue.png");
    private int _timeElapsed = 0;

    public void set_health(int amount)
    {
        var hbox = GetNode<HBoxContainer>("MarginContainer2/HBoxContainer");
        foreach (Node child in hbox.GetChildren())
        {
            child.QueueFree();
        }

        for (int i = 0; i < amount; i++)
        {
            var textRect = new TextureRect();
            textRect.Texture = _image;
            textRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
            textRect.CustomMinimumSize = new Vector2(50, 50);
            hbox.AddChild(textRect);
        }
    }

    public void _on_score_timer_timeout()
    {
        _timeElapsed += 1;
        GetNode<Label>("MarginContainer/Label").Text = _timeElapsed.ToString();
        Global.Score = _timeElapsed;
    }
}
