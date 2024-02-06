namespace newton.Simulation
{
    public interface IUniverseService
    {
        Universe CreateUniverse(SimulationSzenario theConfiguration);
        Universe? LoadUniverse(string theFileName);
    }
}