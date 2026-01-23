using AmpelSimulation.Classes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpelSimulation.Classes.Container
{
    public class CclContLane
    {
        public event EventHandler E_LaneCountChanged;
        //
        public int ID { get; set; }

        public int Width { get; set; } = 10;
        public List<CclSvcHandleCar> CarsInLane { get; set;} = new List<CclSvcHandleCar>();

        // Methods

        //Method to invoke event when lane count changes
        public void LaneCountChanged()
        {
            E_LaneCountChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
