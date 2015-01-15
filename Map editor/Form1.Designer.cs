namespace Map_editor
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Roads", 1, 1);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Objects", 2, 2);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Utilities", 3, 3);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPointer = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Map = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.Map);
            this.panel1.Location = new System.Drawing.Point(339, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 488);
            this.panel1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(12, 87);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 1;
            treeNode1.Name = "Node0";
            treeNode1.SelectedImageIndex = 1;
            treeNode1.Text = "Roads";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "Node1";
            treeNode2.SelectedImageIndex = 2;
            treeNode2.Text = "Objects";
            treeNode3.ImageIndex = 3;
            treeNode3.Name = "Node2";
            treeNode3.SelectedImageIndex = 3;
            treeNode3.Text = "Utilities";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(155, 413);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "arrow-19-256.png");
            this.imageList1.Images.SetKeyName(1, "road.png");
            this.imageList1.Images.SetKeyName(2, "box.png");
            this.imageList1.Images.SetKeyName(3, "fuel-256.png");
            this.imageList1.Images.SetKeyName(4, "marker-128.png");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 42);
            this.button1.TabIndex = 2;
            this.button1.Text = "New";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.cmbType);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(176, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 69);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Place";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "Road",
            "Object",
            "Utility"});
            this.cmbType.Location = new System.Drawing.Point(76, 21);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(73, 21);
            this.cmbType.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(155, 69);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(102, 21);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(42, 42);
            this.button5.TabIndex = 5;
            this.button5.Text = "Save";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(54, 21);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(42, 42);
            this.button4.TabIndex = 4;
            this.button4.Text = "Open";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(42, 42);
            this.button3.TabIndex = 3;
            this.button3.Text = "New";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnPointer);
            this.groupBox4.Controls.Add(this.btnDefault);
            this.groupBox4.Location = new System.Drawing.Point(176, 87);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(157, 72);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tool";
            // 
            // btnPointer
            // 
            this.btnPointer.Location = new System.Drawing.Point(76, 19);
            this.btnPointer.Name = "btnPointer";
            this.btnPointer.Size = new System.Drawing.Size(64, 42);
            this.btnPointer.TabIndex = 8;
            this.btnPointer.Text = "Pointer";
            this.btnPointer.UseVisualStyleBackColor = true;
            this.btnPointer.Click += new System.EventHandler(this.btnPointer_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.BackColor = System.Drawing.Color.Gray;
            this.btnDefault.Location = new System.Drawing.Point(6, 19);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(64, 42);
            this.btnDefault.TabIndex = 7;
            this.btnDefault.Text = "None";
            this.btnDefault.UseVisualStyleBackColor = false;
            this.btnDefault.Click += new System.EventHandler(this.button6_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(176, 165);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(157, 335);
            this.propertyGrid1.TabIndex = 7;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "C:\\Users\\Jaka\\Documents\\Visual Studio 2013\\Projects\\Driving simulator\\Driving sim" +
    "ulator\\bin\\Debug\\data\\maps";
            // 
            // Map
            // 
            this.Map.BackColor = System.Drawing.Color.Green;
            this.Map.BackgroundImage = global::Map_editor.Properties.Resources.Untitled2;
            this.Map.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Map.Location = new System.Drawing.Point(3, 3);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(5000, 5000);
            this.Map.TabIndex = 2;
            this.Map.Click += new System.EventHandler(this.Map_Click);
            this.Map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(76, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 512);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Map editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel Map;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnPointer;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button2;
    }
}

