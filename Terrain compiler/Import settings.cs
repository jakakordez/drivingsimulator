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
    public partial class Import_settings : Form
    {
        public Import_settings()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Visible = (cmbFormat.Text == "ASCII");
            txtDelimiter.Visible = (cmbFormat.Text == "ASCII");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Format = cmbFormat.Text;
             Properties.Settings.Default.Delimiter = txtDelimiter.Text;
            Properties.Settings.Default.Length = (int)nmrLength.Value;
            Properties.Settings.Default.Width = (int)nmrWidth.Value;
            Properties.Settings.Default.Save();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void Import_settings_Load(object sender, EventArgs e)
        {
            cmbFormat.Text = Properties.Settings.Default.Format;
            txtDelimiter.Text = Properties.Settings.Default.Delimiter;
            nmrLength.Value = Properties.Settings.Default.Length;
            nmrWidth.Value = Properties.Settings.Default.Width;
        }
    }
}
