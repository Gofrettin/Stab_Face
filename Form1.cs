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
        DebugWindow dbg;
        
        public MainForm()
        {
            InitializeComponent();
            this.Stop.Enabled = false;

            // Setup the debug window
            dbg = DebugWindow.getDebugWindow();
            TextBoxTraceListener traceWindow = TextBoxTraceListener.getTraceListener(dbg.getMainDebugOutputTextBox());
            Trace.Listeners.Add(traceWindow);
            DebugWindow.getDebugWindow().Show();

            Debug.Print("Test");
        }

        protected virtual void On_NPFC(Object o, EventArgs e)
        {
            //this.profile = ((CreateProfile)o).getProfile();
            this.profile = Profile.Load(AppDomain.CurrentDomain.BaseDirectory + "\\Profiles\\" + ((CreateProfile)o).getProfile().getName() + ".xml");
            o = null;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
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
                this.Start.Enabled = false;
                this.Stop.Enabled = true;
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.stop();
                player = null;
                this.Start.Enabled = true;
                this.Stop.Enabled = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopToolStripMenuItem_Click(sender, e);
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProfile cp = new CreateProfile(this);
            cp.ShowDialog();
            cp.FormClosed += new FormClosedEventHandler(On_NPFC);
        }

        private void loadProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xml files (*.xml)|*.xml";
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Profiles";
            dialog.Title = "Select a Profile";

            if (dialog.ShowDialog() == DialogResult.OK)
                profile = Profile.Load(dialog.FileName);
        }

        private void loadGliderProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xml files (*.xml)|*.xml";
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Profiles";
            dialog.Title = "Select a Profile";

            if (dialog.ShowDialog() == DialogResult.OK)
                profile = Profile.LoadGliderProfile(dialog.FileName);
        }

        private void debugWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugWindow.getDebugWindow().Show();
        }
    }
}
