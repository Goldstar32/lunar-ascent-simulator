using System;
using Godot;

public partial class PhysicsProcess : Node
{
    // Constants vvv (might move later)

    const double G = 6.67430e-11; // Gravitational constant

    // Constants ^^^
    //
    // Instantiate objects vvv

    private PackedScene rocketScene; // Rocket scene path

    private Rocket rocket1; // Instantiate rocket object

    private PackedScene moonScene; // Moon scene path

    private Moon moon; // Create moon object

    // GUI Labels
    private Label distanceLabel;
    private Label velocityLabel;
    private Label accelerationLabel;
    private Label fuelLabel;

    // Timer for updating labels
    private double timeAccumulator = 0.0;

    // Instantiate objects ^^^
    //
    // Methods vvv

    // Constructor
    public override void _Ready()
    {
        LoadRocket();
        LoadMoon();
        GetGUILabels();
    }

    // Runs regularly to update physics
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta); // Makes this run with the base physics process

        UpdateAcceleration(rocket1, delta); // Update rockets acceleration
        UpdateVelocity(rocket1, delta); // Update rockets velocity
        UpdatePosition(rocket1, delta); // Update rockets position

        UpdateAngularAcceleration(rocket1, delta);
        UpdateAngularVelocity(rocket1, delta);
        UpdateRotation(rocket1, delta);

		// Add delta to accumulated time
        timeAccumulator += delta;

        // Update every 1 second
        if (timeAccumulator >= 1.0)
        {
            UpdateGUI(rocket1); // Call GUI update function
            timeAccumulator = 0.0; // Reset timer
        }
    }

    // Returns input value with x decimals of precision
    public static double Round(double value, int precision)
    {
        double factor = Math.Pow(10, precision); // 10^precision
        return Math.Floor(value * factor) / factor;
    }

    // Method for loading and instanciating rocket
    private void LoadRocket()
    {
        // Load rocket scene
        rocketScene = (PackedScene)ResourceLoader.Load("res://rocket.tscn");

        // Instanciate rocket from scene
        rocket1 = (Rocket)rocketScene.Instantiate();

        // Set rocket starting position
        rocket1.Position = new Vector3(0, 1, 0);

        // Set some initial values on rocket for testing
        rocket1.MDry = 2260; // [kg]
        rocket1.MFuel = 2387; // [kg]
        rocket1.Velocity = new Vector3(-100, 1000, -100); // Initial velocity
        rocket1.AngularVelocity = new Vector3(0, 0, 0); // Initial angular velocity
        rocket1.Radius = 5; // [m]

        // Add rocket to scene
        AddChild(rocket1);

        // Add main engine to rocket
        rocket1.AddNewThruster(
            "mainThruster",
            new Vector3(0, -1, 0),
            rocket1.Transform.Basis,
            5,
            3040
        );
    }

    private void LoadMoon()
    {
        // Load moon scene
        moonScene = (PackedScene)ResourceLoader.Load("res://moon.tscn");

        // Instanciate moon from scene
        moon = (Moon)moonScene.Instantiate();

        // Add moon to scene
        AddChild(moon);
    }

    // Gets the labels for the GUI
    private void GetGUILabels()
    {
        // Access the GUI labels
        var statsContainer = GetNode<VBoxContainer>(
            "Rocket/Camera3D/GUI/HBoxContainer/HSplitContainer/BackgroundPanel/StatsContainer"
        );

        distanceLabel = statsContainer.GetNode<Label>("DistanceLabel");
        velocityLabel = statsContainer.GetNode<Label>("VelocityLabel");
        accelerationLabel = statsContainer.GetNode<Label>("AccelerationLabel");
        fuelLabel = statsContainer.GetNode<Label>("FuelLabel");
    }

    // Updates gui to correctly show rocket's values
    private void UpdateGUI(Rocket rocket)
    {
        // Round values with two decimals of precision
        double distanceToMoon = Round(
            (moon.GlobalPosition.DistanceTo(rocket.GlobalPosition) - moon.Radius) / 1000,
            2
        ); // Distance from rocket to moon's surface in km [km]
        double velocity = rocket.Velocity.Length(); // Length of velocity vector [m/s]
        double acceleration = rocket.Acceleration.Length(); // Length of acceleration vector [m/s^2]
        double remainingFuel = rocket1.MFuel; // Current fuel mass [kg]

        // Update the GUI labels with formatted text
        distanceLabel.Text = $"Distance to moon: \n{distanceToMoon:F2} km";
        velocityLabel.Text = $"Velocity: \n{velocity:F2} m/s";
        accelerationLabel.Text = $"Acceleration: \n{acceleration:F2} m/s²";
        fuelLabel.Text = $"Remaining fuel: \n{remainingFuel:F2} kg";
    }

    // Calculate all current external forces and return resulting force
    private Vector3 GetTotForce(Rocket rocket, double delta)
    {
        Vector3 totForce = new Vector3(); // Instantiate total force as new Vector3
        totForce += GetGravForce(rocket); // Add gravitational force
        totForce += rocket.GetTotalThrustForce(delta); // Add force from thrusters
        return totForce;
    }

    // Calculate and return gravitational force
    private Vector3 GetGravForce(Rocket rocket)
    {
        // Calculate distance between moon and rocket
        Vector3 distanceVector = moon.GlobalPosition - rocket.GlobalPosition;
        double distance = distanceVector.Length();

        // Avoid division with 0 in case rocket is at the moons center
        if (distance == 0)
            return Vector3.Zero;

        // Calculate the magnitude
        double magnitude = G * (moon.MoonMass * rocket.MTot) / (distance * distance);

        // Return gravitational force as vector proportional to distance and direction of rocket relative to moon
        return distanceVector.Normalized() * (float)magnitude;
    }

    // Updates acceleration based on forces (Gravitation from moon (add thrust from rocket here later))
    private void UpdateAcceleration(Rocket rocket, double delta)
    {
        Vector3 newRes = GetTotForce(rocket, delta); // New resulting force
        Vector3 newAcc = newRes / rocket.MTot; // New acceleration

        // Update rocket with new acceleration
        rocket.Acceleration = newAcc;
    }

    // Updates velocity based on acceleration and time step
    private void UpdateVelocity(Rocket rocket, double delta)
    {
        rocket.Velocity += rocket.Acceleration * (float)delta; // Velocity = acceleration * time since last update (as float)
    }

    // Updates position based on velocity and time step
    private void UpdatePosition(Rocket rocket, double delta)
    {
        rocket.Position += rocket.Velocity * (float)delta; // Position = velocity * time since last update (as float)
    }

    // Method to calculate and return total torque based on the forces acting on the rocket
    private Vector3 GetTotTorque(Rocket rocket, double delta)
    {
        Vector3 totTorque = new Vector3(); // Instantiate total torque as new Vector3
        totTorque += rocket.GetTotalThrustTorque(delta); // Add torque from thrusters
        return totTorque; // Return total torque
    }

    // Update angular acceleration based on applied torque and moment of inertia
    private void UpdateAngularAcceleration(Rocket rocket, double delta)
    {
        // Calculate the moment of inertia for a homogeneous cylinder
        float momentOfInertia = 0.5f * rocket.MTot * rocket.Radius * rocket.Radius;

        // Calculate torque acting on the rocket
        Vector3 torque = GetTotTorque(rocket, delta); // Get torque based on other forces

        // Update rocket's angular acceleration
        rocket.AngularAcceleration = torque / momentOfInertia;
    }

    // Update angular velocity based on angular acceleration and time step
    private void UpdateAngularVelocity(Rocket rocket, double delta)
    {
        // Angular velocity = angular acceleration * time since last update (as float)
        rocket.AngularVelocity += rocket.AngularAcceleration * (float)delta;
    }

    // Update rotation based on angular velocity and time step
    private void UpdateRotation(Rocket rocket, double delta)
    {
        // Convert angular velocity into a small rotation
        Vector3 angularVelocity = rocket.AngularVelocity * (float)delta;

        // Apply the incremental rotation to the Basis using Rotated on Transform.Basis
        rocket.Transform = new Transform3D(
            rocket.Transform.Basis.Rotated(angularVelocity.Normalized(), angularVelocity.Length()),
            rocket.Transform.Origin
        );
    }
}
