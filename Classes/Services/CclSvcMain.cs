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
        private int counterCarPlace = 0;
        private int counterTrafficLight = 0;
        //
        public CclSvcHandleCrossroad CrossroadHandler { get; set; } 

        public event EventHandler E_PlaceNewCar;
        public CclSvcMain()
        {
            try
            {
                CrossroadHandler = new CclSvcHandleCrossroad();
                _timer = new System.Timers.Timer(10);
                _timer.Elapsed += MainTick;
                _timer.AutoReset = true;
                _timer.Enabled = false;
            }
            finally
            {
                _timer.Enabled = true;
            }
        }

        private void MainTick(object sender, ElapsedEventArgs e)
        {
            counterCarPlace++;
            counterTrafficLight++;
            CrossroadHandler.MoveCarsInCrossroad();
            if (counterCarPlace == 200)
            {
                _timer.Stop();
                counterCarPlace = 0;
                CrossroadHandler.PlaceNewCar();
                E_PlaceNewCar.Invoke(this, EventArgs.Empty);
                _timer.Start();
            }

            //Später den Zwischenschritt mit SpeedOfChanging rausnehmen, ist eigentlich unnötig
            switch (CrossroadHandler.TrafficLights.FirstOrDefault().SpeedOfChanging)
            {
                case 1:
                    if (counterTrafficLight == 700)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counterTrafficLight = 0;
                    }
                    break;
                case 2:
                    if (counterTrafficLight == 400)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counterTrafficLight = 0;
                    }
                    break;
                case 3:
                    if (counterTrafficLight == 300)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counterTrafficLight = 0;
                    }
                    break;
                case 4:
                    if (counterTrafficLight == 200)
                    {
                        CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        counterTrafficLight = 0;
                    }
                    break;
                default:
                    break;


            }

        }
    }
}
