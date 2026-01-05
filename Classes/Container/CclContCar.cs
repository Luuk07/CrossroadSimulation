using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Container
{
    public class CclContCar
    {
        private int _PositionX;
        private int _PositionY;
        private CclContLane _CurrentLane;
        //
        public string Color { get; set; }

        public CarDirection Direction { get; set; }
        public CclContLane CurrentLane { get; set; }
        public int PS { get; set; }
        public double Weight { get; set; }
        public double PositionX
        {
            get;
            set;
        } 
          
        public double PositionY 
        {
            get;
            set;
        }

        public double Speed { get; set; } = 1;

        // public double Speed { get { return (PS / Weight) * 100; } } If cars should be faster/slower

        //Eventhandler
        public event EventHandler PositionChanged;

        // Constructor

        // Methods
        public bool IsAtTrafficLight(CclContTrafficLight trafficLight, int laneID)
        {
            switch (laneID)
            {
                case 1:
                    if (PositionY == trafficLight.PositionY +10)
                        return true;
                    break;
                case 2:
                    if (PositionX == trafficLight.PositionX +10)
                        return true;
                    break;
                case 3:
                    if (PositionY == trafficLight.PositionY -10)
                        return true;
                    break;
                case 4:
                    if (PositionX == trafficLight.PositionX -10)
                        return true;
                    break;
                default:
                    break;
            }
            return false;

        }

        public bool CarInFront(int lineID)
        {
            switch (lineID)
            {
                case 1:
                    if (PositionY == + 10)
                        return true;
                    break;
                case 2:
                    if (PositionX == + 10)
                        return true;
                    break;
                case 3:
                    if (PositionY == - 10)
                        return true;
                    break;
                case 4:
                    if (PositionX ==  - 10)
                        return true;
                    break;
                default:
                    break;
            }
            return false;
        }

        // Methods to control the car
        public void StartOrContinueDriving(int LaneID)
        {
            // Position update based on lane
            Speed = 1; // Set speed to normal driving speed
            StraightAhead(LaneID);
        }

        public void Stop(int LaneID)
        {
            // Stop the car
            Speed = 0;
        }   

        public void TurnLeft(int LaneID)
        {
            // Turn the car left
            switch (LaneID)
            {
                case 1:      
                    CurrentLane.ID = 4;
                    //PositionX -= Speed;
                    break;
                case 2:
                    CurrentLane.ID = 1;
                    //PositionY += Speed;
                    break;
                case 3:
                    CurrentLane.ID = 2;
                    //PositionX += Speed;
                    break;
                case 4:
                    CurrentLane.ID = 3;
                    //PositionY -= Speed;
                    break;
                default:
                    break;
            }
        }

        public void TurnRight(int LaneID)
        {
            // Turn the car right
            switch (LaneID)
            {
                case 1:
                    CurrentLane.ID = 2;
                    //PositionX += Speed;
                    break;
                case 2:
                    CurrentLane.ID = 3;
                    //PositionY -= Speed;
                    break;
                case 3:
                    CurrentLane.ID = 4;
                    //PositionX -= Speed;
                    break;
                case 4:
                    CurrentLane.ID = 1;
                    //PositionY += Speed;
                    break;
                default:
                    break;
            }
        }

        public void StraightAhead(int LaneID)
        {
            // Drive straight ahead
            switch (LaneID)
            {
                case 1:
                    PositionY -= Speed;
                    break;
                case 2:
                    PositionX -= Speed; 
                    break;
                case 3:
                    PositionY += Speed;
                    break;
                case 4:
                    PositionX += Speed;
                    break;
                default:
                    break;
            }
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
