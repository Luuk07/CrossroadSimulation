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

        public double waitingTimeForChanging = 1000; // in milliseconds

        public Rectangle Rec { get; set; }

        public CclSvcHandleCar(CclContCar car, CclContTrafficLight trafficLight, CclSvcHandleLight lightHandler, List<CclSvcHandleCar> carHandlers)
        {
            
            CarHandlers = carHandlers;
            Car = car;
            TrafficLight = trafficLight;
            LightHandler = lightHandler;
            if (!ReferenceEquals(car, Car)) return;
            // Subscribe to the PositionChanged event of the car which triggers the logic when the car moves
            Car.PositionChanged += (s, e) =>
            {
                PoitionChangedLogik(carHandlers);
            };

            // Subscribe to the StateChanged event of the traffic light which triggers the logic when the traffic light changes
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
        

            // Subscribe to the CarStopped event of the car which triggers the logic when the car stops
            Car.CarStopped += async (s, e) =>
            {
                try
                {
                    await DelayForCarToPassCrossroad(carHandlers);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in DelayForCarToPassCrossroad: {ex.Message}");
                }
            };
        }

        // Logic when the position of the car changes
        public async void PoitionChangedLogik(List<CclSvcHandleCar> carHandlers)
        {
            Rec = new Rectangle((int)Car.PositionX, (int)Car.PositionY, 10, 10);

            // Check if the car is at the traffic light position
            if (Car.IsAtTrafficLight(TrafficLight, Car.CurrentLane.ID) && !Car.IsIgnoringTrafficLight)
            {
                CheckTrafficLightState();
            }
            // Check if the car is at the turning point for left and if the direction is of the car is left
            else if (Car.Direction == CarDirection.Left && Car.IsAtTurningPointLeft(Car, TrafficLight, Car.CurrentLane.ID))
            {
                // Check if the car can drive at the turning point
                if (CheckIfCarCanDriveAtTurningPoint(Rec, carHandlers, this))
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
                    try
                    {
                        await ChangeDirectionToStraightAfterDelay(carHandlers); 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in ChangeDirectionToStraightAfterDelay: {ex.Message}");
                    }
                }

            }
            // Check if the car is at the turning point for right and if the direction is of the car is right
            else if (Car.Direction == CarDirection.Right && Car.IsAtTurningPointRight(Car, TrafficLight, Car.CurrentLane.ID))
            {
                Car.IsIgnoringTrafficLight = true;
                SetCarDirection();
            }
        }

        // Delay for checking again after car had stopped at the crossroad
        public async Task DelayForCarToPassCrossroad(List<CclSvcHandleCar> carHandlers)
        {
            try
            {
                await Task.Delay(150); 
                PoitionChangedLogik(carHandlers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DelayForCarToPassCrossroad: {ex.Message}");
            }
        }

        // Drive straight after delay -> to avoid blocking the crossroad
        public async Task ChangeDirectionToStraightAfterDelay(List<CclSvcHandleCar> carHandlers)
        {
             await Task.Delay((int)waitingTimeForChanging); 
             Car.IsDriving = true;
             Car.Direction = CarDirection.Straight;
             SetCarDirection();
             Car.StartOrContinueDriving(Car.CurrentLane.ID);
        }

        // Check the traffic light state and handle the car behavior
        public void CheckTrafficLightState()
        {
            // Check the traffic light of the current car -> handle car behavior
            if (TrafficLight.CurrentState == TrafficLightState.Green)
            {
                // Car can drive
                Car.IsDriving = true;
                Car.StartOrContinueDriving(Car.CurrentLane.ID);
            }
            else
            {
                // Car has to stop
                Car.IsDriving = false;
                Car.Stop(Car.CurrentLane.ID);
            }
        }

        // Set the car direction based on its intended direction
        public void SetCarDirection()
        {
            // Check the car direction and handle the car behavior
            if (Car.Direction == CarDirection.Left && !Car.IsAlreadyTurned)
            {
                // Turn left -> set the new lane id after turning
                Car.TurnLeft(Car.CurrentLane.ID);
                Car.IsAlreadyTurned = true;
                Car.Direction = CarDirection.Straight;
            }
            else if (Car.Direction == CarDirection.Right && !Car.IsAlreadyTurned)
            {
                // Turn right -> set the new lane id after turning
                Car.TurnRight(Car.CurrentLane.ID);
                Car.IsAlreadyTurned = true;
                Car.Direction = CarDirection.Straight;
            }
            else if (Car.Direction == CarDirection.Straight)
            {
                // Drive straight ahead -> set the new lane id after turning
                Car.StraightAhead(Car.CurrentLane.ID);

            }

        }

        // Method to check if another car is in front or to the right of the car at the turning point
        public bool CheckIfCarCanDriveAtTurningPoint(Rectangle rec, List<CclSvcHandleCar> carHandlers, CclSvcHandleCar currentCarHandler)
        {
            // Size of the car
            const int size = 10;

            // Checking areas
            const int forwardSize = 10;  
            const int leftSize = 10;

            // Distances for checking areas
            const int forward = 10;
            const int left = 10;        

            foreach (var handler in carHandlers.ToList())
            {
                // Doesnt check itself
                if ((int)handler.Car.PositionX == rec.X && (int)handler.Car.PositionY == rec.Y)
                    continue;

                // Rectangle of the other car
                Rectangle other = new Rectangle((int)handler.Car.PositionX, (int)handler.Car.PositionY, size, size);

                // Areas to check
                Rectangle leftArea;
                Rectangle forwardLeftArea;

                // Area definition based on the lane of the current car
                switch (currentCarHandler.Car.CurrentLane.ID)
                {
                    case 1: 
                        leftArea = new Rectangle(rec.X - left, rec.Y, leftSize, leftSize);
                        forwardLeftArea = new Rectangle(rec.X - left, rec.Y - forward, forwardSize, forwardSize);
                        break;
                    case 2: 
                        leftArea = new Rectangle(rec.X, rec.Y + left, leftSize, leftSize);
                        forwardLeftArea = new Rectangle(rec.X - forward, rec.Y + left, forwardSize, forwardSize);
                        break;
                    case 3: 
                        leftArea = new Rectangle(rec.X + left, rec.Y, leftSize, leftSize);
                        forwardLeftArea = new Rectangle(rec.X + left, rec.Y + forward, forwardSize, forwardSize);
                        break;
                    case 4: 
                        leftArea = new Rectangle(rec.X, rec.Y - left, leftSize, leftSize);
                        forwardLeftArea = new Rectangle(rec.X + forward, rec.Y - left, forwardSize, forwardSize);
                        break;
                    default:
                        continue;
                }

                // Check if the other car crosses the areas, with the current car wants to turn left
                if (other.IntersectsWith(leftArea) || other.IntersectsWith(forwardLeftArea))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
