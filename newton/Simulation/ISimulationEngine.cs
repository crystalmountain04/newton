namespace newton.Simulation
{
    public interface ISimulationEngine
    {
        bool IsRunning { get; }
        Universe Universe { get; }

        void ApplyGravitationConstant(double theConstant);
        void Initialize(Universe theUniverseToSimulate);
        void StartSimulation();
        void StopSimulation();
    }
}