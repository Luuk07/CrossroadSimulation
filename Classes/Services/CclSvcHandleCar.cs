using AmpelSimulation.Classes.Container;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            // Check if the car is at the traffic light position
            Car.PositionChanged += (s, e) =>
            {
                if (Car.IsAtTrafficLight(TrafficLight, Car.CurrentLane.ID))
                {
                    CheckTrafficLightState();
                }
                if (Car.Direction == CarDirection.Left && Car.IsAtTurningPointLeft(TrafficLight, Car.CurrentLane.ID))
                {
                    SetCarDirection();
                }
                if (Car.Direction == CarDirection.Right && Car.IsAtTurningPointRight(TrafficLight, Car.CurrentLane.ID))
                {
                    SetCarDirection();
                }
            };
            //LightHandler.StateChanged += (s, e) =>
            //{
            //    if (Car.IsAtTrafficLight(TrafficLight, LaneID))
            //    {
            //        CheckTrafficLightState();
            //    }
            //};

        }

        public void CheckTrafficLightState()
        {
            // Check the traffic light of the current car -> handle car behavior
            if (TrafficLight.CurrentState == TrafficLightState.Green)
            {
                // Car can drive
                Car.StartOrContinueDriving(Car.CurrentLane.ID);
                //SetCarDirection();
            }
            //else if (State == TrafficLightState.Yellow)
            //{
            //    // Car can start or stop based on the light before
            //}
            else 
            {
                // Car has to stop
                Car.Stop(Car.CurrentLane.ID);
            }
        }

        public void SetCarDirection()
        {
            // Check the car direction and handle the car behavior
                if (Car.Direction == CarDirection.Left)
                {
                    // Turn left
                    Car.TurnLeft(Car.CurrentLane.ID);
                }
                else if (Car.Direction == CarDirection.Right)
                {
                    // Turn right
                    Car.TurnRight(Car.CurrentLane.ID);
                }
                else if (Car.Direction == CarDirection.Straight)
                {
                    // Drive straight ahead
                    Car.StraightAhead(Car.CurrentLane.ID);
                }
        }

        // Handle car behavior at crossroad based on traffic light state
        public void CarBehaviorAtCrossroad()
        {

            CheckTrafficLightState();

        }


    }
}
