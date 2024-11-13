using Godot;
using System;

public partial class Engine : Node
{
	// Properties

	// Position relative to rocket
	public Vector3 RelativePosition
	{ get; set; }

	// Rotation relative to rocket
	public Basis RelativePositionBasis
	{ get; set; }

	// Thrust
	public double Thrust
	{ get; set; }

	// Fuel consumption
	public double FuelConsumption
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