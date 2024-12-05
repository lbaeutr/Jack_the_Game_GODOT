using Godot;
using System;
using VideoJuegoJack.scripts;

public partial class Player : CharacterBody2D
{
	// Constante para la velocidad de movimiento del jugador
	public const float Speed = 130.0f;

	// Velocidad de salto del jugador
	public float JumpVelocity = -300.0f;

	// Velocidad de la bala disparada
	[Export] public float speedBullet = 250.0f;

	// Munición actual del jugador
	[Export] public int ammo = 0;

	// Máxima munición del revolver
	[Export] public int MaxAmmoRevolver = 6;

	// Salud del jugador
	[Export] public int Health = 100;

	// Tasa de disparo del jugador
	[Export] public float fireRate = 2.0f;

	private AnimatedSprite2D animation;
	private PackedScene bullet;
	private Timer fireRateTimer;
	private Timer deathTimer;
	public Weapon currentWeapon;
	private bool isDead = false;
	private Vector2 knockBack;

	// Método llamado cuando el nodo está listo
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		bullet = GD.Load<PackedScene>("res://scenes/bullet.tscn");

		// Obtén los timers definidos en el editor
		fireRateTimer = GetNode<Timer>("FireRateTimer");
		deathTimer = GetNode<Timer>("DeathTimer");
	}

	// Método llamado en cada frame de física
	public override void _PhysicsProcess(double delta)
	{
		// Si la salud es 0 o el jugador está muerto, no permitir movimiento
		if (Health <= 0 || isDead)
		{
			this.Position = new Vector2(this.Position.X, -8);
			return;
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector2 velocity = Velocity;

		// Si el jugador no está en el suelo, aplicar gravedad
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			animation.Play("Jump1");
			animation.FlipH = direction.X < 0;
		}
		else
		{
			// Si se presiona la tecla de salto, aplicar velocidad de salto
			if (Input.IsActionJustPressed("ui_up"))
			{
				velocity.Y = JumpVelocity;
			}

			// Si hay dirección de movimiento, mover al jugador
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
				animation.FlipH = velocity.X < 0;
				PlayAnimation("walk");
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				PlayAnimation("idle");
			}
		}

		// Si se presiona la tecla de disparo y el temporizador de tasa de disparo está detenido, disparar
		if (Input.IsActionJustPressed("shoot") && fireRateTimer.IsStopped())
		{
			Shoot();
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	// Método para disparar una bala
	private void Shoot()
	{
		// Verificar si el FireRateTimer está detenido
		if (!fireRateTimer.IsStopped())
		{
			GD.Print("No puedes disparar aún. Espera al siguiente disparo.");
			return;
		}

		if (ammo > 0)
		{
			Bullet instBullet = (Bullet)bullet.Instantiate();
			if (animation.FlipH)
			{
				instBullet.Position = this.GlobalPosition + new Vector2(-21, -10);
			}
			else
			{
				instBullet.Position = this.GlobalPosition + new Vector2(21, -10);
			}

			instBullet.RotationDegrees = this.RotationDegrees;
			instBullet.speedBullet = animation.FlipH ? -speedBullet : speedBullet;
			PlayAnimation("fire");

			GetParent().AddChild(instBullet);
			GD.Print("Disparo");

			ammo--;
			GD.Print("Munición restante: " + ammo);

			fireRateTimer.Start(fireRate);
		}
		else
		{
			GD.Print("Sin munición");
		}
	}

	// Método para recibir daño
	public void TakeDamage(int damage, Vector2 knockBackDirection)
	{
		Health -= damage;
		GD.Print($"Player took {damage} damage. Health is now {Health}.");

		knockBack = knockBackDirection;

		if (Health <= 0)
		{
			Health = 0;
			Die();
		}
	}

	// Método para manejar la muerte del jugador
	private void Die()
	{
		GD.Print("El jugador ha muerto");
		isDead = true;
		animation.Play("Die");
		deathTimer.Start();
	}

	// Método llamado cuando el temporizador de muerte se agota
	private void OnDeathTimerTimeout()
	{
		GD.Print("El jugador ha muerto");
		QueueFree();
		GetTree().ReloadCurrentScene();
	}

	// Método llamado cuando la animación termina
	private void OnAnimationFinished()
	{
		if (animation.Animation == "Die")
		{
			animation.QueueFree();
		}
	}

	// Método para cambiar el arma del jugador
	public void SetWeapon(Weapon weapon)
	{
		currentWeapon = weapon;
		fireRate = weapon.fireRate;
		ammo = weapon.maxAmmo;
		GD.Print("Arma cambiada a: " + weapon.GetType().Name);
	}

	// Método para reproducir la animación correspondiente
	/*
	 * Parámetro de entrada:
	 * El método toma un parámetro baseAnimation que es el nombre base de la animación que se desea reproducir.
	 * Determinación del nombre de la animación:
	 * Si el arma actual (currentWeapon) es una escopeta (Shotgun), se añade "_shotgun" al nombre base de la animación.
	 * Si el arma actual es un revólver (Revolver), se añade "_revolver" al nombre base de la animación.
	 * Reproducción de la animación: Finalmente, se llama al método Play del objeto animation para reproducir la animación
	 * con el nombre determinado.
	 * El método SetAnimation simplemente reproduce una animación específica basada en el nombre proporcionado como parámetro.
	 */

	private void PlayAnimation(string baseAnimation)
	{
		string animationName = baseAnimation;

		if (currentWeapon is Shotgun)
		{
			animationName += "_shotgun";
		}
		else if (currentWeapon is Revolver)
		{
			animationName += "_revolver";
		}

		animation.Play(animationName);
	}

	// Método para establecer la animación
	public void SetAnimation(string animationName)
	{
		animation.Play(animationName);
	}
}
