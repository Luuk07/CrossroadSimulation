using AmpelSimulation.Classes.Container;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace AmpelSimulation.Classes.Services
{
    public class CclSvcHandleCrossroad
    {

        //Handle crossroad logic here
        
        //
        public List <CclContTrafficLight> TrafficLights { get; set; }
        public List <CclContLane> Lanes { get; set; }
        public CclContCar Car { get; set; } 
        public CclSvcHandleCar CarHandler { get; set; }

        //
        public int SpaceBetweenCar { get; set; } = 10;
        public List<CclSvcHandleCar> l_CarHandler { get;set; } = new List<CclSvcHandleCar>();

        public CclSvcCreatAll Creat { get; set; } = new CclSvcCreatAll();
      
        public CclSvcHandleLight LightHandler { get; set; } = new CclSvcHandleLight();
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
            CarHandler = new CclSvcHandleCar(Car, trafficLight, LightHandler);
            l_CarHandler.Add(CarHandler);
        }
        // Move cars in the crossroad
        public void MoveCarsInCrossroad()
        {
            foreach (var carHandler in l_CarHandler)
            {
                // Move each car based on its handler
                if(IsDistanceBetweenCarInFrontEnough(carHandler))
                {
                    carHandler.Car.StraightAhead(carHandler.Car.CurrentLane.ID);
                    E_MoveCar?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        // Check distance between cars in the same line
        public bool IsDistanceBetweenCarInFrontEnough(CclSvcHandleCar currentCarHandler)
        {
           foreach (var CarHandler in l_CarHandler.Where(c=> c.Car.CurrentLane.ID == currentCarHandler.Car.CurrentLane.ID && !ReferenceEquals(c, currentCarHandler)))
            {
                switch(CarHandler.Car.CurrentLane.ID)
                {
                    case 1:
                        if (currentCarHandler.Car.PositionY - SpaceBetweenCar <= CarHandler.Car.PositionY)
                            return false;
                        break;
                    case 2:
                        if (currentCarHandler.Car.PositionX - SpaceBetweenCar <= CarHandler.Car.PositionX)
                            return false;
                        break;
                    case 3:
                        if (currentCarHandler.Car.PositionY + SpaceBetweenCar >= CarHandler.Car.PositionY)
                            return false;
                        break;
                    case 4:
                        if (currentCarHandler.Car.PositionX + SpaceBetweenCar >= CarHandler.Car.PositionX)
                            return false;
                        break;
                }
            }
            return true;
        }
    }
}
