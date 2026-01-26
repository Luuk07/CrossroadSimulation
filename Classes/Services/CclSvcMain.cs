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
                _timer = new System.Timers.Timer(12);
                _timer.Elapsed += MainTick;
                _timer.AutoReset = true;
                _timer.Enabled = false;
            }
            finally
            {
                _timer.Enabled = true;
            }
        }

        // Methods

        // Main timer tick method
        private async void MainTick(object sender, ElapsedEventArgs e)
        {

            counterCarPlace++;
            counterTrafficLight++;
            CrossroadHandler.MoveCarsInCrossroad();
            // Place new car every 0,8 second
            if (counterCarPlace >= 50)
            {
                _timer.Stop();
                counterCarPlace = 0;
                CrossroadHandler.PlaceNewCar();
                E_PlaceNewCar.Invoke(this, EventArgs.Empty);
                _timer.Start();
            }

            // Change traffic light based on its speed of changing
            switch (CrossroadHandler.TrafficLights.FirstOrDefault().SpeedOfChanging)
            {
                case 1:
                    if (counterTrafficLight >= 700)
                    {
                        counterTrafficLight = 0;
                        await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        CrossroadHandler.RemoveCarFromCrossroad();
                    }
                    break;
                case 2:
                    if (counterTrafficLight >= 400)
                    {
                        counterTrafficLight = 0;
                        await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        CrossroadHandler.RemoveCarFromCrossroad();
                    }
                    break;
                case 3:
                    if (counterTrafficLight >= 300)
                    {
                        counterTrafficLight = 0;
                        await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        CrossroadHandler.RemoveCarFromCrossroad();
                    }
                    break;
                case 4:
                    if (counterTrafficLight >= 200)
                    {
                        counterTrafficLight = 0;
                        await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                        CrossroadHandler.RemoveCarFromCrossroad();
                    }
                    break;
                default:
                    break;


            }

        }
    }
}
