using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Container
{
    public class CclContStatistic
    {
        public int TotalCarsPassed { get; set; } = 0;
        public int Timer { get; set; } = 0;


        public List<int> ListOfWaitingTimes { get; set; } = new List<int>();


        public void AddCoutOfStopedCarsToList(int counterOfStoppedCars)
        {
            ListOfWaitingTimes.Add(counterOfStoppedCars);
        }
    }
}
