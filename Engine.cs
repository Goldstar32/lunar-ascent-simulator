using System;
using Godot;

public partial class Engine : StaticBody3D
{
    // Properties vvv

    public Vector3 RelativePosition { get; set; } // Position relative to rocket

    public Basis RelativePositionBasis { get; set; } // Rotation relative to rocket

    public double Thrust { get; set; } // Thrust

    public double FuelConsumption { get; set; } // Fuel consumption

	// Properties ^^^
	//
	// Methods vvv

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
