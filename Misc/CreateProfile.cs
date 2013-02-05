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
using System.Threading;

namespace Stab_Face.Misc
{
    public partial class CreateProfile : Form
    {
        Profile profile;
        Boolean recording;
        private static Thread recordThread;

        public CreateProfile(Form calling)
        {
            InitializeComponent();
        }

        public Profile getProfile()
        {
            return this.profile;
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                disableChanges();
                recordThread = new Thread(new ThreadStart(record));
                recordThread.Start();
            }
        }

        private void record()
        {

            profile = new Profile();
            profile.setName(this.name.Text);
            Player p = new Player(0);

            recording = true;
            while (recording)
            {
                if (this.normalCheckBox.Checked)
                {
                    Thread.Sleep(100);
                    Mob m = (Mob)p.checkForTarget(this);
                    if (m != null)
                    {
                        UInt16 fac = m.getFaction();
                        if (profile.getFactions().Contains(fac) == false)
                        {
                            profile.addFaction(fac);
                        }
                    }

                    Waypoint pLoc = p.getLocation();
                    if (profile.getWaypoints().Count == 0)
                    {
                        profile.AddWaypoint(pLoc);
                    }
                    else if (profile.getWaypoints().Last<Waypoint>().getDistance(pLoc) > 10.0f)
                    {
                        profile.AddWaypoint(pLoc);
                    }
                }
                else if (this.ghostCheckBox.Checked)
                {
                    Waypoint pLoc = p.getLocation();
                    if (profile.getGhostWaypoints().Count == 0)
                    {
                        profile.AddGhostWaypoint(pLoc);
                    }
                    else if (profile.getGhostWaypoints().Last<Waypoint>().getDistance(pLoc) > 10.0f)
                    {
                        profile.AddGhostWaypoint(pLoc);
                    }
                }
                else
                {

                }
            }
        }

        private Boolean checkInput()
        {
            if (this.name.Text.Length < 1)
            {
                MessageBox.Show("Please Enter A Valid Profile Name");
                return false;
            }

            return true;
        }

        private void disableChanges()
        {
            this.name.Enabled = false;
            this.start.Enabled = false;
        }

        private void stop_Click(object sender, EventArgs e)
        {
            recording = false;
            recordThread.Abort();
            profile.Save();
            this.Close();
        }

        private void normalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.normalCheckBox.Checked)
            {
                this.ghostCheckBox.Checked = false;
            }
        }

        private void ghostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ghostCheckBox.Checked)
            {
                this.normalCheckBox.Checked = false;
            }
        }
    }
}
