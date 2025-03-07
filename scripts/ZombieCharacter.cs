using Godot;
using System;

public partial class ZombieCharacter : CharacterBody2D
{
	// Velocidad de movimiento del zombie
	[Export] public float Speed = 50.0f;
	// Gravedad aplicada al zombie
	[Export] public float Gravity = 500.0f;
	// Salud del zombie
	[Export] public int Health = 100;
	// Multiplicador de velocidad de ataque del zombie
	[Export] public float AttackSpeedMultiplier = 2.0f;
	// Rango de detección del zombie
	[Export] public float DetectionRange = 300.0f;
	// Daño de ataque del zombie
	[Export] public int AttackDamage = 1;
	// Fuerza de retroceso aplicada al jugador cuando es atacado
	[Export] public float KnockBackForce = 50.0f;

	private Player player;
	private AnimatedSprite2D animation;
	private bool isAttacking = false;
	private Timer deathTimer;
	private CambioNivel cambioNivel;
	private Area2D attackArea;

	private EnemyCounter enemyCounter;


	// Método llamado cuando el nodo está listo
	public override async void _Ready()
	{

		// Esperar a que se procese el frame
		await ToSignal(GetTree(), "process_frame");
		await ToSignal(GetTree(), "process_frame");

		if (GetTree().CurrentScene.Name == "Game")
		{
			enemyCounter = GetNode<EnemyCounter>("/root/Game/Player/Camera2D/Label");
		}
		else
		{
			enemyCounter = GetNode<EnemyCounter>("/root/Node2D/Player2/Camera2D/Label");
		}


		if (enemyCounter == null)
		{
			GD.PrintErr("❌ ERROR: No se encontró el Label para la puntuación.");
		}


		// Verificar si la escena actual es "Game"
		if (GetTree().CurrentScene.Name == "Game")
		{
			cambioNivel = GetNode<CambioNivel>("/root/Game/CambioNivel");
			player = GetParent().GetNode<Player>("/root/Game/Player");
			cambioNivel.RegisterZombie();
		}
		else
		{
			cambioNivel = GetNode<CambioNivel>("/root/Node2D/CambioNivel");
			player = GetParent().GetNode<Player>("/root/Node2D/Player2");
			GD.Print(player);
		}

		// Obtener el nodo de animación y conectar la señal de animación terminada
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Connect("animation_finished", new Callable(this, nameof(OnDeathAnimationFinished)));

		// Configurar el temporizador de muerte
		deathTimer = new Timer();
		deathTimer.OneShot = true;
		deathTimer.WaitTime = 0.65f;
		deathTimer.Connect("timeout", new Callable(this, nameof(OnDeathTimerTimeout)));
		AddChild(deathTimer);

		// Obtener el área de ataque
		attackArea = GetNode<Area2D>("areaAttack");
	}

	// Método llamado en cada frame de física
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Aplicar gravedad si el zombie no está en el suelo
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		else
		{
			velocity.Y = 0;
		}

		// Si el jugador está dentro del rango de detección y el zombie tiene salud, moverse hacia el jugador
		if (player != null && GlobalPosition.DistanceTo(player.GlobalPosition) <= DetectionRange && Health > 0)
		{
			float directionX = (player.GlobalPosition.X - GlobalPosition.X) > 0 ? 1 : -1;
			velocity.X = directionX * Speed;

			var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite.FlipH = directionX > 0;
			animation.Play("Attack");

			// Si el jugador está lo suficientemente cerca, atacar
			if (GlobalPosition.DistanceTo(player.GlobalPosition) <= 20.0f)
			{
				AttackPlayer();
			}
		}
		else
		{
			velocity.X = 0;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	// Método para recibir daño
	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Speed = 0;
			Die();
		}
	}

	// Método para manejar la muerte del zombie
	private void Die()
	{
		animation.Play("Die");
		GD.Print("Zombie muerto!");
		deathTimer.Start();

		if (enemyCounter != null)
		{
			//enemyCounter.AddPoints(10); // 🏆 Suma 10 puntos por cada zombi muerto
			ScoreManager.Instance.AddPoints(10);

		}

		if (cambioNivel != null)
		{
			cambioNivel.NotifyZombieDeath(this);
		}
	}

	// Método llamado cuando la animación de muerte termina
	private void OnDeathAnimationFinished()
	{
		if (animation.Animation == "Die")
		{
			deathTimer.Start();
		}
	}

	// Método llamado cuando el temporizador de muerte se agota
	private void OnDeathTimerTimeout()
	{
		QueueFree();
	}

	// Método para atacar al jugador
	private void AttackPlayer()
	{
		if (player != null)
		{
			Vector2 knockBackDirection = (player.GlobalPosition - GlobalPosition).Normalized() * KnockBackForce;
			player.TakeDamage(AttackDamage, knockBackDirection);
			GD.Print("El zombie ha atacado al jugador!");
		}
	}

	// Método llamado cuando el área de ataque detecta un cuerpo
	private void _on_area_attack_body_entered(Node body)
	{
		if (body is Player)
		{
			GD.Print("El jugador ha sido golpeado!");
			AttackPlayer();
		}
	}
}
