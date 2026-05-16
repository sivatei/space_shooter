extends CanvasLayer

static var image = load("res://Space shooter asset/kenney_space-shooter-remastered/PNG/UI/playerLife1_blue.png")
var time_elapsed := 0

func set_health(amount):
	# remove children from HBoxContainer
	for child in $MarginContainer2/HBoxContainer.get_children():
		child.queue_free()
	# create new children by amount of health
	for i in amount:
		var text_rect = TextureRect.new()
		text_rect.texture = image
		text_rect.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_CENTERED
		text_rect.custom_minimum_size = Vector2(50, 50)
		$MarginContainer2/HBoxContainer.add_child(text_rect)

func _on_score_timer_timeout() -> void:
	time_elapsed += 1
	$MarginContainer/Label.text = str(time_elapsed)
	Global.score = time_elapsed
