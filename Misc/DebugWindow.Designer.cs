namespace Stab_Face.Misc
{
    partial class DebugWindow
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
            this.MainDebugTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // MainDebugTextBox
            // 
            this.MainDebugTextBox.Location = new System.Drawing.Point(19, 21);
            this.MainDebugTextBox.Name = "MainDebugTextBox";
            this.MainDebugTextBox.Size = new System.Drawing.Size(460, 379);
            this.MainDebugTextBox.TabIndex = 0;
            this.MainDebugTextBox.Text = "";
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 417);
            this.Controls.Add(this.MainDebugTextBox);
            this.Name = "DebugWindow";
            this.Text = "DebugWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox MainDebugTextBox;
    }
}