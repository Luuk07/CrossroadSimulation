using AmpelSimulation.Classes.Container;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace AmpelSimulation.Classes.Services
{
    public class CclSvcHandleCrossroad
    {

        //Handle crossroad logic here
        
        
        public List <CclContTrafficLight> TrafficLights { get; set; }
        public List <CclContLane> Lanes { get; set; }
        public CclContCar Car { get; set; } 
        public CclSvcHandleCar CarHandler { get; set; }

        //
        public int SpaceBetweenCar { get; set; } = 10;
        public List<CclSvcHandleCar> l_CarHandler { get;set; } = new List<CclSvcHandleCar>();

        public CclSvcCreatAll Creat { get; set; } = new CclSvcCreatAll();
      
        public CclSvcHandleLight LightHandler { get; set; } = new CclSvcHandleLight();

        public CclContStatistic Statistic { get; set; } = new CclContStatistic();

        public Rectangle Rec { get; set; }

        //

        public event EventHandler E_MoveCar;

        public CclSvcHandleCrossroad() 
        {
            Creat.CreateLanes();
            Creat.CreateTrafficLight();
            Lanes = Creat.l_AllLane;
            TrafficLights = Creat.TrafficLights;
            LightHandler.TrafficLights = TrafficLights;
            LightHandler.SyncTrafficLights(TrafficLightMode.ModeOne);
        }

        //Place new car in the crossroad
        public void PlaceNewCar()
        {
            Car = Creat.CreateNewCar();
            var trafficLight = TrafficLights.FirstOrDefault(tl => tl.ID == Car.CurrentLane.ID);
            var lane = Lanes.FirstOrDefault(l => l.ID == Car.CurrentLane.ID);    
            CarHandler = new CclSvcHandleCar(Car, trafficLight, LightHandler, l_CarHandler);
            lane.CarsInLane.Add(CarHandler);
            lane.LaneCountChanged();
            l_CarHandler.Add(CarHandler);
        }
        // Move cars in the crossroad
        public void MoveCarsInCrossroad()
        {
            foreach (var carHandler in l_CarHandler.ToList())
            {
                // Move each car based on its handler
                if (IsDistanceBetweenCarInFrontEnough(carHandler) && carHandler.Car.IsDriving)
                {
                   carHandler.Car.StraightAhead(carHandler.Car.CurrentLane.ID);
                   E_MoveCar?.Invoke(this, EventArgs.Empty);
                }
               
            }
        }

        // Check if the distance between the current car and the car in front is enough
        public bool IsDistanceBetweenCarInFrontEnough(CclSvcHandleCar currentCarHandler)
        {
            int laneID = currentCarHandler.Car.CurrentLane.ID;

            foreach (var c in l_CarHandler)
            {
                if (ReferenceEquals(c, currentCarHandler) || c.Car.CurrentLane.ID != laneID)
                    continue;

                // Check distance based on lane direction
                switch (laneID)
                {
                    case 1:
                        if (c.Car.PositionY < currentCarHandler.Car.PositionY &&
                            currentCarHandler.Car.PositionY - c.Car.PositionY < SpaceBetweenCar)
                            return false;
                        break;

                    case 2: 
                        if (c.Car.PositionX < currentCarHandler.Car.PositionX &&
                            currentCarHandler.Car.PositionX - c.Car.PositionX < SpaceBetweenCar)
                            return false;
                        break;

                    case 3: 
                        if (c.Car.PositionY > currentCarHandler.Car.PositionY &&
                            c.Car.PositionY - currentCarHandler.Car.PositionY < SpaceBetweenCar)
                            return false;
                        break;

                    case 4: 
                        if (c.Car.PositionX > currentCarHandler.Car.PositionX &&
                            c.Car.PositionX - currentCarHandler.Car.PositionX < SpaceBetweenCar)
                            return false;
                        break;
                }
            }

            return true;
        }


        // Remove car from crossroad when it passes the crossroad
        public void RemoveCarFromCrossroad()
        {
           E_MoveCar += (s, e) =>
            {
                foreach (var carHandler in l_CarHandler.ToList())
                {
                    if ((carHandler.Car.PositionX < -100|| carHandler.Car.PositionX>100) || (carHandler.Car.PositionY < -100 || carHandler.Car.PositionY >100))
                    {
                        l_CarHandler.Remove(carHandler);
                        Lanes.FirstOrDefault(l => l.ID == carHandler.Car.CurrentLane.ID).CarsInLane.Remove(carHandler);
                        Statistic.TotalCarsPassed += 1;

                    }
                }
                
            };
        }


    }
}
