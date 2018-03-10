using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace newton
{
    public class Universe
    {
        public Universe()
        {
        }

        public Universe(List<Planet> thePlanets)
        {
            Planets = thePlanets;
        }

        public Universe(string theFileName)
        {
            var aUniverse = Load(theFileName);
            if(null != aUniverse)
            {
                this.Planets = aUniverse.Planets;
            }
        }

        public List<Planet> Planets
        {
            get;
            set;
        }

        private Universe Load(string theFileName)
        {
            using (var stream = new System.IO.FileStream(theFileName, System.IO.FileMode.Open))
            {
                var serializer = new XmlSerializer(this.GetType());
                var aObject = serializer.Deserialize(stream);
                if(aObject is Universe)
                {
                    return aObject as Universe;
                }
            }
            return null;
        }

        public void Save(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }
    }
}
