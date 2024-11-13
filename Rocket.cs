using System;
using Godot;

public partial class Rocket : StaticBody3D
{
    // Properties vvv

    public float MFuel { get; set; } // Mass of fuel

    public float MDry { get; set; } // Dry mass aka mass of rocket excluding mass of fuel

    public float MTot => MFuel + MDry; // Total mass. Return mass of fuel and rocket combined

    public float Radius { get; set; } // Rockets radius in meters

    // Use Node3D.GlobalPosition directly for position

    public Vector3 Velocity { get; set; } // Velocity relative to space

    public Vector3 Acceleration { get; set; } // Acceleration relative to space

    // Use Node3D.GlobalBasis directly for rotation

    public Vector3 AngularVelocity { get; set; } // Property for angular velocity relative to space

    public Vector3 AngularAcceleration { get; set; } // Property for angular acceleration relative to space

    // Properties ^^^
    //
    // Methods vvv

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
