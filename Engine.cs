using System;
using Godot;

public partial class Engine : Node3D
{
	// Constants vvv

	private const double g = 9.80665; // Standard gravity (used in calculation) [N]

	// Constants ^^^
	//
	// Properties vvv

	public Vector3 RelativePosition { get; set; } // Position relative to rocket [m]

	public Basis RelativePositionBasis { get; set; } // Rotation relative to rocket [rad]

	public double Mdot { get; set; } // Mass flow rate or fuel consumption [kg/s]

	// Properties ^^^
	//
	// Methods vvv

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	// Returns thrust based on engine's direction relative to rocket
	public Vector3 GetThrust(Rocket rocket)
	{
		// The thrust direction is based on the engine's local -Z axis
		Vector3 thrustDirection = -Transform.Basis.Z.Normalized(); // Thrust is applied in the engine's local -Z direction
		Vector3 thrustForce = thrustDirection /* Insert calculated thrust */;

		// Return thrust force
		return thrustForce;
	}
}
