using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace newton
{
    public static class UniverseFactory
    {
        public static Universe CreateUniverse(Configuration theConfiguration)
        {
            var aUniverse = new Universe();
            aUniverse.Configuration = theConfiguration;
            aUniverse.Planets = createInitialPlanets(aUniverse.Configuration);
            return aUniverse;
        }

        private static List<Planet> createInitialPlanets(Configuration theConfiguration)
        {
            var aPlanets = new List<Planet>();

            aPlanets.Add(new Planet(50000, new Point(theConfiguration.SandboxSize_px * 0.5, theConfiguration.SandboxSize_px * 0.5), new Point(0, 0), "Yellow"));
            aPlanets.Add(new Planet(30, new Point(theConfiguration.SandboxSize_px - 200, 100), new Point(20, 30), "Turquoise"));
            aPlanets.Add(new Planet(30, new Point(100, theConfiguration.SandboxSize_px - 220), new Point(0, -30), "Magenta"));
            aPlanets.Add(new Planet(20, new Point(105, 103), new Point(20, 5), "Green"));
            aPlanets.Add(new Planet(10, new Point(theConfiguration.SandboxSize_px - 123, theConfiguration.SandboxSize_px - 142), new Point(-10, 0), "Red"));

            //Random aRandom = new Random();
            //for (int i = 0; i < 1000; i++)
            //{
            //    var aLocX = aRandom.Next(0, myUniverse.Configuration.SandboxSize_px);
            //    var aLocY = aRandom.Next(0, myUniverse.Configuration.SandboxSize_px);
            //    var aMass = aRandom.Next(30, 50);
            //    var aAccX = aRandom.Next(-100, 100);
            //    var aAccY = aRandom.Next(-100, 100);
            //    aPlanets.Add(new Planet(aMass, new Point(aLocX, aLocY), new Point(aAccX, aAccY), "White"));
            //}

            return aPlanets;
        }
    }
}
