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

        public void SyncTrafficLights(TrafficLightMode Mode)
        {
            // Sync all traffic lights in the crossroad and give them the same mode
            foreach (var light in TrafficLights)
            {
                light.SpeedChangeMode(Mode);
            }
        }

        public void ChangeColorOfTrafficLight2()
        {
            foreach (var light in TrafficLights)
            {
                var lastState = light.CurrentState;
                if (light.CurrentState == TrafficLightState.Green || light.CurrentState == TrafficLightState.Red)
                {
                    light.CurrentState = TrafficLightState.Yellow;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
                else if (lastState == TrafficLightState.Red)
                {
                    light.CurrentState = TrafficLightState.Green;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
                else if (lastState == TrafficLightState.Green)
                {
                    light.CurrentState = TrafficLightState.Red;
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


        public async Task ChangeColorOfTrafficLight()
        {
            // Vorherige Zustände sichern (bevor wir auf Gelb gehen)
            var previous = TrafficLights.ToDictionary(l => l, l => l.CurrentState);

            // PHASE 1: Alle gleichzeitig auf Gelb
            foreach (var l in TrafficLights)
                SetState(l, TrafficLightState.Yellow);

            // Gemeinsame Wartezeit für alle (UI-freundlich, blockiert nicht)
            await Task.Delay(2000);

            // PHASE 2: Für alle gemeinsam weiter in den Zielzustand
            foreach (var l in TrafficLights)
            {
                var prev = previous[l];

                if (prev == TrafficLightState.Green)
                {
                    // Grün -> (Gelb) -> Rot
                    SetState(l, TrafficLightState.Red);
                }
                else if (prev == TrafficLightState.Red)
                {
                    // Rot -> (Gelb) -> Grün
                    SetState(l, TrafficLightState.Green);
                }
                else
                {
                    // Falls vorher schon Gelb/Unknown: Richtung anhand der ID bestimmen (deine Logik)
                    if (l.ID == 1 || l.ID == 3)
                        SetState(l, TrafficLightState.Red);
                    else
                        SetState(l, TrafficLightState.Green);
                }
            }
        }

        // Event-sicherer Setter
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
