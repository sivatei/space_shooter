extends Area2D

var speed: int
var rotation_speed: int
var direction_x: float

signal collision
var can_collide := true

func _ready():
	var rng := RandomNumberGenerator.new()
		
	# start position
	var width = int(get_viewport().get_visible_rect().size.x)
	var random_x = rng.randi_range(0, width)
	var random_y = rng.randi_range(-150, -50)
	position = Vector2(random_x, random_y)
	
	speed = rng.randi_range(600, 900)
	direction_x = rng.randi_range(-1, 1)
	rotation_speed = rng.randi_range(40, 100)
	
func _process(delta):
	position += Vector2(direction_x, 0.1) * speed * delta
	rotation_degrees += rotation_speed * delta

func _on_body_entered(body: Node2D) -> void:
	if can_collide and body.is_in_group("player"):
		collision.emit()
		$Sprite2D.hide()
		can_collide = false
		await get_tree().create_timer(1).timeout
		queue_free()

func _on_area_entered(area: Area2D) -> void:
	area.queue_free()
	$ExplosionSound.play()
	$Sprite2D.hide()
	can_collide = false
	await get_tree().create_timer(1).timeout
	queue_free()
