using newton.Simulation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newton.Utility
{
    public static class Helper
    {
        public static List<Planet> DeepCopy(List<Planet> thePlanets)
        {
            var aReturn = new List<Planet>();
            foreach (var aPlanet in thePlanets)
            {
                aReturn.Add(new Planet(aPlanet));
            }
            return aReturn;
        }
    }
}
