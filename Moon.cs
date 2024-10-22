using Godot;
using System;

public partial class Moon : StaticBody3D
{
	// Constant for moons radius
	const double moonRadius = 1737.4e3;

	// Property for moons mass
	public double MoonMass
	{ get; set; }

	// Use global position

	// Constructor
	public Moon() { }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set initial values
		MoonMass = 7.342e22; // Mass of moon
		this.GlobalPosition = new Vector3(0, -(float)moonRadius, 0); // Center of moon (origo is approx at surface of moon)
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
