using Godot;
using System;

public partial class PhysicsProcess : Node
{
	// TEMPORARY variables for testing


	// Constants (might move later)
	const double G = 6.67430e-11; // Gravitational constant

	// Rocket scene path
	private PackedScene rocketScene;
	// Create rocket object
	private Rocket rocket1;

	// Moon scene path
	private PackedScene moonScene;
	// Create moon object
	private Moon moon;

	// Method for loading and instanciating rocket
	private void LoadRocket()
	{
		// Load rocket scene
		rocketScene = (PackedScene)ResourceLoader.Load("res://rocket.tscn");

		// Instanciate rocket from scene
		rocket1 = (Rocket)rocketScene.Instantiate();

		// Set rocket starting position
		rocket1.Position = new Vector3(0, 1, 0);
		// Set some initial values on rocket for testing
		rocket1.MTot = 10e3f;
		rocket1.Velocity = new Vector3(0, 30, 0);

		// Add rocket to scene
		AddChild(rocket1);
	}

	private void LoadMoon()
	{
		// Load moon scene
		moonScene = (PackedScene)ResourceLoader.Load("res://moon.tscn");

		// Instanciate moon from scene
		moon = (Moon)moonScene.Instantiate();

		// Add moon to scene
		AddChild(moon);
	}

	public override void _Ready()
	{
		LoadRocket();
		LoadMoon();
	}

	// Calculate all current external forces and return resulting force
	private Vector3 GetTotForce(Rocket rocket)
	{
		Vector3 totForce = GetGravForce(rocket);
		return totForce;
	}

	// Calculate and return gravitational force
	private Vector3 GetGravForce(Rocket rocket)
	{
		// Calculate distance between moon and rocket
		Vector3 distanceVector = moon.GlobalPosition - rocket.GlobalPosition;
		double distance = distanceVector.Length();

		// Avoid division with 0 in case rocket is at the moons center
		if (distance == 0)
			return Vector3.Zero;

		// Calculate the magnitude
		double magnitude = G * (moon.MoonMass * rocket.MTot) / (distance * distance);

		// Return gravitational force as vector proportional to distance and direction of rocket relative to moon
		return distanceVector.Normalized() * (float)magnitude;
	}

	// Updates acceleration based on forces (Gravitation from moon (add thrust from rocket here later))
	private void UpdateAcceleration(Rocket rocket)
	{
		Vector3 lastRes = rocket.MTot * rocket.Acceleration; // Resulting force on rocket before update
		Vector3 newRes = GetTotForce(rocket) + lastRes; // New resulting force
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
