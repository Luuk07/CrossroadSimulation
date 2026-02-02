using AmpelSimulation.Classes.Container;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Services
{
    public class CclSvcHandleLight
    {
        public List<CclContTrafficLight> TrafficLights { get; set; } = new List<CclContTrafficLight>();

        public event EventHandler StateChanged;

        private int _isTransitioning = 0;

        public double yellowLightMilliSeconds  = 3000;


        // Methods 

        // Method to sync all traffic lights in the crossroad and give them the same mode
        //public void SyncTrafficLights(TrafficLightMode Mode)
        //{
        //    foreach (var light in TrafficLights)
        //    {
        //        light.SpeedChangeMode(Mode);
        //    }
        //}
        public void SetAllTrafficLightsRedLightSeconds(int redLightSeconds)
        {
            foreach (var light in TrafficLights)
            {
                light.RedLightSeconds = redLightSeconds;
            }
        }

        // Method to change the color of all traffic lights in the crossroad
        public async Task ChangeColorOfTrafficLight()
        {
            //Avoid parallel transitions,so isnt possible that the method is called again while transitioning
            if (System.Threading.Interlocked.Exchange(ref _isTransitioning, 1) == 1)
                return;

            try
            {
                // Snapshot previous states
                var previous = TrafficLights.ToDictionary(l => l, l => l.CurrentState);

                // All traffic lights to Yellow
                foreach (var l in TrafficLights)
                {
                    SetState(l, TrafficLightState.Yellow);
                }

                // Duration of yellow light
                await Task.Delay((int)yellowLightMilliSeconds).ConfigureAwait(false);

                // Set new states based on previous
                foreach (var l in TrafficLights)
                {
                    var prev = previous[l];

                    if (prev == TrafficLightState.Green)
                    {
                        SetState(l, TrafficLightState.Red);
                    }
                    else if (prev == TrafficLightState.Red)
                    {
                        SetState(l, TrafficLightState.Green);
                    }
                    else
                    {
                        // Here we choose Red as a safe default
                        SetState(l, TrafficLightState.Red);
                    }
                }
            }
            finally
            {
                // Reset transitioning flag, so its possible to call the method again
                System.Threading.Interlocked.Exchange(ref _isTransitioning, 0);
            }

        }

        // Helper method to set the state and trigger event if changed
        private void SetState(CclContTrafficLight light, TrafficLightState newState)
        {
            if (light.CurrentState != newState)
            {
                light.CurrentState = newState;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }


    }
}
