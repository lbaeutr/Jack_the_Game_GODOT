// Revolver.cs

using Godot;
using System;
using VideoJuegoJack.scripts;

public partial class Revolver : Weapon
{
	// Método llamado cuando el nodo está listo
	public override void _Ready()
	{
		ammo = 6; // Establecer la munición inicial a 6
	}

	// Método llamado cuando otro cuerpo entra en el área del arma
	public void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			if (player.currentWeapon is Revolver)
			{
				player.ammo = maxAmmo; // Recargar munición
				GD.Print("Munición del jugador recargada: " + player.ammo);
			}
			else
			{
				player.SetWeapon(this); // Cambiar el arma del jugador al revólver
				player.SetAnimation("idle_revolver"); // Establecer la animación del jugador
				player.ammo = ammo; // Establecer la munición del jugador a la munición actual del arma
				GD.Print("Munición del jugador: " + player.ammo);
			}

			QueueFree(); // Liberar el nodo del arma
		}
	}
}
