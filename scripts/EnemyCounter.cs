using Godot;
using System;

public partial class EnemyCounter : Label
{
	private int enemyCount = 7; // Contador de enemigos

	public override void _Ready()
	{
		UpdateLabel();
	}

	private void UpdateLabel()
	{
		Text = "Enemies: " + enemyCount;
	}

	public void IncreaseCount()
	{
		enemyCount++;
		UpdateLabel();
	}

	public void DecreaseCount()
	{
		enemyCount = Mathf.Max(0, enemyCount - 1); // Evita valores negativos
		UpdateLabel();
	}

	public void ResetCount()
	{
		enemyCount = 0;
		UpdateLabel();
	}
}
