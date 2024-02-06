namespace newton.Simulation
{
    public class Universe
    {
        public Universe()
        {
        }

        public Universe(List<Planet> thePlanets, SimulationSzenario theConfiguration)
        {
            Planets = thePlanets;
            Configuration = theConfiguration;
        }

        public List<Planet> Planets
        {
            get;
            set;
        }

        public SimulationSzenario Configuration
        {
            get;
            set;
        }
    }
}
