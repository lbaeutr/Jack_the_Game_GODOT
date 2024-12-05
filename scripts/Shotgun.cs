// Shotgun.cs
using Godot;
using System;
using VideoJuegoJack.scripts;

public partial class Shotgun : Weapon
{
	// Método llamado cuando el nodo está listo
	public override void _Ready()
	{

		ammo = 6; // Establecer la munición inicial a 6
	}

	// Método llamado cuando otro cuerpo entra en el área del arma
	private void _on_shotgun_body_entered(Node body)
	{
		if (body is Player player)
		{
			if (player.currentWeapon is Shotgun)
			{
				player.ammo = ammo; // Recargar munición
				GD.Print("Munición del jugador recargada: " + player.ammo);
			}
			else
			{
				player.SetWeapon(this); // Cambiar el arma del jugador a la escopeta
				player.SetAnimation("idle_shotgun"); // Establecer la animación del jugador
				player.ammo = ammo; // Establecer la munición del jugador a la munición actual del arma
				player.fireRate = fireRate; // Ajustar la cadencia de fuego
				GD.Print("Munición del jugador: " + player.ammo);
			}
			QueueFree(); // Liberar el nodo del arma
		}

	}
}
