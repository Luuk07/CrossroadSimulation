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

        public int yellowLightMilliSeconds  = 3000;


        // Methods 

        // Method to sync all traffic lights in the crossroad and give them the same mode
        public void SyncTrafficLights(TrafficLightMode Mode)
        {
            foreach (var light in TrafficLights)
            {
                light.SpeedChangeMode(Mode);
            }
        }
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
            // Verhindert parallele Umschaltungen
            if (System.Threading.Interlocked.Exchange(ref _isTransitioning, 1) == 1)
                return;


            try
            {
                // Snapshot vor Gelb
                var previous = TrafficLights.ToDictionary(l => l, l => l.CurrentState);

                // Alle auf Gelb
                foreach (var l in TrafficLights)
                {
                    SetState(l, TrafficLightState.Yellow);
                }

                // Gelb-Dauer
                await Task.Delay(yellowLightMilliSeconds).ConfigureAwait(false);

                // Zustände auf Basis von previous togglen
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
                        // Optional: Fallback KONSISTENT halten.
                        // Empfehlung: Einheitlich auf Red ODER auf Green,
                        // aber ohne ID-sonderlogik (die erzeugt Asymmetrien).
                        SetState(l, TrafficLightState.Red);
                    }
                }
            }
            finally
            {
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
