using AmpelSimulation.Classes.Container;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public CclSvcHandleCar(CclContCar car, CclContTrafficLight trafficLight, CclSvcHandleLight lightHandler)
        {
            Car = car;
            TrafficLight = trafficLight;
            LightHandler = lightHandler;
            if (!ReferenceEquals(car, Car)) return; // optional: nur das Service-Auto behandeln
            Car.PositionChanged += (s, e) =>
            {
                // Check if the car is at the traffic light position
                if (Car.IsAtTrafficLight(TrafficLight, Car.CurrentLane.ID) && !Car.IsIgnoringTrafficLight)
                {
                    CheckTrafficLightState(car);
                }
                else if (Car.Direction == CarDirection.Left && Car.IsAtTurningPointLeft(Car, TrafficLight, Car.CurrentLane.ID))
                { 
                    Car.IsIgnoringTrafficLight = true; 
                    SetCarDirection(Car);
                }
                else if (Car.Direction == CarDirection.Right && Car.IsAtTurningPointRight(Car, TrafficLight, Car.CurrentLane.ID))
                {
                    Car.IsIgnoringTrafficLight = true;
                    SetCarDirection(Car);
                }
            };
            LightHandler.StateChanged += (s, e) =>
            {
                if (Car.IsAtTrafficLight(TrafficLight, Car.CurrentLane.ID) && !Car.IsIgnoringTrafficLight)
                {
                    CheckTrafficLightState(car);
                }
            };

        }

        public void CheckTrafficLightState(CclContCar car)
        {
            // Check the traffic light of the current car -> handle car behavior
            if (TrafficLight.CurrentState == TrafficLightState.Green)
            {
                // Car can drive
                car.StartOrContinueDriving(car.CurrentLane.ID);
                //SetCarDirection();
            }
            //else if (State == TrafficLightState.Yellow)
            //{
            //    // Car can start or stop based on the light before
            //}
            else 
            {
                // Car has to stop
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
    }
}
