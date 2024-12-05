using Godot;
using System;

public partial class Amunnation : Area2D
{
	// Este método se llama cuando el nodo entra en la escena.
	public override void _Ready()
	{

	}

	// Este método se ejecuta cuando otro nodo entra en el área de colisión.
	void _on_body_entered(Node body)
	{
		// Comprobamos si el nodo que entra es el jugador (o cualquier nodo que quieras detectar)
		if (body is Player player)
		{
			// Recargar la munición del jugador al valor máximo
			player.ammo = player.MaxAmmoRevolver;

			// Imprimir la nueva cantidad de munición en pantalla
			GD.Print("Munición del jugador: " + player.ammo);

			// Eliminar el objeto de munición (se destruye a sí mismo)
			QueueFree();
		}
	}

	// Método opcional para controlar el movimiento de la bala si es necesario
	public override void _Process(double delta)
	{
		// TODO completar
	}
}
