using Godot;
using System;

public partial class Meteor : Area2D
{
    private int _speed;
    private int _rotationSpeed;
    private float _directionX;
    private bool _canCollide = true;

    [Signal]
    public delegate void CollisionEventHandler();

    public override void _Ready()
    {
        var rng = new RandomNumberGenerator();

        var width = (int)GetViewport().GetVisibleRect().Size.X;
        var randomX = rng.RandiRange(0, width);
        var randomY = rng.RandiRange(-150, -50);
        Position = new Vector2(randomX, randomY);

        _speed = rng.RandiRange(600, 900);
        _directionX = rng.RandiRange(-1, 1);
        _rotationSpeed = rng.RandiRange(40, 100);
    }

    public override void _Process(double delta)
    {
        Position += new Vector2(_directionX, 0.1f) * _speed * (float)delta;
        RotationDegrees += _rotationSpeed * (float)delta;
    }

    public async void _on_body_entered(Node2D body)
    {
        if (_canCollide && body.IsInGroup("player"))
        {
            EmitSignal(SignalName.Collision);
            Explode();
            GetNode<Sprite2D>("Sprite2D").Hide();
            _canCollide = false;
            await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
            QueueFree();
        }
    }

    public async void _on_area_entered(Area2D area)
    {
        area.QueueFree();
        GetNode<AudioStreamPlayer2D>("ExplosionSound").Play();
        Explode();
        GetNode<Sprite2D>("Sprite2D").Hide();
        _canCollide = false;
        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        QueueFree();
    }

    private void Explode()
    {
        var particles = new CpuParticles2D();
        particles.Emitting = false;
        particles.OneShot = true;
        particles.Explosiveness = 1.0f;
        particles.Direction = Vector2.Zero;
        particles.Spread = 180.0f;
        particles.Gravity = Vector2.Zero;
        particles.InitialVelocityMin = 50.0f;
        particles.InitialVelocityMax = 150.0f;
        particles.ScaleAmountMin = 2.0f;
        particles.ScaleAmountMax = 5.0f;
        particles.Color = new Color(1.0f, 0.5f, 0.0f);
        particles.GlobalPosition = GlobalPosition;
        GetParent().AddChild(particles);
        particles.Emitting = true;
        
        GetTree().CreateTimer(1.0f).Connect("timeout", new Callable(particles, MethodName.QueueFree));
    }
}
