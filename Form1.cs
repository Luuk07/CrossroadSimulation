using AmpelSimulation.Classes.Services;
using AmpelSimulation.Classes.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmpelSimulation
{
    public partial class Form1 : Form
    {
        public CclSvcMain Main { get; set; }
        public Form1()
        {
            Main = new CclSvcMain();
            InitializeComponent();
            this.Paint += Form1_PaintCar;
            this.Paint += Form1_PaintTrafficLight;
            this.DoubleBuffered = true;
            Main.E_PlaceNewCar += (s, e) => this.Invalidate();
            Main.CrossroadHandler.E_MoveCar += (s, e) => this.Invalidate();

        }

        public void Form1_PaintCar(object sender, PaintEventArgs e) 
        {
            Graphics g = e.Graphics;
            foreach (var CarHandler in Main.CrossroadHandler.l_CarHandler)
            {
                // Rechteck für das Auto (Breite = 20, Höhe = 10)
                Rectangle rect = new Rectangle(
                    (int)CarHandler.Car.PositionX,
                    (int)CarHandler.Car.PositionY,
                    5, 5
                );
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, rect);
                }
                g.DrawRectangle(Pens.Black, rect);
            }
        }

        public void Form1_PaintTrafficLight(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (var trafficLight in Main.CrossroadHandler.TrafficLights)
            {
                // Rechteck für die Ampel (Breite = 10, Höhe = 30)
                Rectangle rect = new Rectangle(
                    (int)trafficLight.PositionX,
                    (int)trafficLight.PositionY,
                    4, 4
                );
                if (trafficLight.CurrentState == TrafficLightState.Green)
                {
                    using (Brush brush = new SolidBrush(Color.Green))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
                else
                {
                    using (Brush brush = new SolidBrush(Color.Red))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
              
                
                g.DrawRectangle(Pens.Black, rect);
            }
        }
    }
}
