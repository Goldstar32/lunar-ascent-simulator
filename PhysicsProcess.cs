using Godot;
using System;

public partial class PhysicsProcess : Node
{
	// TEMPORARY variables for testing


	// Constants (might move later)
	const double G = 6.67430e-11; // Gravitational constant
	const double moonMass = 7.342e22; // Mass of moon
	Vector3 moonPos = new Vector3(0, 0, 0); // Center of moon (currently origo)


	// Rocket scene path
	private PackedScene rocketScene;
	private Rocket rocket1;

	public override void _Ready()
	{
		// Load rocket scene
		rocketScene = (PackedScene)ResourceLoader.Load("res://rocket.tscn");

		// Instanciate rocket from scene
		rocket1 = (Rocket)rocketScene.Instantiate();

		// Set rocket starting position
		rocket1.Position = new Vector3(0, 1, 0);
		// Set some initial values on rocket for testing
		rocket1.MTot = 100;
		rocket1.Velocity = new Vector3(0, 10, 0);

		// Add rocket to scene
		AddChild(rocket1);
	}

	// Calculate all current forces and return resulting force

	// Calculate and return gravitational force
	private Vector3 GetGravForce(Rocket rocket)
	{
		// Calculate distance between moon and rocket
		Vector3 distanceVector = moonPos - rocket.GlobalPosition;
		double distance = distanceVector.Length();

		// Avoid division with 0 in case rocket is at the moons center
		if (distance == 0) 
        return Vector3.Zero;

		// Calculate the magnitude
		double magnitude = G * (moonMass * rocket.MTot) / (distance * distance);

		// Return gravitational force as vector proportional to distance and direction of rocket relative to moon
		return distanceVector.Normalized() * (float)magnitude;
	}

	// Updates acceleration based on forces (Gravitation from moon (add thrust from rocket here later))
	private void UpdateAcceleration(Rocket rocket)
	{
		Vector3 lastRes = rocket.MTot * rocket.Acceleration; // Resulting force on rocket before update
		Vector3 newRes = GetGravForce(rocket) + lastRes; // New resulting force
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
		UpdateAcceleration(rocket1);
		UpdateVelocity(rocket1, delta);
		UpdatePosition(rocket1, delta);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
