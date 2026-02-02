using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        //private int counterTrafficLight = 0;
        private int counterCarPlace = 0;
        private int counterTimer = 0;
        private int currentStoppedCars = 0;


        public int intervalTimer = 1; 

        public double multipleTempo = 1; // Speed of simulation 

        //
        public CclSvcHandleCrossroad CrossroadHandler { get; set; }

        public event EventHandler E_PlaceNewCar;

        public event EventHandler E_UIUpdate;

        public event EventHandler E_Done;
        //
        public bool IsTrafficLightAlreadySwitched { get; set; } = false; 
        public CclSvcMain()
        {
            try
            {
                CrossroadHandler = new CclSvcHandleCrossroad();
                _timer = new System.Timers.Timer(intervalTimer);
                _timer.Elapsed += MainTick;
                _timer.AutoReset = true;
                _timer.Enabled = false;
                ScaleYellowLightDurationBasedOnIntervall(multipleTempo);
            }
            finally
            {
                _timer.Enabled = true;
            }
        }

        // Methods

        // Main timer tick method
        private void MainTick(object sender, ElapsedEventArgs e)
        {
            foreach (var car in CrossroadHandler.l_CarHandler.Where(c=>c.Car.Speed == 0).ToList())
            {
                currentStoppedCars++;
            }
            counterCarPlace++;
            //counterTrafficLight++;
            counterTimer++;
            CrossroadHandler.MoveCarsInCrossroad();
            CrossroadHandler.RemoveCarFromCrossroad();
            // Update timer every second if tick every 12 ms
            if (counterTimer >= 83/ multipleTempo)
            {
                counterTimer = 0;
                CrossroadHandler.Statistic.Timer++;
                E_UIUpdate?.Invoke(this, EventArgs.Empty);
            }
            // Place new car every 0,8 second
            if (counterCarPlace >= 50/multipleTempo)
            {
                _timer.Stop();
                counterCarPlace = 0;
                CrossroadHandler.PlaceNewCar(multipleTempo);
                ScaleWaitingTimeForChangingBasedOnIntervall(multipleTempo);
                //ScaleCarSpeedBasedOnIntervall(multipleTempo);
                E_PlaceNewCar?.Invoke(this, EventArgs.Empty);
                E_UIUpdate?.Invoke(this, EventArgs.Empty);
                _timer.Start();
            }
            CheckSimulationState();
            TrafficLightSetup();
            //if (CrossroadHandler.Statistic.Timer >= 30)
            //{
            //    switch (CrossroadHandler.TrafficLights.FirstOrDefault().CurrentMode)
            //    {
            //        case TrafficLightMode.ModeOne:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeTwo);
            //            break;
            //        case TrafficLightMode.ModeTwo:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeThree);
            //            break;
            //        case TrafficLightMode.ModeThree:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeFour);
            //            break;
            //        case TrafficLightMode.ModeFour:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeOne);
            //            // Das Endergebnis der Statistik anzeigen
            //            break;
            //        default:
            //            break;
            //    }
            //    CrossroadHandler.Statistic.Timer = 0;
            //}

            //if (CrossroadHandler.Statistic.Timer >= 30)
            //{
            //    switch (CrossroadHandler.TrafficLights.FirstOrDefault().CurrentMode)
            //    {
            //        case TrafficLightMode.ModeOne:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeTwo);
            //            break;
            //        case TrafficLightMode.ModeTwo:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeThree);
            //            break;
            //        case TrafficLightMode.ModeThree:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeFour);
            //            break;
            //        case TrafficLightMode.ModeFour:
            //            CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeOne);  
            //            break;
            //        default:
            //            break;
            //    }
            //    CrossroadHandler.Statistic.Timer = 0;
            //}



            // Change traffic light based on its speed of changing
            //switch (CrossroadHandler.TrafficLights.FirstOrDefault().SpeedOfChanging)
            //{
            //    case 1:
            //        if (counterTrafficLight >= 700)
            //        {
            //            counterTrafficLight = 0;
            //            await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
            //        }
            //        break;
            //    case 2:
            //        if (counterTrafficLight >= 400)
            //        {
            //            counterTrafficLight = 0;
            //            await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
            //        }
            //        break;
            //    case 3:
            //        if (counterTrafficLight >= 300)
            //        {
            //            counterTrafficLight = 0;
            //            await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
            //        }
            //        break;
            //    case 4:
            //        if (counterTrafficLight >= 200)
            //        {
            //            counterTrafficLight = 0;
            //            await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight(); 
            //        }
            //        break;
            //    default:
            //        break;
            //}

        }
        // Traffic light setup method
        public async void TrafficLightSetup()
        {
            //Check if the traffic light has already been switched in this cycle
            if (!IsTrafficLightAlreadySwitched)
            {
                // Check the red light seconds of the traffic lights and change color accordingly
                switch (CrossroadHandler.LightHandler.TrafficLights.FirstOrDefault().RedLightSeconds)
                {
                    case 1:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 1)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 2)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 3)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 4)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 5)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 6:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 6)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 7:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 7)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 8:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 8)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 9:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 9)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 10:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 10)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 11:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 11)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 12:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 12)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 13:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 13)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 14:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 14)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 15:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 15)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 16:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 16)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 17:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 17)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 18:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 18)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 19:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 19)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 20:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 20)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }

                    case 21:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 21)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 22:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 22)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 23:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 23)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 24:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 24)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 25:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 25)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 26:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 26)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 27:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 27)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 28:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 28)
                            {
                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    case 29:
                        {
                            if (CrossroadHandler.Statistic.Timer >= 29)
                            {

                                await CrossroadHandler.LightHandler.ChangeColorOfTrafficLight();
                                IsTrafficLightAlreadySwitched = true;
                            }
                            break;
                        }
                    default:
                        {                    
                            //E_Done?.Invoke(this, EventArgs.Empty);
                            break;
                        }
                }
            }
        }

        public void CheckSimulationState()
        {
            //30 seconds one passage end plus 2 seconds yellow light
            if (CrossroadHandler.Statistic.Timer >= 32)
            {
                // Reducing the red light seconds for next passage
                switch (CrossroadHandler.TrafficLights.FirstOrDefault().RedLightSeconds)
                {
                    case 29: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(28); break; }
                    case 28: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(27); break; }
                    case 27: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(26); break; }
                    case 26: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(25); break; }
                    case 25: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(24); break; }
                    case 24: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(23); break; }
                    case 23: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(22); break; }
                    case 22: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(21); break; }
                    case 21: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(20); break; }

                    case 20: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(19); break; }
                    case 19: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(18); break; }
                    case 18: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(17); break; }
                    case 17: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(16); break; }
                    case 16: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(15); break; }
                    case 15: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(14); break; }
                    case 14: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(13); break; }
                    case 13: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(12); break; }
                    case 12: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(11); break; }
                    case 11: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(10); break; }

                    case 10: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(9); break; }
                    case 9: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(8); break; }
                    case 8: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(7); break; }
                    case 7: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(6); break; }
                    case 6: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(5); break; }
                    case 5: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(4); break; }
                    case 4: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(3); break; }
                    case 3: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(2); break; }
                    case 2: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(1); break; }
                    case 1: { CrossroadHandler.LightHandler.SetAllTrafficLightsRedLightSeconds(0); 
                            E_Done?.Invoke(this, EventArgs.Empty); 
                            _timer.Stop();
                            break; } 
                    default:
                        {
                            _timer.Stop();
                            break;
                        }
                }
                IsTrafficLightAlreadySwitched = false;
                CrossroadHandler.l_CarHandler.Clear();
                CrossroadHandler.Statistic.Timer = 0;
                CrossroadHandler.Statistic.AddCoutOfStopedCarsToList(currentStoppedCars);
                currentStoppedCars = 0;
            }
        }

        // Scale yellow light duration based on Speed of simulation 
        public void ScaleYellowLightDurationBasedOnIntervall(double multiple)
        {
            CrossroadHandler.LightHandler.yellowLightMilliSeconds = CrossroadHandler.LightHandler.yellowLightMilliSeconds/ multiple;
        }

        // Scale Waiting Time duration based on Speed of simulation 
        public void ScaleWaitingTimeForChangingBasedOnIntervall(double multiple)
        {
            CrossroadHandler.CarHandler.waitingTimeForChanging = CrossroadHandler.CarHandler.waitingTimeForChanging / multiple;
        }

        //public void ScaleCarSpeedBasedOnIntervall(double multiple)
        //{
        //    CrossroadHandler.CarHandler.Car.Speed = CrossroadHandler.CarHandler.Car.Speed * multiple;
        //}
    }
}
