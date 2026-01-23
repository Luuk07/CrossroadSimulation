using AmpelSimulation.Classes.Container;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Services
{
    // Is for each car in the crossroad
    public class CclSvcHandleCar
    {
        public CclContCar Car { get; set; }
        public CclContTrafficLight TrafficLight { get; set; }
        public CclSvcHandleLight LightHandler { get; set; }

        public List<CclSvcHandleCar> CarHandlers { get; set; }

        public Rectangle Rec { get; set; }

        public CclSvcHandleCar(CclContCar car, CclContTrafficLight trafficLight, CclSvcHandleLight lightHandler, List<CclSvcHandleCar> carHandlers)
        {
            CarHandlers = carHandlers;
            Car = car;
            TrafficLight = trafficLight;
            LightHandler = lightHandler;
            if (!ReferenceEquals(car, Car)) return; 
            Car.PositionChanged += (s, e) =>
            {
                PoitionChangedLogik(carHandlers);
            };
            LightHandler.StateChanged += (s, e) =>
            {
                if (Car.IsAtTrafficLight(TrafficLight, Car.CurrentLane.ID) && !Car.IsIgnoringTrafficLight)
                {
                    CheckTrafficLightState();
                }
                else
                {
                    PoitionChangedLogik(carHandlers); 
                } 
            };
            // Klappt noch nicht so ganz, ich finde aber kein Event, welches auslöst, wenn das Auto fahren könnte
            Car.CurrentLane.E_LaneCountChanged += (s, e) =>
            {
                PoitionChangedLogik(carHandlers);
            };
        }

       
        public void PoitionChangedLogik(List<CclSvcHandleCar> carHandlers)
        {
            Rec = new Rectangle((int)Car.PositionX, (int)Car.PositionY, 10, 10);

            // Check if the car is at the traffic light position
            if (Car.IsAtTrafficLight(TrafficLight, Car.CurrentLane.ID) && !Car.IsIgnoringTrafficLight)
            {
                CheckTrafficLightState();
            }
            else if (Car.Direction == CarDirection.Left && Car.IsAtTurningPointLeft(Car, TrafficLight, Car.CurrentLane.ID))
            {
                if (CheckIfCarCanDriveAtTurningPoint(Rec, carHandlers))
                {
                    Car.IsIgnoringTrafficLight = true;
                    Car.IsDriving = true;
                    SetCarDirection();
                }
                // Car has to stop if another car is in the turning area
                else
                {
                    Car.Stop(Car.CurrentLane.ID); 
                    Car.IsDriving = false;
                    ChangeDirectionToStraightAfterDelay(carHandlers); 
                }

            }
            else if (Car.Direction == CarDirection.Right && Car.IsAtTurningPointRight(Car, TrafficLight, Car.CurrentLane.ID))
            {
                Car.IsIgnoringTrafficLight = true;
                SetCarDirection();
            }
        }

        // Drive straight after delay
        public async Task ChangeDirectionToStraightAfterDelay(List<CclSvcHandleCar> carHandlers)
        {
            await Task.Delay(3000); // 3 Sekunden warten

            Car.IsDriving = true;
            Car.Direction = CarDirection.Straight;
            SetCarDirection();
            Car.StartOrContinueDriving(Car.CurrentLane.ID);
        }

        public void CheckTrafficLightState()
        {
            // Check the traffic light of the current car -> handle car behavior
            if (TrafficLight.CurrentState == TrafficLightState.Green)
            {
                // Car can drive
                Car.IsDriving = true;
                Car.StartOrContinueDriving(Car.CurrentLane.ID);
                //SetCarDirection();
            }
            else
            {
                // Car has to stop
                Car.IsDriving = false;
                Car.Stop(Car.CurrentLane.ID);
            }
        }

        public void SetCarDirection()
        {
            // Check the car direction and handle the car behavior
            if (Car.Direction == CarDirection.Left && !Car.IsAlreadyTurned)
            {
                // Turn left
                Car.TurnLeft(Car.CurrentLane.ID);
                Car.IsAlreadyTurned = true;
                Car.Direction = CarDirection.Straight;
            }
            else if (Car.Direction == CarDirection.Right && !Car.IsAlreadyTurned)
            {
                // Turn right
                Car.TurnRight(Car.CurrentLane.ID);
                Car.IsAlreadyTurned = true;
                Car.Direction = CarDirection.Straight;
            }
            else if (Car.Direction == CarDirection.Straight)
            {
                // Drive straight ahead
                Car.StraightAhead(Car.CurrentLane.ID);

            }

        }
        //public bool CheckIfCarCanDriveAtTurningPoint2(Rectangle rec, List<CclSvcHandleCar> carHandler)
        //{
        //    foreach (var currentCarHandler in carHandler)
        //    {
        //        int laneID = currentCarHandler.Car.CurrentLane.ID;
        //        switch (laneID)
        //        {
        //            case 1:
        //                if (rec.Contains((int)currentCarHandler.Car.PositionX, (int)currentCarHandler.Car.PositionY - 10))
        //                {
        //                    return false;
        //                }
        //                break;
        //            case 2:
        //                if (rec.Contains((int)currentCarHandler.Car.PositionX - 10, (int)currentCarHandler.Car.PositionY))
        //                {
        //                    return false;
        //                }
        //                break;
        //            case 3:
        //                if (rec.Contains((int)currentCarHandler.Car.PositionX, (int)currentCarHandler.Car.PositionY + 10))
        //                {
        //                    return false;
        //                }
        //                break;
        //            case 4:
        //                if (rec.Contains((int)currentCarHandler.Car.PositionX + 10, (int)currentCarHandler.Car.PositionY))
        //                {
        //                    return false;
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return true;

        //}





        public bool CheckIfCarCanDriveAtTurningPoint(Rectangle rec, List<CclSvcHandleCar> carHandlers)
        {
            // Size of the car
            const int size = 5;   
            
            // Checking area in front of the car
            const int forward = 10;   
            const int right = 10;     

            foreach (var handler in carHandlers)
            {
                // Dont check the car itself
                if ((int)handler.Car.PositionX == rec.X && (int)handler.Car.PositionY == rec.Y)
                    continue;

                // Rectangle of the other car
                Rectangle other = new Rectangle((int)handler.Car.PositionX, (int)handler.Car.PositionY, size, size);
                // Define the area in front and to the right of the car based on its lane
                Rectangle forwardArea;
                Rectangle forwardRightArea;

                // Determine the areas based on the car's current lane
                switch (handler.Car.CurrentLane.ID)
                {
                   
                    case 1:
                        forwardArea = new Rectangle(rec.X, rec.Y - forward, size, size);
                        forwardRightArea = new Rectangle(rec.X + right, rec.Y - forward, size, size);
                        break;
  
                    case 2:
                        forwardArea = new Rectangle(rec.X - forward, rec.Y, size, size);
                        forwardRightArea = new Rectangle(rec.X - forward, rec.Y - right, size, size);
                        break;

                    case 3:
                        forwardArea = new Rectangle(rec.X, rec.Y + forward, size, size);
                        forwardRightArea = new Rectangle(rec.X - right, rec.Y + forward, size, size);
                        break;
        
                    case 4:
                        forwardArea = new Rectangle(rec.X + forward, rec.Y, size, size);
                        forwardRightArea = new Rectangle(rec.X + forward, rec.Y + right, size, size);
                        break;

                    default:
                        continue;
                }

                // Check if the other car is in the defined areas
                // IntersectsWith() checks if two rectangles overlap
                if (other.IntersectsWith(forwardArea) || other.IntersectsWith(forwardRightArea))
                {
                    return false;
                }
            }
            
            return true;
        }

    }
    }
