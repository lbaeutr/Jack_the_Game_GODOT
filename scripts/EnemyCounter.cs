using Godot;
using System;

public partial class EnemyCounter : Label
{
	public override void _Ready()
	{
		GD.Print("🔄 Cargando EnemyCounter. Puntuación actual: " + ScoreManager.Instance.GetScore());

		UpdateScore();

		if (ScoreManager.Instance == null)
		{
			GD.PrintErr("ERROR: ScoreManager no está disponible en EnemyCounter.");
		}
		else
		{
			GD.Print("Conectando EnemyCounter al ScoreManager en Game2.");

			// Conectar al Signal solo si existe
			if (ScoreManager.Instance.HasSignal("ScoreUpdatedEventHandler"))
			{
				ScoreManager.Instance.Connect("ScoreUpdatedEventHandler", new Callable(this, nameof(UpdateScore)));
			}
			else
			{
				GD.PrintErr("ERROR: No se pudo conectar al Signal 'ScoreUpdatedEventHandler'.");
			}
		}
	}


	private void UpdateScore()
	{
		if (ScoreManager.Instance != null)
		{
			Text = "Score: " + ScoreManager.Instance.GetScore();
			GD.Print("Actualizando Label: " + Text);
		}
		else
		{
			GD.PrintErr("ERROR: No se pudo obtener la puntuación en EnemyCounter.");
		}
	}


	public void AddPoints(int points)
	{
		if (ScoreManager.Instance != null)
		{
			ScoreManager.Instance.AddPoints(points);
			UpdateScore();
		}
	}

	public void ResetScore()
	{
		if (ScoreManager.Instance != null)
		{
			ScoreManager.Instance.ResetScore();
			UpdateScore();
		}
	}
}
