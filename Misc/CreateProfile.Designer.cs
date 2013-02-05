namespace Stab_Face.Misc
{
    partial class CreateProfile
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.ghostCheckBox = new System.Windows.Forms.CheckBox();
            this.normalCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(254, 46);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "Click start and run your path attacking the types of  mobs you want the profile t" +
    "o attack. Once you\'re finished click finish.";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(124, 75);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(142, 20);
            this.name.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(15, 130);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(76, 20);
            this.start.TabIndex = 3;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(190, 130);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(76, 20);
            this.stop.TabIndex = 4;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // ghostCheckBox
            // 
            this.ghostCheckBox.AutoSize = true;
            this.ghostCheckBox.Location = new System.Drawing.Point(15, 156);
            this.ghostCheckBox.Name = "ghostCheckBox";
            this.ghostCheckBox.Size = new System.Drawing.Size(54, 17);
            this.ghostCheckBox.TabIndex = 5;
            this.ghostCheckBox.Text = "Ghost";
            this.ghostCheckBox.UseVisualStyleBackColor = true;
            this.ghostCheckBox.CheckedChanged += new System.EventHandler(this.ghostCheckBox_CheckedChanged);
            // 
            // normalCheckBox
            // 
            this.normalCheckBox.AutoSize = true;
            this.normalCheckBox.Location = new System.Drawing.Point(15, 179);
            this.normalCheckBox.Name = "normalCheckBox";
            this.normalCheckBox.Size = new System.Drawing.Size(59, 17);
            this.normalCheckBox.TabIndex = 6;
            this.normalCheckBox.Text = "Normal";
            this.normalCheckBox.UseVisualStyleBackColor = true;
            this.normalCheckBox.CheckedChanged += new System.EventHandler(this.normalCheckBox_CheckedChanged);
            // 
            // CreateProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 255);
            this.Controls.Add(this.normalCheckBox);
            this.Controls.Add(this.ghostCheckBox);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.name);
            this.Controls.Add(this.richTextBox1);
            this.Name = "CreateProfile";
            this.Text = "CreateProfile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.CheckBox ghostCheckBox;
        private System.Windows.Forms.CheckBox normalCheckBox;
    }
}