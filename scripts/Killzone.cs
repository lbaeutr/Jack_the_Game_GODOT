using Godot;
using System;

public partial class Killzone : Area2D
{
	// Método llamado cuando el nodo está listo
	public override void _Ready()
	{

	}

	// Método llamado cuando otro cuerpo entra en el área de la killzone
	private void _on_body_entered(Node2D body)
	{
		GD.Print("Estas muerto...");
		GD.Print("Reinicio..");
		CallDeferred(nameof(ReloadScene)); // Llamar al método ReloadScene de forma diferida
	}

	// Método para recargar la escena actual
	private void ReloadScene()
	{
		GetTree().ReloadCurrentScene(); // Recargar la escena actual
	}
}
