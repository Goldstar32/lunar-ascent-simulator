using Godot;
using System;

public partial class PhysicsProcess : Node
{
	// Testing the rocket
	private Rocket rocket = new(10, 10, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Basis());

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set rocket starting position
		rocket.Position = new Vector3(0, 0, 0);

		// Add rocket to scene
		AddChild(rocket);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
