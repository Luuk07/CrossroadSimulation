using AmpelSimulation.Classes.Services;
using AmpelSimulation.Classes.Tools;
using System;
using System.Drawing;
using System.Linq;
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
            this.Paint += Form1_PaintLanes;
            this.DoubleBuffered = true;
            Main.E_PlaceNewCar += (s, e) => this.Invalidate();
            Main.CrossroadHandler.E_MoveCar += (s, e) => this.Invalidate();
            labelCounter.Text = $"Cars passed:{Main.CrossroadHandler.Statistic.TotalCarsPassed.ToString()}";
            labelTimer.Text = $"Simulation Time:{Main.CrossroadHandler.Statistic.Timer.ToString()}s";
            //trackBarOfSimSpeed.Value = Main.multipleTempo;
            Main.E_Done += (s, e) =>
            {
                int lowestWaitingTime = Main.CrossroadHandler.Statistic.ListOfWaitingTimes.Min();
                MessageBox.Show(
                    string.Join(Environment.NewLine, Main.CrossroadHandler.Statistic.ListOfWaitingTimes),
                    "Wartezeiten"
                );
                MessageBox.Show(
                    $"Niedrigste Wartezeit: {lowestWaitingTime} Sekunden, bei Schaltung {Main.CrossroadHandler.Statistic.ListOfWaitingTimes.FindIndex(wt => wt == lowestWaitingTime) + 1}",
                    "Statistik"
                );
            };

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
                        //Main.multipleTempo = trackBarOfSimSpeed.Value;
                        UpdateUI();
                    }));
                }
                // Already on UI thread
                else
                {
                    //Main.multipleTempo = trackBarOfSimSpeed.Value;
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
            foreach (var CarHandler in Main.CrossroadHandler.l_CarHandler.ToList())
            {
                
                Rectangle rect = new Rectangle(
                    (int)CarHandler.Car.PositionX * scaleFactor,
                    (int)CarHandler.Car.PositionY * scaleFactor,
                    10, 10
                );
                Rectangle rectLeftCorner = new Rectangle(
                          (int)CarHandler.Car.PositionX * scaleFactor,
                          (int)CarHandler.Car.PositionY * scaleFactor,
                          4, 4
                );
                Rectangle rectRightCorner = new Rectangle(
                        (int)CarHandler.Car.PositionX * scaleFactor,
                        (int)CarHandler.Car.PositionY * scaleFactor,
                        4, 4
              );

                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, rect);
                }
                g.DrawRectangle(Pens.Black, rect);

                if (CarHandler.Car.Direction == CarDirection.Left)
                {
                    switch (CarHandler.Car.CurrentLane.ID) 
                    {
                        case 1:
                            break;
                        case 2:
                            rectLeftCorner.Y += 6;
                            break;  
                        case 3:
                            rectLeftCorner.X += 6;
                            rectLeftCorner.Y += 6;
                            break;
                        case 4:
                            rectLeftCorner.X += 6;
                            break;
                        default:
                            break;
                    }
                    if (CarHandler.HasColorForTurning == false)
                    {
                        using (Brush brush = new SolidBrush(Color.Orange))
                        {
                            g.FillRectangle(brush, rectLeftCorner);
                        }
                        _ = CarHandler.SetColorForTurningAsync(true); //_ = to avoid warning, starting Method without await
                    }
                    else
                    {
                        using (Brush brush = new SolidBrush(Color.Black))
                        {
                            g.FillRectangle(brush, rectLeftCorner);
                        }
                        _ = CarHandler.SetColorForTurningAsync(false);
                    }
                   
                }
                if (CarHandler.Car.Direction == CarDirection.Right)
                {
                    switch (CarHandler.Car.CurrentLane.ID)
                    {
                        case 1:
                            rectRightCorner.X += 6;
                            break;
                        case 2:
                            break;
                        case 3:
                            rectRightCorner.Y += 6;
                            break;
                        case 4:
                            rectRightCorner.X += 6;
                            rectRightCorner.Y += 6;
                            break;
                        default:
                            break;
                    }
                    if (CarHandler.HasColorForTurning == false)
                    {
                        using (Brush brush = new SolidBrush(Color.Orange))
                        {
                            g.FillRectangle(brush, rectRightCorner);
                        }
                        CarHandler.HasColorForTurning = true;
                    }
                    else
                    {
                        using (Brush brush = new SolidBrush(Color.Black))
                        {
                            g.FillRectangle(brush, rectRightCorner);
                        }
                        CarHandler.HasColorForTurning = false;
                    }
                }

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

                int x = (int)(trafficLight.PositionX * scaleFactor) - 12;
                int y = (int)(trafficLight.PositionY * scaleFactor) - 12;

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

        // Paint Method for lanes
        public void Form1_PaintLanes(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int lineLength = 50;

            foreach (var trafficlight in Main.CrossroadHandler.TrafficLights)
            {
                Pen pen = new Pen(Color.Black, 1);

                switch (trafficlight.ID)
                {
                    case 1:
                        pen.Color = Color.Black;
                        int x1 = trafficlight.PositionX - 10;
                        int y1 = trafficlight.PositionY;


                        int x2 = trafficlight.PositionX - 10;
                        int y2 = trafficlight.PositionY + lineLength;
                        g.DrawLine(pen, x1 * scaleFactor, y1 * scaleFactor, x2 * scaleFactor, y2 * scaleFactor);
                        break;
                    case 2:
                        pen.Color = Color.Black;

                        int x12 = trafficlight.PositionX;
                        int y12 = trafficlight.PositionY + 10;


                        int x22 = trafficlight.PositionX + lineLength;
                        int y22 = trafficlight.PositionY + 10;
                        g.DrawLine(pen, x12 * scaleFactor, y12 * scaleFactor, x22 * scaleFactor, y22 * scaleFactor);
                        break;
                    case 3:
                        pen.Color = Color.Black;

                        int x13 = trafficlight.PositionX + 10;
                        int y13 = trafficlight.PositionY;


                        int x23 = trafficlight.PositionX + 10;
                        int y23 = trafficlight.PositionY - lineLength;
                        g.DrawLine(pen, x13 * scaleFactor, y13 * scaleFactor, x23 * scaleFactor, y23 * scaleFactor);
                        break;
                    case 4:
                        pen.Color = Color.Black;

                        int x14 = trafficlight.PositionX;
                        int y14 = trafficlight.PositionY - 10;

                        int x24 = trafficlight.PositionX - lineLength;
                        int y24 = trafficlight.PositionY - 10;
                        g.DrawLine(pen, x14 * scaleFactor, y14 * scaleFactor, x24 * scaleFactor, y24 * scaleFactor);
                        pen.Color = Color.Purple;
                        break;
                    default:
                        pen.Color = Color.Gray;
                        break;
                }

            }


            // Mode Button Click Events
            //private void button1_Click(object sender, EventArgs e)
            //{
            //    Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeOne);
            //    button1.Enabled = false;
            //    button3.Enabled = true;
            //    button4.Enabled = true;
            //    button5.Enabled = true;
            //}

            //private void button3_Click(object sender, EventArgs e)
            //{
            //    Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeTwo);
            //    button1.Enabled = true;
            //    button3.Enabled = false;
            //    button4.Enabled = true;
            //    button5.Enabled = true;
            //}

            //private void button4_Click(object sender, EventArgs e)
            //{
            //    Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeThree);
            //    button1.Enabled = true;
            //    button3.Enabled = true;
            //    button4.Enabled = false;
            //    button5.Enabled = true;
            //}

            //private void button5_Click(object sender, EventArgs e)
            //{
            //    Main.CrossroadHandler.LightHandler.SyncTrafficLights(TrafficLightMode.ModeFour);
            //    button1.Enabled = true;
            //    button3.Enabled = true;
            //    button4.Enabled = true;
            //    button5.Enabled = false;
            //}


        }
    }
}
