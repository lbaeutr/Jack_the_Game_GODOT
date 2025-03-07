extends Control

@onready var score_label = $ScoreLabel  # Asegura que esta ruta es correcta
@onready var score_display = $ScoreDisplay  # 📌 Label o RichTextLabel para mostrar las puntuaciones
@onready var initials_input = $InitialsInput
@onready var save_button = $SaveButton
@onready var api_request = $APIRequest  # 📌 Nodo HTTPRequest para hacer las peticiones

var API_URL = "https://api-web-asp-net.onrender.com/api/Jugadores"  # 📌 URL de tu API

func _ready():
	load_high_scores()  # 📌 Cargar las puntuaciones desde la API al iniciar
	var final_score = ScoreManager.GetScore()
	score_label.text = "Puntuación Final: " + str(final_score)

# 📌 Cargar las puntuaciones desde la API (sin cambios)
func load_high_scores():
	api_request.request(API_URL + "/top5")  # 📌 GET a la API para obtener puntuaciones

# 📌 Guardar una nueva puntuación en la API
func save_high_score(initials: String, final_score: int):
	var json_data = { "siglas": initials, "puntuacion": final_score }
	var body = JSON.stringify(json_data)
	var headers = ["Content-Type: application/json"]
	
	api_request.request(API_URL, headers, HTTPClient.METHOD_POST, body)  # 📌 POST a la API
	
	# ✅ Esperar 5 segundos antes de actualizar la lista, pero solo si el envío es exitoso
	await get_tree().create_timer(5.0).timeout  
	load_high_scores()  

# 📌 Guardar la puntuación al presionar el botón
func _on_save_button_pressed():
	var initials = initials_input.text.strip_edges()
	if initials.length() < 1:
		initials = "XXX"

	save_high_score(initials, ScoreManager.GetScore())  # 📌 Enviar la puntuación a la API

# 📌 Manejar la respuesta de la API y actualizar puntuaciones
func _on_api_request_request_completed(result, response_code, headers, body):
	if response_code == 200:
		var json = JSON.parse_string(body.get_string_from_utf8())

		if json and json is Array:
			var formatted_scores = []
			for entry in json.slice(0, 5):  # 📌 Tomar solo las 5 mejores puntuaciones
				if "siglas" in entry and "puntuacion" in entry:
					formatted_scores.append(entry["siglas"] + " -> " + str(entry["puntuacion"]))

			# 📌 Convertir la lista en texto plano y mostrarlo en `score_display`
			score_display.text = "🏆 TOP 5 PUNTUACIONES:\n" + "\n".join(formatted_scores)
		else:
			score_display.text = "⚠ No hay puntuaciones guardadas aún."
	else:
		score_display.text = "⌛ Cargando puntuaciones..."
		await get_tree().create_timer(3.0).timeout  # ✅ Espera 3 segundos y vuelve a intentarlo
		load_high_scores()  # 📌 Intenta de nuevo cargar los datos

# 📌 Reiniciar el juego al presionar "Reiniciar"
func _on_reiniciar_pressed():
	get_tree().change_scene_to_file("res://scenes/game.tscn")

# 📌 Salir del juego
func _on_exit_pressed():
	get_tree().quit()
