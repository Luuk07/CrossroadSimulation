using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Container
{
    public class CclContLane
    {
        public int ID { get; set; }

        public int Width { get; set; } = 10;
        public List<CclContCar> CarsInLane { get; set;  } = new List<CclContCar>();
       
    }
}
