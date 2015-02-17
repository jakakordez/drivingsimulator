using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Terrain_compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Terrain terrain;
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblFile.Text = "File: " + openFileDialog1.FileName;
                if (new Import_settings().ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    terrain = new Terrain(openFileDialog1.FileName);
                    pictureBox1.Image = terrain.GenerateBitmap();
                    lblHeightRange.Text = "Height range: " + terrain.min + " - " + terrain.max;
                }
            }
        }

        private void flipHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap(pictureBox1.Image);
            b.RotateFlip(RotateFlipType.RotateNoneFlipX);
            pictureBox1.Image = b;
        }

        private void flipVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap(pictureBox1.Image);
            b.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox1.Image = b;
        }

        private void flipHFlipVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap(pictureBox1.Image);
            b.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = b;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                terrain.Export(saveFileDialog1.FileName);
                MessageBox.Show("Exported to file: " + saveFileDialog1.FileName);
            }
        }
    }
}
