using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Tools
{

    // Enums for Traffic Light States, Modes, and Car Directions
    public enum TrafficLightState
    {
        Red,
        Yellow,
        Green
    }
    public enum TrafficLightMode
    {
        ModeOne,
        ModeTwo,
        ModeThree,
        ModeFour
    }
    public enum CarDirection
    {
        Right = 1,
        Left = 2,
        Straight = 3,
    }
}
