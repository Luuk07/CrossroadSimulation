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
        public List<CclContCar> CarsInLane { get; set;  } = new List<CclContCar>();
       
    }
}
