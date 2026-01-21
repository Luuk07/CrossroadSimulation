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

        public int scaleFactor = 5;
        public Form1()
        {
            Main = new CclSvcMain();
            InitializeComponent();
            this.Paint += Form1_PaintCar;
            this.Paint += Form1_PaintTrafficLight;
            this.DoubleBuffered = true;
            Main.E_PlaceNewCar += (s, e) => this.Invalidate();
            Main.CrossroadHandler.E_MoveCar += (s, e) => this.Invalidate();
            labelCounter.Text = $"Cars passed:{Main.CrossroadHandler.Statistic.TotalCarsPassed.ToString()}";

        }

        public void Form1_PaintCar(object sender, PaintEventArgs e) 
        {
            Graphics g = e.Graphics;
            foreach (var CarHandler in Main.CrossroadHandler.l_CarHandler.ToList())// Erzeugt Momentaufnahme
            {
                // Rechteck für das Auto (Breite = 20, Höhe = 10)
                Rectangle rect = new Rectangle(
                    (int)CarHandler.Car.PositionX * scaleFactor,
                    (int)CarHandler.Car.PositionY * scaleFactor,
                    10 ,10
                );
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, rect);
                }
                g.DrawRectangle(Pens.Black, rect);
            }
            labelCounter.Text = $"Cars passed:{Main.CrossroadHandler.Statistic.TotalCarsPassed.ToString()}";
        }

        public void Form1_PaintTrafficLight(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (var trafficLight in Main.CrossroadHandler.TrafficLights)
            {
                // Rechteck für die Ampel (Breite = 10, Höhe = 30)
                Rectangle rect = new Rectangle(
                    (int)trafficLight.PositionX * scaleFactor,
                    (int)trafficLight.PositionY * scaleFactor,
                    4 * scaleFactor/2, 4 * scaleFactor/2
                );
                if (trafficLight.CurrentState == TrafficLightState.Green)
                {
                    using (Brush brush = new SolidBrush(Color.Green))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
                else if (trafficLight.CurrentState == TrafficLightState.Yellow)
                {
                    using (Brush brush = new SolidBrush(Color.Yellow))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
                else if (trafficLight.CurrentState == TrafficLightState.Red)
                {
                    using (Brush brush = new SolidBrush(Color.Red))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
              
                
                g.DrawRectangle(Pens.Black, rect);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeOne);
            button1.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeTwo);
            button1.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeThree);
            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeFour);
            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = false;
        }
    }
}
