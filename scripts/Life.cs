using Godot;
using System;

public partial class Life : Area2D
{
	// Este método se llama cuando el nodo entra en la escena.
	public override void _Ready()
	{

	}

	// Este método se ejecuta cuando otro nodo entra en el área de colisión.
	private void _on_body_entered(Player body)
	{
		// Comprobamos si el nodo que entra es el jugador (o cualquier nodo que quieras detectar)
		if (body is Player player)
		{
			// Aumentar la salud del jugador
			player.Health += 10;

			// Asegurarse de que la vida del jugador no supere los 100 puntos
			if (player.Health > 100)
			{
				player.Health = 100;
			}

			GD.Print("Vida del jugador: " + player.Health);

			QueueFree();
		}
	}
}
