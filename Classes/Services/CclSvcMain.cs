using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AmpelSimulation.Classes.Services
{
    public class CclSvcMain
    {
        // Main service class to manage other services
        //
        private static System.Timers.Timer _timer;
        private int counter = 0;
        //
        public CclSvcHandleCrossroad CrossroadHandler { get; set; } 

        public CclSvcMain()
        {
            CrossroadHandler = new CclSvcHandleCrossroad();
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += MainTick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void MainTick(object sender, ElapsedEventArgs e)
        {
            counter++;
            CrossroadHandler.MoveCarsInCrossroad();
            if (counter == 500)
            {
                counter = 0;
                CrossroadHandler.PlaceNewCar();
            }

            //Später den Zwischenschritt mit SpeedOfChanging rausnehmen, ist eigentlich unnötig
            switch (CrossroadHandler.TrafficLights.FirstOrDefault().SpeedOfChanging)
            {
                case 1:
                    if (counter == 1000)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counter = 0;
                    }
                    break;
                case 2:
                    if (counter == 800)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counter = 0;
                    }
                    break;
                case 3:
                    if (counter == 600)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counter = 0;
                    }
                    break;
                case 4:
                    if (counter == 400)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counter = 0;
                    }
                    break;
                default:
                    break;


            }

        }
    }
}
