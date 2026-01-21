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
                    CheckTrafficLightState(car);
                }
                else
                {
                    PoitionChangedLogik(carHandlers); 
                }
                
            };

        }


        public void PoitionChangedLogik(List<CclSvcHandleCar> carHandlers)
        {
            Rec = new Rectangle((int)Car.PositionX, (int)Car.PositionY, 10, 10);

            // Check if the car is at the traffic light position
            if (Car.IsAtTrafficLight(TrafficLight, Car.CurrentLane.ID) && !Car.IsIgnoringTrafficLight)
            {
                CheckTrafficLightState(Car);
            }
            else if (Car.Direction == CarDirection.Left && Car.IsAtTurningPointLeft(Car, TrafficLight, Car.CurrentLane.ID))
            {
                if (CheckIfCarCanDriveAtTurningPoint(Rec, carHandlers))
                {
                    Car.IsIgnoringTrafficLight = true;
                    Car.IsDriving = true;
                    SetCarDirection(Car);
                }
                else
                {
                    Car.Stop(Car.CurrentLane.ID); // Car has to stop if another car is in the turning area
                    Car.IsDriving = false;
                    ChangeDirectionToStraightAfterDelay(Car, carHandlers); // Fährt  geradeaus weiter, falls er nach 3 Sekunden nicht abbiegen kann
                }

            }
            else if (Car.Direction == CarDirection.Right && Car.IsAtTurningPointRight(Car, TrafficLight, Car.CurrentLane.ID))
            {
                Car.IsIgnoringTrafficLight = true;
                SetCarDirection(Car);
            }
        }


        public async Task ChangeDirectionToStraightAfterDelay(CclContCar car, List<CclSvcHandleCar> carHandlers)
        {
            await Task.Delay(3000); // 3 Sekunden warten

            Car.IsDriving = true;
            car.Direction = CarDirection.Straight;
            SetCarDirection(car);
            car.StartOrContinueDriving(car.CurrentLane.ID);
        }

        public void CheckTrafficLightState(CclContCar car)
        {
            // Check the traffic light of the current car -> handle car behavior
            if (TrafficLight.CurrentState == TrafficLightState.Green)
            {
                // Car can drive
                car.IsDriving = true;
                car.StartOrContinueDriving(car.CurrentLane.ID);
                //SetCarDirection();
            }
            else
            {
                // Car has to stop
                car.IsDriving = false;
                car.Stop(car.CurrentLane.ID);
            }
        }

        public void SetCarDirection(CclContCar car)
        {
            // Check the car direction and handle the car behavior
            if (car.Direction == CarDirection.Left && !car.IsAlreadyTurned)
            {
                // Turn left
                car.TurnLeft(car.CurrentLane.ID);
                car.IsAlreadyTurned = true;
                car.Direction = CarDirection.Straight;
            }
            else if (car.Direction == CarDirection.Right && !car.IsAlreadyTurned)
            {
                // Turn right
                car.TurnRight(car.CurrentLane.ID);
                car.IsAlreadyTurned = true;
                car.Direction = CarDirection.Straight;
            }
            else if (car.Direction == CarDirection.Straight)
            {
                // Drive straight ahead
                car.StraightAhead(car.CurrentLane.ID);

            }

        }
        public bool CheckIfCarCanDriveAtTurningPoint2(Rectangle rec, List<CclSvcHandleCar> carHandler)
        {
            foreach (var currentCarHandler in carHandler)
            {
                int laneID = currentCarHandler.Car.CurrentLane.ID;
                switch (laneID)
                {
                    case 1:
                        if (rec.Contains((int)currentCarHandler.Car.PositionX, (int)currentCarHandler.Car.PositionY - 10))
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (rec.Contains((int)currentCarHandler.Car.PositionX - 10, (int)currentCarHandler.Car.PositionY))
                        {
                            return false;
                        }
                        break;
                    case 3:
                        if (rec.Contains((int)currentCarHandler.Car.PositionX, (int)currentCarHandler.Car.PositionY + 10))
                        {
                            return false;
                        }
                        break;
                    case 4:
                        if (rec.Contains((int)currentCarHandler.Car.PositionX + 10, (int)currentCarHandler.Car.PositionY))
                        {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;

        }





        public bool CheckIfCarCanDriveAtTurningPoint(Rectangle rec, List<CclSvcHandleCar> cars)
        {
            const int size = 2;      // Auto hat Größe 10x10
            const int forward = 10;   // 10px nach vorne checken
            const int right = 10;     // 3px rechts checken

            foreach (var handler in cars)
            {
                // Das Auto, das gerade betrachtet wird
                var car = handler.Car;

                // Skip: nicht gegen sich selbst prüfen
                if ((int)car.PositionX == rec.X && (int)car.PositionY == rec.Y)
                    continue;

                // Rechteck des anderen Autos
                Rectangle other = new Rectangle((int)car.PositionX, (int)car.PositionY, size, size);

                Rectangle forwardArea;
                Rectangle forwardRightArea;

                switch (handler.Car.CurrentLane.ID)
                {
                    // ↑ nach oben
                    case 1:
                        forwardArea = new Rectangle(rec.X, rec.Y - forward, size, size);
                        forwardRightArea = new Rectangle(rec.X + right, rec.Y - forward, size, size);
                        break;

                    // ← nach links
                    case 2:
                        forwardArea = new Rectangle(rec.X - forward, rec.Y, size, size);
                        forwardRightArea = new Rectangle(rec.X - forward, rec.Y - right, size, size);
                        break;

                    // ↓ nach unten
                    case 3:
                        forwardArea = new Rectangle(rec.X, rec.Y + forward, size, size);
                        forwardRightArea = new Rectangle(rec.X - right, rec.Y + forward, size, size);
                        break;

                    // → nach rechts
                    case 4:
                        forwardArea = new Rectangle(rec.X + forward, rec.Y, size, size);
                        forwardRightArea = new Rectangle(rec.X + forward, rec.Y + right, size, size);
                        break;

                    default:
                        continue;
                }

                // PRÜFEN: steht irgendein anderes Auto im Abbiegebereich?
                if (other.IntersectsWith(forwardArea) || other.IntersectsWith(forwardRightArea))
                {
                    return false;
                }
            }

            return true;
        }

    }
    }
