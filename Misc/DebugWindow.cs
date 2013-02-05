using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stab_Face.Misc
{
    public partial class DebugWindow : Form
    {
        private static DebugWindow instance = null;

        private DebugWindow()
        {
            InitializeComponent();
        }

        public static DebugWindow getDebugWindow()
        {
            if (instance == null)
            {
                instance = new DebugWindow();
            }
            return instance;
        }

        public RichTextBox getMainDebugOutputTextBox()
        {
            return instance.MainDebugTextBox;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
