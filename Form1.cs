using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stab_Face.Units;
using System.Diagnostics;

namespace Stab_Face
{
    public partial class Form1 : Form
    {
        Player p;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (p == null)
            {
                p = new Player(0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (p != null)
            {
                Debug.WriteLine(p.ToString());
            }
        }
    }
}
