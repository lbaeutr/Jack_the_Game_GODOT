using Godot;
using System;

public partial class ScoreManager : Node
{
	public static ScoreManager Instance { get; private set; }
	private int score = 0;

	// Definir el Signal
	[Signal]
	public delegate void ScoreUpdatedEventHandler();

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
			SetProcess(false); // Evita que se procese dos veces si se duplica

			// Asegurar que el Signal esté registrado antes de usarse
			AddUserSignal("ScoreUpdatedEventHandler");

			GD.Print("ScoreManager cargado. Puntuación actual: " + score);
		}
		else
		{
			GD.Print("ScoreManager ya existe. Eliminando duplicado.");
			QueueFree(); // Evita que se creen muchos ScoreManager
		}
	}



	public void AddPoints(int points)
	{
		score += points;
		GD.Print("Puntuación actual: " + score);

		// Verificar si el Signal existe antes de emitirlo
		if (HasSignal("ScoreUpdatedEventHandler"))
		{
			EmitSignal("ScoreUpdatedEventHandler");
		}
		else
		{
			GD.PrintErr("ERROR: El Signal 'ScoreUpdatedEventHandler' no está registrado.");
		}
	}

	public void ResetScore()
	{
		score = 0;
		EmitSignal("ScoreUpdatedEventHandler");
	}

	public int GetScore()
	{
		return score;
	}
}
