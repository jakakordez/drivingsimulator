namespace Terrain_compiler
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.flipHFlipVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblHeightRange = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Location = new System.Drawing.Point(7, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(525, 301);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flipHFlipVToolStripMenuItem,
            this.flipHToolStripMenuItem,
            this.flipVToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 70);
            // 
            // flipHFlipVToolStripMenuItem
            // 
            this.flipHFlipVToolStripMenuItem.Name = "flipHFlipVToolStripMenuItem";
            this.flipHFlipVToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.flipHFlipVToolStripMenuItem.Text = "Rotate";
            this.flipHFlipVToolStripMenuItem.Click += new System.EventHandler(this.flipHFlipVToolStripMenuItem_Click);
            // 
            // flipHToolStripMenuItem
            // 
            this.flipHToolStripMenuItem.Name = "flipHToolStripMenuItem";
            this.flipHToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.flipHToolStripMenuItem.Text = "Flip H";
            this.flipHToolStripMenuItem.Click += new System.EventHandler(this.flipHToolStripMenuItem_Click);
            // 
            // flipVToolStripMenuItem
            // 
            this.flipVToolStripMenuItem.Name = "flipVToolStripMenuItem";
            this.flipVToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.flipVToolStripMenuItem.Text = "Flip V";
            this.flipVToolStripMenuItem.Click += new System.EventHandler(this.flipVToolStripMenuItem_Click);
            // 
            // lblHeightRange
            // 
            this.lblHeightRange.AutoSize = true;
            this.lblHeightRange.Location = new System.Drawing.Point(287, 14);
            this.lblHeightRange.Name = "lblHeightRange";
            this.lblHeightRange.Size = new System.Drawing.Size(71, 13);
            this.lblHeightRange.TabIndex = 5;
            this.lblHeightRange.Text = "Height range:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFile});
            this.statusStrip1.Location = new System.Drawing.Point(0, 342);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(544, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblFile
            // 
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(28, 17);
            this.lblFile.Text = "File:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(80, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Export";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Raw file (*.raw)|*.raw";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 364);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblHeightRange);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Terrain compiler";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblHeightRange;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem flipHFlipVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipVToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

