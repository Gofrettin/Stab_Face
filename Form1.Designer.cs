namespace Stab_Face
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.start = new System.Windows.Forms.Button();
            this.MainPane = new System.Windows.Forms.TabControl();
            this.RunTab = new System.Windows.Forms.TabPage();
            this.stop = new System.Windows.Forms.Button();
            this.ProfileTab = new System.Windows.Forms.TabPage();
            this.NewProfile = new System.Windows.Forms.Button();
            this.LoadProfile = new System.Windows.Forms.Button();
            this.MainPane.SuspendLayout();
            this.RunTab.SuspendLayout();
            this.ProfileTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(26, 20);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(82, 29);
            this.start.TabIndex = 0;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // MainPane
            // 
            this.MainPane.Controls.Add(this.RunTab);
            this.MainPane.Controls.Add(this.ProfileTab);
            this.MainPane.Location = new System.Drawing.Point(12, 2);
            this.MainPane.Name = "MainPane";
            this.MainPane.SelectedIndex = 0;
            this.MainPane.Size = new System.Drawing.Size(268, 258);
            this.MainPane.TabIndex = 2;
            // 
            // RunTab
            // 
            this.RunTab.Controls.Add(this.stop);
            this.RunTab.Controls.Add(this.start);
            this.RunTab.Location = new System.Drawing.Point(4, 22);
            this.RunTab.Name = "RunTab";
            this.RunTab.Padding = new System.Windows.Forms.Padding(3);
            this.RunTab.Size = new System.Drawing.Size(260, 232);
            this.RunTab.TabIndex = 0;
            this.RunTab.Text = "Run";
            this.RunTab.UseVisualStyleBackColor = true;
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(141, 20);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(82, 29);
            this.stop.TabIndex = 1;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // ProfileTab
            // 
            this.ProfileTab.Controls.Add(this.NewProfile);
            this.ProfileTab.Controls.Add(this.LoadProfile);
            this.ProfileTab.Location = new System.Drawing.Point(4, 22);
            this.ProfileTab.Name = "ProfileTab";
            this.ProfileTab.Padding = new System.Windows.Forms.Padding(3);
            this.ProfileTab.Size = new System.Drawing.Size(260, 232);
            this.ProfileTab.TabIndex = 1;
            this.ProfileTab.Text = "Profile";
            this.ProfileTab.UseVisualStyleBackColor = true;
            // 
            // NewProfile
            // 
            this.NewProfile.Location = new System.Drawing.Point(6, 73);
            this.NewProfile.Name = "NewProfile";
            this.NewProfile.Size = new System.Drawing.Size(68, 26);
            this.NewProfile.TabIndex = 3;
            this.NewProfile.Text = "New";
            this.NewProfile.UseVisualStyleBackColor = true;
            this.NewProfile.Click += new System.EventHandler(this.NewProfile_Click);
            // 
            // LoadProfile
            // 
            this.LoadProfile.Location = new System.Drawing.Point(6, 6);
            this.LoadProfile.Name = "LoadProfile";
            this.LoadProfile.Size = new System.Drawing.Size(68, 26);
            this.LoadProfile.TabIndex = 0;
            this.LoadProfile.Text = "Load";
            this.LoadProfile.UseVisualStyleBackColor = true;
            this.LoadProfile.Click += new System.EventHandler(this.LoadProfile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 272);
            this.Controls.Add(this.MainPane);
            this.Name = "MainForm";
            this.Text = "Stab Face";
            this.MainPane.ResumeLayout(false);
            this.RunTab.ResumeLayout(false);
            this.ProfileTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.TabControl MainPane;
        private System.Windows.Forms.TabPage RunTab;
        private System.Windows.Forms.TabPage ProfileTab;
        private System.Windows.Forms.Button NewProfile;
        private System.Windows.Forms.Button LoadProfile;
        private System.Windows.Forms.Button stop;
    }
}

