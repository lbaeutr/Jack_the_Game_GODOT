# VideoJuegoJack

## Descripción
VideoJuegoJack es un juego desarrollado en Godot utilizando C#. El juego incluye varios elementos como personajes, armas, municiones, y zonas de muerte. Los jugadores pueden interactuar con estos elementos para avanzar en el juego.

## Estructura del Proyecto

### Clases Principales

#### ZombieCharacter
- **Descripción**: Representa a un zombi en el juego.
- **Propiedades**:
  - `Speed`: Velocidad de movimiento del zombi.
  - `Gravity`: Gravedad aplicada al zombi.
  - `Health`: Salud del zombi.
  - `AttackSpeedMultiplier`: Multiplicador de velocidad de ataque.
  - `DetectionRange`: Rango de detección del zombi.
  - `AttackDamage`: Daño de ataque del zombi.
  - `KnockBackForce`: Fuerza de retroceso aplicada al jugador cuando es atacado.
- **Métodos**:
  - `_Ready()`: Inicializa el zombi.
  - `_PhysicsProcess(double delta)`: Actualiza la física del zombi.
  - `TakeDamage(int damage)`: Aplica daño al zombi.
  - `Die()`: Maneja la muerte del zombi.
  - `OnDeathAnimationFinished()`: Llamado cuando la animación de muerte termina.
  - `OnDeathTimerTimeout()`: Llamado cuando el temporizador de muerte se agota.
  - `AttackPlayer()`: Ataca al jugador.
  - `_on_area_attack_body_entered(Node body)`: Llamado cuando el área de ataque detecta un cuerpo.

#### CambioNivel
- **Descripción**: Maneja el cambio de niveles en el juego.
- **Propiedades**:
  - `currentScene`: Escena actual.
  - `routeScene`: Ruta de la escena actual.
  - `zombiesVivos`: Contador de zombis vivos.
  - `isFirstScene`: Indica si es la primera escena.
- **Métodos**:
  - `_Ready()`: Inicializa el nodo.
  - `RegisterZombie()`: Registra un zombi.
  - `NotifyZombieDeath(ZombieCharacter zombie)`: Notifica la muerte de un zombi.
  - `LoadScene(string scenePath)`: Carga una nueva escena.
  - `ShowFinalScreen()`: Muestra la pantalla final.

#### Weapon
- **Descripción**: Clase abstracta que define las propiedades básicas de un arma.
- **Propiedades**:
  - `fireRate`: Cadencia de fuego del arma.
  - `ammo`: Munición actual del arma.
  - `maxAmmo`: Munición máxima del arma.
  - `damage`: Daño que causa el arma.

#### Amunnation
- **Descripción**: Representa la munición en el juego.
- **Métodos**:
  - `_Ready()`: Inicializa el nodo.
  - `_on_body_entered(Node body)`: Llamado cuando otro nodo entra en el área de colisión.

#### Bullet
- **Descripción**: Representa una bala en el juego.
- **Propiedades**:
  - `speedBullet`: Velocidad de la bala.
  - `damage`: Daño que causa la bala.
- **Métodos**:
  - `_Ready()`: Inicializa el nodo.
  - `_Process(double delta)`: Actualiza la posición de la bala.
  - `OnBodyEntered(Node body)`: Llamado cuando la bala colisiona con otro cuerpo.

#### Killzone
- **Descripción**: Representa una zona de muerte en el juego.
- **Métodos**:
  - `_Ready()`: Inicializa el nodo.
  - `_on_body_entered(Node2D body)`: Llamado cuando otro cuerpo entra en el área de la killzone.
  - `ReloadScene()`: Recarga la escena actual.

#### Life
- **Descripción**: Representa un área que aumenta la vida del jugador.
- **Métodos**:
  - `_Ready()`: Inicializa el nodo.
  - `_on_body_entered(Player body)`: Llamado cuando otro nodo entra en el área de colisión.

#### Revolver
- **Descripción**: Representa un revólver en el juego.
- **Métodos**:
  - `_Ready()`: Inicializa el nodo.
  - `_on_body_entered(Node body)`: Llamado cuando otro cuerpo entra en el área del arma.

#### Shotgun
- **Descripción**: Representa una escopeta en el juego.
- **Métodos**:
  - `_Ready()`: Inicializa el nodo.
  - `_on_shotgun_body_entered(Node body)`: Llamado cuando otro cuerpo entra en el área del arma.

## Requisitos
- Godot Engine
- .NET SDK

## Instalación
1. Clona el repositorio.
2. Abre el proyecto en Godot.
3. Compila y ejecuta el proyecto.
