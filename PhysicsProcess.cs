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

	public override void _Ready()
	{
		LoadRocket();
		LoadMoon();
	}

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
		rocket1.Velocity = new Vector3(-100, 1000, -100);
		rocket1.Acceleration = new Vector3(0, 50, 0);
		rocket1.AngularVelocity = new Vector3(3, 0, 0);
		rocket1.Radius = 5f;

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
		Vector3 newRes = lastRes + GetTotForce(rocket); // New resulting force
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

	// Method to calculate torque based on the forces acting on the rocket (wip)
	private Vector3 CalculateTorque(Rocket rocket)
	{
		// Teste calculation: modify this based on force application logic
		// For instance, if thrust is applied at a distance from the center:
		Vector3 thrustForce = new Vector3(0, 0, 0); // Replace with actual thrust force later
		Vector3 distanceFromCenter = new Vector3(rocket.Radius, -2 * rocket.Radius, 0); // Distance vector to where the force is applied

		// Torque = r x F (cross product)
		return distanceFromCenter.Cross(thrustForce);
	}

	// Update angular acceleration based on applied torque and moment of inertia
	private void UpdateAngularAcceleration(Rocket rocket)
	{
		// Calculate the moment of inertia for a homogeneous cylinder
		float momentOfInertia = 0.5f * rocket.MTot * rocket.Radius * rocket.Radius;

		// Calculate torque acting on the rocket (you'll need to define how you calculate torque)
		Vector3 torque = CalculateTorque(rocket); // Get torque based on other forces

		// Calculate angular acceleration
		Vector3 angularAcceleration = torque / momentOfInertia;

		// Update rocket's angular acceleration
		rocket.AngularAcceleration = angularAcceleration;
	}

	// Update angular velocity based on angular acceleration and time step
	private void UpdateAngularVelocity(Rocket rocket, double delta)
	{
		// Angular velocity = angular acceleration * time since last update (as float)
		rocket.AngularVelocity += rocket.AngularAcceleration * (float)delta;
	}

	// Update rotation based on angular velocity and time step
	private void UpdateRotation(Rocket rocket, double delta)
	{
		// Convert angular velocity into a small rotation
		Vector3 angularVelocity = rocket.AngularVelocity * (float)delta;

		// Apply the incremental rotation to the GlobalBasis using Rotated on GlobalTransform.Basis
		rocket.GlobalTransform = new Transform3D(
			rocket.GlobalTransform.Basis.Rotated(angularVelocity.Normalized(), angularVelocity.Length()),
			rocket.GlobalTransform.Origin
		);
	}

	// Runs regularly to update physics
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta); // Makes this run with the base physics process

		UpdateAcceleration(rocket1); // Update rockets acceleration
		UpdateVelocity(rocket1, delta); // Update rockets velocity
		UpdatePosition(rocket1, delta); // Update rockets position

		UpdateAngularAcceleration(rocket1);
		UpdateAngularVelocity(rocket1, delta);
		UpdateRotation(rocket1, delta);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
