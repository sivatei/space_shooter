using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed { get; set; } = 700;
    
    private bool _canShoot = true;
    private float _shakeStrength = 0.0f;
    private float _shakeFade = 5.0f;

    [Signal]
    public delegate void LaserEventHandler(Vector2 pos);

    public void ApplyShake(float randomStrength = 30.0f)
    {
        _shakeStrength = randomStrength;
    }

    public override void _Ready()
    {
        Position = new Vector2(100, 500);
    }

    public override void _Process(double delta)
    {
        Vector2 direction = Input.GetVector("Left", "Right", "forward", "back");
        Velocity = direction * Speed;
        MoveAndSlide();

        if (_shakeStrength > 0)
        {
            _shakeStrength = Mathf.Lerp(_shakeStrength, 0, _shakeFade * (float)delta);
            var rng = new RandomNumberGenerator();
            GetNode<Camera2D>("Camera2D").Offset = new Vector2(rng.RandfRange(-_shakeStrength, _shakeStrength), rng.RandfRange(-_shakeStrength, _shakeStrength));
        }

        if (Input.IsActionJustPressed("shoot") && _canShoot)
        {
            EmitSignal(SignalName.Laser, Position + new Vector2(0, -60));
            _canShoot = false;
            GetNode<Timer>("LaserTimer").Start();
            GetNode<AudioStreamPlayer2D>("LaserSound").Play();
        }
    }

    public void PlayCollisionSound()
    {
        GetNode<AudioStreamPlayer2D>("DamageSound").Play();
    }

    public void _on_laser_timer_timeout()
    {
        _canShoot = true;
    }
}
