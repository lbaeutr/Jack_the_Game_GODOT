using Godot;
using System;

public partial class CambioNivel : Node
{
	private Node currentScene;
	private string routeScene; // Ruta de la escena actual
	private int zombiesVivos = 0; // Contador de zombis vivos
	private bool isFirstScene; // Para saber si es la primera escena o no

	// Método llamado cuando el nodo está listo
	public override void _Ready()
	{
		// Si es la primera escena, la lógica de inicio estará activa.
		isFirstScene = (GetTree().CurrentScene.Name == "Game");
		GD.Print(isFirstScene);
	}

	// Método para registrar un zombi
	public void RegisterZombie()
	{
		zombiesVivos++; // Incrementa el contador de zombis vivos
		GD.Print($"Zombis vivos: {zombiesVivos}");
	}

	// Método para notificar cuando un zombi muere
	public void NotifyZombieDeath(ZombieCharacter zombie)
	{
		zombiesVivos--; // Decrementa el contador de zombis vivos
		GD.Print($"Zombi muerto. Zombis vivos: {zombiesVivos}");

		// Si no quedan zombis vivos y es la primera escena, cambiar de escena
		if (zombiesVivos <= 0)
		{
			if (isFirstScene)
			{
				GD.Print("¡Todos los zombis han muerto! Cambiando a la siguiente escena...");
				// Cambiar a la siguiente escena
				GetTree().CallDeferred("change_scene_to_file", "res://scenes/game2.tscn"); // Ruta de la nueva escena
				isFirstScene = false; // Asegura que no se ejecute más de una vez
			}
			else
			{
				// Si ya no es la primera escena, mostrar la pantalla final
				ShowFinalScreen();
			}
		}
	}

	// Método para cargar una nueva escena
	public void LoadScene(string scenePath)
	{
		routeScene = scenePath;

		if (currentScene != null)
		{
			currentScene.CallDeferred("queue_free"); // Limpia la escena actual de manera segura.
		}

		var nextScene = GD.Load<PackedScene>(scenePath);
		if (nextScene != null)
		{
			currentScene = nextScene.Instantiate(); // Instancia la nueva escena.
			GetTree().ChangeSceneToPacked(nextScene); // Añadir la nueva escena.
		}
		else
		{
			GD.PrintErr($"No se pudo cargar la escena en {scenePath}.");
		}
	}

	// Método para mostrar la pantalla final
	public void ShowFinalScreen()
	{
		GD.Print("Mostrando la pantalla final...");
		var finalScreenScene = GD.Load<PackedScene>("res://scenes/you_win.tscn"); // Ruta de la pantalla final
		if (finalScreenScene != null)
		{
			GetTree().CallDeferred("change_scene_to_packed", finalScreenScene);
		}
		else
		{
			GD.PrintErr("No se pudo cargar la pantalla final.");
		}
	}
}
