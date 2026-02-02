using AmpelSimulation.Classes.Services;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Container
{
    public class CclContCar
    {
        
        // Properties
        public bool IsDriving { get; set; } = true;
        public bool IsIgnoringTrafficLight { get; set; } = false;
        public bool IsAlreadyTurned { get; set; } = false;
        public string Color { get; set; }
        public CarDirection Direction { get; set; }
        public CclContLane CurrentLane { get; set; }
        public int PS { get; set; }
        public Func<int, CclContLane> ResolveLaneById { get; set; } 
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

        public double Speed { get; set; }
        public double MultipleTempo {get; set;}

        public CclContCar(double multipleTempo)
        {
            Speed = multipleTempo;
            MultipleTempo = multipleTempo;
        }

        //Eventhandler
        public event EventHandler PositionChanged;

        public event EventHandler CarStopped;

       


        // Methods
        // Method check if car is at TrafficLight
        public bool IsAtTrafficLight2(CclContTrafficLight trafficLight, int laneID)
        {
            switch (laneID)
            {
                case 1:
                    if (PositionY == trafficLight.PositionY +12)
                        return true;
                    break;
                case 2:
                    if (PositionX == trafficLight.PositionX +12)
                        return true;
                    break;
                case 3:
                    if (PositionY == trafficLight.PositionY -12)
                        return true;
                    break;
                case 4:
                    if (PositionX == trafficLight.PositionX -12)
                        return true;
                    break;
                default:
                    break;
            }
            return false;

        }

        //Is check if car is at TrafficLight in an area 
        public bool IsAtTrafficLight(
            CclContTrafficLight trafficLight,
            int laneID,
            int offsetToStopLine = 12,
            int window = 10)
        {
            switch (laneID)
            {
                case 1: // unten -> oben
                    return this.PositionY <= trafficLight.PositionY + offsetToStopLine &&
                           this.PositionY >= trafficLight.PositionY + offsetToStopLine - window;

                case 2: // rechts -> links
                    return this.PositionX <= trafficLight.PositionX + offsetToStopLine &&
                           this.PositionX >= trafficLight.PositionX + offsetToStopLine - window;

                case 3: // oben -> unten
                    return this.PositionY >= trafficLight.PositionY - offsetToStopLine &&
                           this.PositionY <= trafficLight.PositionY - offsetToStopLine + window;

                case 4: // links -> rechts
                    return this.PositionX >= trafficLight.PositionX - offsetToStopLine &&
                           this.PositionX <= trafficLight.PositionX - offsetToStopLine + window;

                default:
                    return false;
            }
        }


        // Method check if car is at TurningPoint for left turn
        public bool IsAtTurningPointLeft(CclContCar car, CclContTrafficLight trafficLight, int laneID)
        {
            int laneWidth = (int)car.CurrentLane.Width;

            switch (laneID)
            {
                case 1:
                    if ((int)car.PositionY == (int)(trafficLight.PositionY - laneWidth * 1.5))
                        return true;
                    break;

                case 2:
                    if ((int)car.PositionX == (int)(trafficLight.PositionX - laneWidth * 1.5))
                        return true;
                    break;

                case 3:
                    if ((int)car.PositionY == (int)(trafficLight.PositionY + laneWidth * 1.5))
                        return true;
                    break;

                case 4:
                    if ((int)car.PositionX == (int)(trafficLight.PositionX + laneWidth * 1.5))
                        return true;
                    break;
            }

            return false;
        }



        // Method check if car is at TurningPoint for right turn
        public bool IsAtTurningPointRight(CclContCar car, CclContTrafficLight trafficLight, int laneID)
        {
            var currentlaneWidth = car.CurrentLane.Width;
            switch (laneID)
            {
                case 1:
                    if (car.PositionY <= trafficLight.PositionY - currentlaneWidth / 2 && car.Direction == CarDirection.Right)
                        return true;
                    break;
                case 2:
                    if (car.PositionX <= trafficLight.PositionX - currentlaneWidth / 2 && car.Direction == CarDirection.Right)
                        return true;
                    break;
                case 3:
                    if (car.PositionY >= trafficLight.PositionY + currentlaneWidth / 2 && car.Direction == CarDirection.Right)
                        return true;
                    break;
                case 4:
                    if (car.PositionX >= trafficLight.PositionX + currentlaneWidth / 2 && car.Direction == CarDirection.Right)
                        return true;
                    break;
                default:
                    break;
            }
            return false;
        }


        // Methods to control the car

        // Method to start or continue driving
        public void StartOrContinueDriving(int LaneID)
        {
            // Position update based on lane
            Speed = MultipleTempo; // Set speed to normal driving speed
            StraightAhead(LaneID);
        }
        // Method to stop the car
        public void Stop(int LaneID)
        {
            // Stop the car
            Speed = 0;
            CarStopped?.Invoke(this, EventArgs.Empty);
        }
        // Method to turn the car left
        public void TurnLeft(int LaneID)
        {
            // Turn the car left
            switch (LaneID)
            {
                case 1:      
                   
                    CurrentLane = ResolveLaneById(2); // wechselt NUR die Lane-Referenz des Autos
                    StartOrContinueDriving(LaneID);
                    break;
                case 2:
                    CurrentLane = ResolveLaneById(3); 
                    StartOrContinueDriving(LaneID);
                    break;
                case 3:
                    CurrentLane = ResolveLaneById(4); 
                    StartOrContinueDriving(LaneID);  
                    break;
                case 4:
                    CurrentLane = ResolveLaneById(1); 
                    StartOrContinueDriving(LaneID);
                    break;
                default:
                    break;
            }
        }

        // Method to turn the car right
        public void TurnRight(int LaneID)
        {
            // Turn the car right
            switch (LaneID)
            {
                case 1:
                    CurrentLane = ResolveLaneById(4); 
                    StartOrContinueDriving(LaneID);
                    break;
                case 2:
                    CurrentLane = ResolveLaneById(1); 
                    StartOrContinueDriving(LaneID);               
                    break;
                case 3:
                    CurrentLane = ResolveLaneById(2); 
                    StartOrContinueDriving(LaneID);
                    break;
                case 4:
                    CurrentLane = ResolveLaneById(3); 
                    StartOrContinueDriving(LaneID);  
                    break;
                default:
                    break;
            }
        }

        // Method to drive straight ahead
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
