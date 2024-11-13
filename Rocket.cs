using System;
using System.Collections.Generic;
using Godot;

public partial class Rocket : StaticBody3D
{
	// Properties vvv

	public float MFuel { get; set; } // Mass of fuel [kg]

	public float MDry { get; set; } // Dry mass (non fuel mass) [kg]

	public float MTot => MFuel + MDry; // Total mass [kg]

	public float Radius { get; set; } // Rockets radius [m]

	// Use Node3D.GlobalPosition directly for position [m]

	public Vector3 Velocity { get; set; } // Velocity relative to space [m/s]

	public Vector3 Acceleration { get; set; } // Acceleration relative to space [m/s^2]

	// Use Node3D.GlobalBasis directly for rotation [rad]

	public Vector3 AngularVelocity { get; set; } // Property for angular velocity relative to space [rad/s]

	public Vector3 AngularAcceleration { get; set; } // Property for angular acceleration relative to space [rad/s^2]

	public List<Engine> Engines { get; set; } = new List<Engine>(); // List of engines attached to the rocket

	// Properties ^^^
	//
	// Methods vvv

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
