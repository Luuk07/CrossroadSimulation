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
        public List <CclContTrafficLight> TrafficLights { get; set; } = new List<CclContTrafficLight>();

        public event EventHandler StateChanged;

        public void SyncTrafficLights(TrafficLightMode Mode)
        {
            // Sync all traffic lights in the crossroad and give them the same mode
            foreach (var light in TrafficLights)
            {
                light.SpeedChangeMode(Mode);
            }
        }

        public void ChangeColorOfTrafficLight()
        {
            
            foreach(var light in TrafficLights)
            {
                if (light.CurrentState == TrafficLightState.Green)
                { 
                    light.CurrentState = TrafficLightState.Red;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
                else if (light.CurrentState == TrafficLightState.Red)
                {
                    light.CurrentState = TrafficLightState.Green;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    switch (light.ID)
                    {
                        case 1:
                            light.CurrentState = TrafficLightState.Green;
                            break;
                        case 2:
                            light.CurrentState = TrafficLightState.Red;
                            break;
                        case 3:
                            light.CurrentState = TrafficLightState.Green;
                            break;
                        case 4:
                            light.CurrentState = TrafficLightState.Red;
                            break;
                        default:
                            break;
                    }
                }
            }
            
        }
    }
}
