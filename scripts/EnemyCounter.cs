using Godot;
using System;

public partial class EnemyCounter : Label
{
	private int score = 0; // Puntuación inicial

	public override void _Ready()
	{
		UpdateScore();
	}

	private void UpdateScore()
	{
		Text = "Score: " + score;
	}

	public void AddPoints(int points)
	{
		score += points;
		UpdateScore();
	}

	public void ResetScore()
	{
		score = 0;
		UpdateScore();
	}
}
