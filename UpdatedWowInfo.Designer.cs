namespace Stab_Face
{
    partial class UpdatedWowInfo
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
            this.components = new System.ComponentModel.Container();
            this.buttonStartUpdateWowInfo = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.labelZ = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonStartUpdateWowInfo
            // 
            this.buttonStartUpdateWowInfo.Location = new System.Drawing.Point(15, 168);
            this.buttonStartUpdateWowInfo.Name = "buttonStartUpdateWowInfo";
            this.buttonStartUpdateWowInfo.Size = new System.Drawing.Size(75, 23);
            this.buttonStartUpdateWowInfo.TabIndex = 0;
            this.buttonStartUpdateWowInfo.Text = "Start";
            this.buttonStartUpdateWowInfo.UseVisualStyleBackColor = true;
            this.buttonStartUpdateWowInfo.Click += new System.EventHandler(this.buttonStartUpdateWowInfo_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(96, 168);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(133, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop Update Wow Info";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(52, 54);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(100, 20);
            this.textBoxX.TabIndex = 2;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(52, 92);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(100, 20);
            this.textBoxY.TabIndex = 3;
            // 
            // textBoxZ
            // 
            this.textBoxZ.Location = new System.Drawing.Point(52, 129);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxZ.TabIndex = 4;
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(12, 135);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(23, 13);
            this.labelZ.TabIndex = 5;
            this.labelZ.Text = "Z:=";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(12, 92);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(23, 13);
            this.labelY.TabIndex = 6;
            this.labelY.Text = "Y:=";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(12, 54);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(23, 13);
            this.labelX.TabIndex = 7;
            this.labelX.Text = "X:=";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(247, 10);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(470, 212);
            this.listBox1.TabIndex = 8;
            // 
            // UpdatedWowInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 243);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelZ);
            this.Controls.Add(this.textBoxZ);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStartUpdateWowInfo);
            this.Name = "UpdatedWowInfo";
            this.Text = "UpdatedWowInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartUpdateWowInfo;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxZ;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.ListBox listBox1;
    }
}