namespace VideoJuegoJack.scripts;

// Weapon.cs
using Godot;
using System;

/*
 * Clase abstracta que define las propiedades básicas y comunes de un arma en el juego.
 * Esta clase sirve como una plantilla para crear diferentes tipos de armas
 */
public abstract partial class Weapon : Area2D
{
	// Cadencia de fuego del arma en segundos
	[Export] public float fireRate;

	// Munición actual del arma
	[Export] public int ammo;

	// Munición máxima que puede tener el arma
	[Export] public int maxAmmo;

	// Daño que causa el arma
	[Export] public int damage;
}
