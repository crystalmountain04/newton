using newton.Simulation;

namespace newton.Services
{
    public interface ISimulationService
    {
        bool IsRunning { get; }
        Universe Universe { get; }

        void ApplyGravitationConstant(double theConstant);
        void Initialize(Universe theUniverseToSimulate);
        void StartSimulation();
        Task StopSimulationAsync();
    }
}