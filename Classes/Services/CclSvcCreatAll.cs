using AmpelSimulation.Classes.Container;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Services
{
    public class CclSvcCreatAll
    {
        public List<CclContTrafficLight> TrafficLights { get; set; } = new List<CclContTrafficLight>();
        public List <CclContLane> l_AllLane { get; set; } = new List<CclContLane>();
        public List<string> Colours = new List<string>()
        {
            "Blau", "Grün", "Rot",
            "Gelb","Schwarz", "Weiß", "Pink", "Lila", 
        };

        private CclContLane GetLaneById(int id) => l_AllLane.First(l => l.ID == id);

        //Method to create a new car
        public CclContCar CreateNewCar()
        {
            // Create car logic here
            CclContCar car = new CclContCar()
            {
                Color = Colours[CclRandom.Random.Next(Colours.Count)],
                PS = CclRandom.Random.Next(60, 501),
                Weight = CclRandom.Random.Next(800, 2501),
                CurrentLane = l_AllLane[CclRandom.Random.Next(l_AllLane.Count)],  
                Direction = (CarDirection)CclRandom.Random.Next(1, 4),
            };
            // Car get laneID resolver based on current lane 
            car.ResolveLaneById = GetLaneById;

            // Set initial position based on lane
            switch (car.CurrentLane.ID)
            {
                case 1:
                    car.PositionX = 65;
                    car.PositionY = 100;
                    break;
                case 2:
                    car.PositionX = 100;
                    car.PositionY = 55;
                    break;
                case 3:
                    car.PositionX = 55;
                    car.PositionY = 0;
                    break;
                default:
                    car.PositionX = 0;
                    car.PositionY = 65;
                    break;
            }
            return car;

        }

        //Method to create lanes
        public void CreateLanes()
        {
            for (int i = 0; i < 4; i++)
            {
                CclContLane Lane = new CclContLane()
                {
                    ID = i + 1,
                };
                l_AllLane.Add(Lane);
            }
        }

        //Method to create traffic lights
        public void CreateTrafficLight()
        {
            foreach (var lane in l_AllLane)
            {
                CclContTrafficLight trafficLight = new CclContTrafficLight()
                {
                    CurrentLane = lane,
                    ID = lane.ID,
                };

                // Set position and initial state based on lane
                switch (lane.ID)
                {
                    case 1:
                        trafficLight.PositionX = 70;
                        trafficLight.PositionY = 70;
                        trafficLight.CurrentState = TrafficLightState.Green;
                        break;
                    case 2:
                        trafficLight.PositionX = 70;
                        trafficLight.PositionY = 50;
                        trafficLight.CurrentState = TrafficLightState.Red;
                        break;
                    case 3:
                        trafficLight.PositionX = 50;
                        trafficLight.PositionY = 50;
                        trafficLight.CurrentState = TrafficLightState.Green;
                        break;
                    default:
                        trafficLight.PositionX = 50;
                        trafficLight.PositionY = 70;
                        trafficLight.CurrentState = TrafficLightState.Red;
                        break;
                }
                TrafficLights.Add(trafficLight);
            }

            
            
        }
    }
}
