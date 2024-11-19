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

	public Basis RelativeRotationBasis { get; set; } // Rotation relative to rocket [rad]

	public double Mdot { get; set; } // Mass flow rate or fuel consumption [kg/s]

	public double ExhaustVelocity { get; set; } // Velocity of exhausted mass [m/s]

	// Properties ^^^
	//
	// Methods vvv

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	// Returns thrust force as vector based on engine's direction relative to rocket
	public Vector3 GetThrustForce(double delta)
    {
        if (this.GetParent() is Rocket rocket && rocket.MFuel > 0)
        {
            // Fuel consumed in this time step
            float deltaFuel = (float)(Mdot * delta);
            if (deltaFuel > rocket.MFuel) deltaFuel = (float)rocket.MFuel;

            // Calculate thrust force magnitude
            float thrustMagnitude = (float)ExhaustVelocity * deltaFuel / (float)delta;

            // Direction of thrust: forward (engine's local positive Z direction in Godot)
            Vector3 thrustDirection = Transform.Basis.Z.Normalized();

            // Return thrust force as vector
            return thrustDirection * thrustMagnitude;
        }

        // No thrust if out of fuel
        return Vector3.Zero;
    }
}
