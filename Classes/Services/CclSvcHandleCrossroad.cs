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
                    RemoveCarFromCrossroad();
                    E_MoveCar?.Invoke(this, EventArgs.Empty);
                }
                var a = IsDistanceBetweenCarInFrontEnough(carHandler);
            }
        }
        // Check distance between cars in the same line
        public bool IsDistanceBetweenCarInFrontEnough3(CclSvcHandleCar currentCarHandler)
        {
           foreach (var CarHandler in l_CarHandler.Where(c=> c.Car.CurrentLane.ID == currentCarHandler.Car.CurrentLane.ID && !ReferenceEquals(c, currentCarHandler)))
            {
                if (ReferenceEquals(CarHandler, currentCarHandler) || CarHandler.Car.CurrentLane.ID != currentCarHandler.Car.CurrentLane.ID) continue;
                switch (CarHandler.Car.CurrentLane.ID)
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




        public bool IsDistanceBetweenCarInFrontEnough2(CclSvcHandleCar currentCarHandler)
        {
            int laneId = currentCarHandler.Car.CurrentLane.ID;
            CclSvcHandleCar ahead = null;

            foreach (var c in l_CarHandler)
            {
                if (ReferenceEquals(c, currentCarHandler) || c.Car.CurrentLane.ID != laneId)
                    continue;

                switch (laneId)
                {
                    case 1: // nach oben (-Y)
                        if (c.Car.PositionY < currentCarHandler.Car.PositionY &&
                            (ahead == null || c.Car.PositionY > ahead.Car.PositionY))
                            ahead = c;
                        break;

                    case 2: // nach links (-X)
                        if (c.Car.PositionX < currentCarHandler.Car.PositionX &&
                            (ahead == null || c.Car.PositionX > ahead.Car.PositionX))
                            ahead = c;
                        break;

                    case 3: // nach unten (+Y)
                        if (c.Car.PositionY > currentCarHandler.Car.PositionY &&
                            (ahead == null || c.Car.PositionY < ahead.Car.PositionY))
                            ahead = c;
                        break;

                    case 4: // nach rechts (+X)
                        if (c.Car.PositionX > currentCarHandler.Car.PositionX &&
                            (ahead == null || c.Car.PositionX < ahead.Car.PositionX))
                            ahead = c;
                        break;
                }
            }

            if (ahead == null) return true;

            double gap = 0;
            switch (laneId)
            {
                case 1: gap = currentCarHandler.Car.PositionY - ahead.Car.PositionY; break;
                case 2: gap = currentCarHandler.Car.PositionX - ahead.Car.PositionX; break;
                case 3: gap = ahead.Car.PositionY - currentCarHandler.Car.PositionY; break;
                case 4: gap = ahead.Car.PositionX - currentCarHandler.Car.PositionX; break;
            }

            return gap >= SpaceBetweenCar;
        }


        public bool IsDistanceBetweenCarInFrontEnough(CclSvcHandleCar currentCarHandler)
        {
            int laneId = currentCarHandler.Car.CurrentLane.ID;

            foreach (var c in l_CarHandler)
            {
                if (ReferenceEquals(c, currentCarHandler) || c.Car.CurrentLane.ID != laneId)
                    continue;

                // Prüfen je nach Richtung
                switch (laneId)
                {
                    case 1: // nach oben (-Y)
                        if (c.Car.PositionY < currentCarHandler.Car.PositionY &&
                            currentCarHandler.Car.PositionY - c.Car.PositionY < SpaceBetweenCar)
                            return false;
                        break;

                    case 2: // nach links (-X)
                        if (c.Car.PositionX < currentCarHandler.Car.PositionX &&
                            currentCarHandler.Car.PositionX - c.Car.PositionX < SpaceBetweenCar)
                            return false;
                        break;

                    case 3: // nach unten (+Y)
                        if (c.Car.PositionY > currentCarHandler.Car.PositionY &&
                            c.Car.PositionY - currentCarHandler.Car.PositionY < SpaceBetweenCar)
                            return false;
                        break;

                    case 4: // nach rechts (+X)
                        if (c.Car.PositionX > currentCarHandler.Car.PositionX &&
                            c.Car.PositionX - currentCarHandler.Car.PositionX < SpaceBetweenCar)
                            return false;
                        break;
                }
            }

            return true;
        }



        public void RemoveCarFromCrossroad()
        {
            foreach (var carHandler in l_CarHandler)
            {
                if ((carHandler.Car.PositionX <0|| carHandler.Car.PositionX>1000) || (carHandler.Car.PositionY <0 || carHandler.Car.PositionY >1000))
                {
                    l_CarHandler.Remove(carHandler);
                }
            }
        }


    }
}
