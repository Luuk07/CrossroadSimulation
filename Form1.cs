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

        private SolidBrush brushRedOn = new SolidBrush(Color.FromArgb(255, 0, 0));
        private SolidBrush brushRedOff = new SolidBrush(Color.FromArgb(102, 0, 0));
        private SolidBrush brushYellowOn = new SolidBrush(Color.FromArgb(255, 255, 0));
        private SolidBrush brushYellowOff = new SolidBrush(Color.FromArgb(153, 153, 0));
        private SolidBrush brushGreenOn = new SolidBrush(Color.FromArgb(0, 255, 0));
        private SolidBrush brushGreenOff = new SolidBrush(Color.FromArgb(0, 102, 0));


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
            labelTimer.Text = $"Simulation Time:{Main.CrossroadHandler.Statistic.Timer.ToString()}s";


            // Subscribe to UI update event
            // If not UI Thread error
            Main.E_UIUpdate += (s, e) =>
            {
                // Check if form is disposed/closed
                if (this.IsDisposed) return;

                // Check if you are not on the UI thread
                if (this.InvokeRequired)
                {
                    // Code to run on the UI thread
                    this.BeginInvoke(new Action(() =>
                    {
                        UpdateUI();
                    }));
                }
                // Already on UI thread
                else
                {
                    UpdateUI();
                }
            };
        }

        // Methods
        private void UpdateUI()
        {
            labelCounter.Text =
                $"Cars passed: {Main.CrossroadHandler.Statistic.TotalCarsPassed}";

            labelTimer.Text =
                $"Simulation Time: {Main.CrossroadHandler.Statistic.Timer}s";
            this.Invalidate();
        }


        // Paint Method for Cars
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

        // Paint Method for Traffic Lights
        public void Form1_PaintTrafficLight(object sender, PaintEventArgs e)
        {
            int size = 4 * scaleFactor / 2;
            int space = scaleFactor / 2;
            int step = size + space;
            Graphics g = e.Graphics;
            foreach (var trafficLight in Main.CrossroadHandler.TrafficLights)
            {

                int x = (int)(trafficLight.PositionX * scaleFactor)-12;
                int y = (int)(trafficLight.PositionY * scaleFactor)-12;

                // Rechteck für die Ampel (Breite = 10, Höhe = 30)

                Rectangle rectRed = new Rectangle(x, y + 0 * step, size, size);
                Rectangle rectYellow = new Rectangle(x, y + 1 * step, size, size);
                Rectangle rectGreen = new Rectangle(x, y + 2 * step, size, size);


                if (trafficLight.CurrentState == TrafficLightState.Green)
                {       
                     g.FillRectangle(brushRedOff, rectRed);  
                     g.FillRectangle(brushYellowOff, rectYellow);
                     g.FillRectangle(brushGreenOn, rectGreen);
                }
                else if (trafficLight.CurrentState == TrafficLightState.Yellow)
                {
                     g.FillRectangle(brushRedOff, rectRed);
                     g.FillRectangle(brushYellowOn, rectYellow);
                     g.FillRectangle(brushGreenOff, rectGreen);
                }
                else if (trafficLight.CurrentState == TrafficLightState.Red)
                {
                     g.FillRectangle(brushRedOn, rectRed);
                     g.FillRectangle(brushYellowOff, rectYellow);
                     g.FillRectangle(brushGreenOff, rectGreen);   
                }
                g.DrawRectangle(Pens.Black, rectRed);
                g.DrawRectangle(Pens.Black, rectYellow);
                g.DrawRectangle(Pens.Black, rectGreen);
            }
        }

        //public void Form1_PaintTrafficLight2(object sender, PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    foreach (var trafficLight in Main.CrossroadHandler.TrafficLights)
        //    {
        //        // Rechteck für die Ampel (Breite = 10, Höhe = 30)
        //        Rectangle rect = new Rectangle(
        //            (int)trafficLight.PositionX * scaleFactor,
        //            (int)trafficLight.PositionY * scaleFactor,
        //            4 * scaleFactor/2, 4 * scaleFactor/2
        //        );
        //        if (trafficLight.CurrentState == TrafficLightState.Green)
        //        {
        //            using (Brush brush = new SolidBrush(Color.Green))
        //            {
        //                g.FillRectangle(brush, rect);
        //            }
        //        }
        //        else if (trafficLight.CurrentState == TrafficLightState.Yellow)
        //        {
        //            using (Brush brush = new SolidBrush(Color.Yellow))
        //            {
        //                g.FillRectangle(brush, rect);
        //            }
        //        }
        //        else if (trafficLight.CurrentState == TrafficLightState.Red)
        //        {
        //            using (Brush brush = new SolidBrush(Color.Red))
        //            {
        //                g.FillRectangle(brush, rect);
        //            }
        //        }
        //        g.DrawRectangle(Pens.Black, rect);
        //    }
        //}

        // Mode Button Click Events
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
