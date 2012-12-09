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
using Stab_Face.Misc;
using System.IO;

namespace Stab_Face
{
    public partial class MainForm : Form
    {
        Player player;
        Profile profile;
        
        public MainForm()
        {
            InitializeComponent();
            this.stop.Enabled = false;
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (this.profile == null)
            {
                MessageBox.Show("Please Load A Profile First.");
                return;
            }
            if (player == null)
            {
                player = new Player(0);
                player.start(profile);
                this.start.Enabled = false;
                this.stop.Enabled = true;
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.stop();
                player = null;
                this.start.Enabled = true;
                this.stop.Enabled = false;
            }
        }

        /// <summary>
        /// Create a new Profile via NewProfile Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProfile_Click(object sender, EventArgs e)
        {
            CreateProfile cp = new CreateProfile(this);
            cp.ShowDialog();
            cp.FormClosed += new FormClosedEventHandler(On_NPFC);
        }

        protected virtual void On_NPFC(Object o, EventArgs e)
        {
            this.profile = ((CreateProfile)o).getProfile();
            o = null;
        }

        /// <summary>
        /// Load a Saved Profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadProfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xml files (*.xml)|*.xml";
            dialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\Profiles"; 
            dialog.Title = "Select a Profile";

            if (dialog.ShowDialog() == DialogResult.OK)
                 profile = Profile.Load(dialog.FileName);
        }
    }
}
