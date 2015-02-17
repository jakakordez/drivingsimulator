namespace Terrain_compiler
{
    partial class Import_settings
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
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDelimiter = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nmrLength = new System.Windows.Forms.NumericUpDown();
            this.nmrWidth = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nmrLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbFormat
            // 
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Items.AddRange(new object[] {
            "ASCII",
            "Binary"});
            this.cmbFormat.Location = new System.Drawing.Point(104, 24);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(108, 21);
            this.cmbFormat.TabIndex = 0;
            this.cmbFormat.Text = "ASCII";
            this.cmbFormat.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Format:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Delimiter:";
            // 
            // txtDelimiter
            // 
            this.txtDelimiter.Location = new System.Drawing.Point(104, 51);
            this.txtDelimiter.Name = "txtDelimiter";
            this.txtDelimiter.Size = new System.Drawing.Size(108, 20);
            this.txtDelimiter.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(128, 140);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 35);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 140);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 35);
            this.button2.TabIndex = 5;
            this.button2.Text = "Import";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Length:";
            // 
            // nmrLength
            // 
            this.nmrLength.Location = new System.Drawing.Point(104, 103);
            this.nmrLength.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nmrLength.Name = "nmrLength";
            this.nmrLength.Size = new System.Drawing.Size(108, 20);
            this.nmrLength.TabIndex = 8;
            this.nmrLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmrWidth
            // 
            this.nmrWidth.Location = new System.Drawing.Point(104, 77);
            this.nmrWidth.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nmrWidth.Name = "nmrWidth";
            this.nmrWidth.Size = new System.Drawing.Size(108, 20);
            this.nmrWidth.TabIndex = 9;
            this.nmrWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Import_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 183);
            this.Controls.Add(this.nmrWidth);
            this.Controls.Add(this.nmrLength);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDelimiter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbFormat);
            this.MaximizeBox = false;
            this.Name = "Import_settings";
            this.Text = "Import settings";
            this.Load += new System.EventHandler(this.Import_settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmrLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox cmbFormat;
        public System.Windows.Forms.TextBox txtDelimiter;
        public System.Windows.Forms.NumericUpDown nmrLength;
        public System.Windows.Forms.NumericUpDown nmrWidth;
    }
}