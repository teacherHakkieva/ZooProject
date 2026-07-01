using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooProject
{
    public class Animal
    {
        public string AnimalID { get; set; }
        public string Species { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Habitat { get; set; }
        public bool Availability { get; set; }

        public Animal(string id, string species, string name, int age, string habitat, bool availability)
        {
            AnimalID = id;
            Species = species;
            Name = name;
            Age = age;
            Habitat = habitat;
            Availability = availability;
        }
        public string ToFileRow()
        {
            return $"{AnimalID};{Species};{Name};{Age};{Habitat};{Availability}";
        }

    }
}
