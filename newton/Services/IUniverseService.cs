using newton.Simulation;

namespace newton.Services
{
    public interface IUniverseService
    {
        Universe CreateUniverse(SimulationSzenario theConfiguration);
        Universe? LoadUniverse(string theFileName);
        void SaveUniverse(string theFileName, Universe theUniverse);
    }
}