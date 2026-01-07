using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Container
{
    public class CclContTrafficLight
    {
        public double SpeedOfChanging { get; set; } = 1; // Default speed
        public int ID { get; set; }
        public TrafficLightMode CurrentMode { get; set; }
        public TrafficLightState CurrentState { get; set; }
        public CclContLane CurrentLane { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        // 

        public void SpeedChangeMode(TrafficLightMode mode)
        {
            // Change speed based on mode
            if (mode == TrafficLightMode.ModeOne)
            {
                SpeedOfChanging = 1;
            }
            else if (mode == TrafficLightMode.ModeTwo)
            {
                SpeedOfChanging = 2;
            }
            else if (mode == TrafficLightMode.ModeThree)
            {
                SpeedOfChanging = 3;
            }
            else if (mode == TrafficLightMode.ModeFour)
            {
                SpeedOfChanging = 4;
            }
            
        }
    }
}
