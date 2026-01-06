using AmpelSimulation.Classes.Services;
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
            this.Paint += Form1_Paint;
            Main.E_PlaceNewCar += (s, e) => this.Invalidate();
            Main.CrossroadHandler.E_MoveCar += (s, e) => this.Invalidate();
        }

        public void Form1_Paint(object sender, PaintEventArgs e) 
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
    }
}
