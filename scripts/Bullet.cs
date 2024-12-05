using Godot;
using System;

public partial class Bullet : Area2D
{
	// Velocidad de la bala
	[Export] public float speedBullet = 300.0f;

	// Daño que causa la bala
	[Export] public int damage = 25;

	// Método llamado cuando el nodo está listo
	public override void _Ready()
	{
		// Conectar la señal de colisión con el método OnBodyEntered
		this.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	// Método llamado en cada frame para actualizar la posición de la bala
	public override void _Process(double delta)
	{
		// Calcular la velocidad de la bala
		Vector2 velocity = new Vector2(speedBullet * (float)delta, 0.0f);

		// Invertir la dirección si la bala está rotada 180 grados
		if (RotationDegrees == 180)
		{
			velocity = -velocity;
		}

		// Actualizar la posición de la bala
		Position += velocity;
	}

	// Método llamado cuando la bala colisiona con otro cuerpo
	private void OnBodyEntered(Node body)
	{
		// Comprobar si el cuerpo colisionado es un zombie
		if (body is ZombieCharacter zombie)
		{
			GD.Print("Zombie impactado!");
			zombie.TakeDamage(damage);
			QueueFree(); //Destruir la bala
		}
	}
}
