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
        }
    }
}
