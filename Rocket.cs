using Godot;
using System;

public partial class Rocket : StaticBody3D
{
	// Properties

	// Dry mass aka mass excluding mass of fuel
	public float MDry
	{ get; set; }

	// Property for position relative to space
	public Vector3 Position
	{ get; set; }

	public Vector3 Velocity
	{ get; set; }

	// Property for rotation relative to space


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
