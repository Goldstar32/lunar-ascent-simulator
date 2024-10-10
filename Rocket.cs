using Godot;
using System;

public partial class Rocket : StaticBody3D
{
	// Properties

	// Total mass
	public double MTot
	{ get; set; }

	// Dry mass aka mass excluding mass of fuel
	public double MDry
	{ get; set; }

	// Use Node3D.Position directily for position

	// Property for velocity relative to space
	public Vector3 Velocity
	{ get; set; }

	// Property for acceleration relative to space
	public Vector3 Acceleration
	{ get; set; }

	// Property for rotation relative to space
	public Basis RotationBasis
	{ get; set; }


	// Constructor
	public Rocket() {}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
