using Godot;
using System;

public partial class PhysicsProcess : Node
{
	// TEMPORARY variables for testing
	private Vector3 gravity = new Vector3(0, -1.62f, 0);

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
		// Set some initial values on rocket for testing
		rocket.MTot = 100;
		

		// Add rocket to scene
		AddChild(rocket);
	}

	// Forces will need to be calculated based on other factors (gravity by distance to moon and thrust by angle)

	// Updates acceleration based on forces (Gravitation from moon (add thrust from rocket here later))
	private void UpdateAcceleration(Rocket rocket)
	{
		Vector3 lastRes = rocket.MTot * rocket.Acceleration; // Resulting force on rocket before update
		Vector3 newRes = gravity + lastRes; // New resulting force
		Vector3 newAcc = newRes / rocket.MTot; // New acceleration

		// Update rocket with new acceleration
		rocket.Acceleration = newAcc;
	}

	// Updates velocity based on acceleration and time step
	private void UpdateVelocity(Rocket rocket, double delta)
	{
		rocket.Velocity += rocket.Acceleration * (float)delta; // Velocity = acceleration * time since last update (as float)
	}

	// Updates position based on velocity and time step
	private void UpdatePosition(Rocket rocket, double delta)
	{
		rocket.GlobalPosition += rocket.Velocity * (float)delta; // Position = velocity * time since last update (as float)
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		UpdateAcceleration(rocket);
		UpdateVelocity(rocket, delta);
		UpdatePosition(rocket, delta);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
