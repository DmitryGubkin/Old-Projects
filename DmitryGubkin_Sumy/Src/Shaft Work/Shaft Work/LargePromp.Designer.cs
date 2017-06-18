namespace Shaft_Work
{
    partial class LargePromp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LargePromp));
            this.Large_img = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Large_img)).BeginInit();
            this.SuspendLayout();
            // 
            // Large_img
            // 
            this.Large_img.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Large_img.BackColor = System.Drawing.Color.Transparent;
            this.Large_img.Location = new System.Drawing.Point(13, 13);
            this.Large_img.Name = "Large_img";
            this.Large_img.Size = new System.Drawing.Size(487, 487);
            this.Large_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Large_img.TabIndex = 0;
            this.Large_img.TabStop = false;
            // 
            // LargePromp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.Large_img);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LargePromp";
            this.Text = "Shaft Works";
            ((System.ComponentModel.ISupportInitialize)(this.Large_img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Large_img;
    }
}