using Godot;
using System;

public partial class PhysicsProcess : Node
{
	// Rocket scene path
	private PackedScene rocketScene;
	private Rocket rocket;

	public override void _Ready()
	{
		// Load rocket scene
		rocketScene = (PackedScene)ResourceLoader.Load("res://rocket.tscn");

		// Instanciate rocket from scene
		rocket = (Rocket)rocketScene.Instantiate();

		// Set rocket starting position
		rocket.Position = new Vector3(0, 1, 0);

		// Add rocket to scene
		AddChild(rocket);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
