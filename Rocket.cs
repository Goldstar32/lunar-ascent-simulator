using Godot;
using System;

public partial class Rocket : StaticBody3D
{
	// Properties

	// Mass of fuel
	public float MFuel
	{ get; set; }

	// Dry mass aka mass of rocket excluding mass of fuel
	public float MDry
	{ get; set; }

	// Total mass
	public float MTot
	{ 
		get { return MFuel + MDry; } // Return mass of fuel and rocket combined
	}

	// Rockets radius in meters
	public float Radius
	{ get; set; }

	// Use Node3D.GlobalPosition directly for position

	// Property for velocity relative to space
	public Vector3 Velocity
	{ get; set; }

	// Property for acceleration relative to space
	public Vector3 Acceleration
	{ get; set; }

	// Use Node3D.GlobalBasis directly for rotation

	// Property for angular velocity relative to space
	public Vector3 AngularVelocity
	{ get; set; }

	// Property for angular acceleration relative to space
	public Vector3 AngularAcceleration
	{ get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
