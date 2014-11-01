using Stab_Face.Misc;
using Stab_Face.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stab_Face
{
    public partial class UpdatedWowInfo : Form
    {
        Player player;
        public UpdatedWowInfo()
        {
            InitializeComponent();
            player = new Player(0, true);
        }

        private void buttonStartUpdateWowInfo_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Waypoint loc = player.getLocation();
            textBoxX.Text = loc.getX().ToString();
            textBoxY.Text = loc.getY().ToString();
            textBoxZ.Text = loc.getZ().ToString();
            player.getObjects();
            List<Unit> units = player.getNearbyUnits();
            foreach (Unit unit in units)
            {

                listBox1.Items.Add(((Mob)unit).getLocation().getDistance(loc).ToString() +" "+ ((Mob)unit).getFaction().ToString("X"));

            }
        }
    }
}
