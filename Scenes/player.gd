extends CharacterBody2D

@export var speed: int = 700
var can_shoot: bool = true

signal laser(pos)

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	position = Vector2(100,500)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	var direction = Input.get_vector("Left", "Right", "forward", "back")
	velocity = speed * direction
	move_and_slide()
	
	# shoot input device
	if Input.is_action_just_pressed("shoot"):
		laser.emit(position + Vector2(0, -60))
		can_shoot = false
		$LaserTimer.start()
		$LaserSound.play()
		
func play_collision_sound():
	$DamageSound.play()


func _on_laser_timer_timeout() -> void:
	can_shoot = true
