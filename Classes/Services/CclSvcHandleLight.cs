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


        // Methods 

        // Method to sync all traffic lights in the crossroad and give them the same mode
        public void SyncTrafficLights(TrafficLightMode Mode)
        {
            foreach (var light in TrafficLights)
            {
                light.SpeedChangeMode(Mode);
            }
        }

        // Method to change the color of all traffic lights in the crossroad
        public async Task ChangeColorOfTrafficLight()
        {
            // To hold the previous states before changing to Yellow
            var previous = TrafficLights.ToDictionary(l => l, l => l.CurrentState);

            //All traffic lights to Yellow first
            foreach (var l in TrafficLights)
                SetState(l, TrafficLightState.Yellow);

            //Wait for 2 seconds
            await Task.Delay(3000);

            // Change to the next state based on previous state
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
                    if (l.ID == 1 || l.ID == 3)
                        SetState(l, TrafficLightState.Red);
                    else
                        SetState(l, TrafficLightState.Green);
                }
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
