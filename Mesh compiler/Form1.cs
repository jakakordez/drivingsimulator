using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesh_compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string outputFolder = "";
        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
		        {
                    string[] parts = openFileDialog1.FileNames[i].Replace('\\', '/').Replace("//", "/").Replace("//", "/").Split('/');
                    ListViewItem a = new ListViewItem(parts[parts.Length-1]);
                    a.Tag = openFileDialog1.FileNames[i];
                    listView1.Items.Add(a);
		        }
                if (outputFolder != "") btnConvert.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputFolder = folderBrowserDialog1.SelectedPath;
                lblOutputFolder.Text = "Output folder: "+folderBrowserDialog1.SelectedPath;
                if (listView1.Items.Count > 0) btnConvert.Enabled = true;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (btnConvert.Text == "Convert")
            {
                btnConvert.Text = "Cancel";
                string[] files = new string[listView1.Items.Count + 1];
                files[0] = outputFolder;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    files[i + 1] = listView1.Items[i].Tag as string;
                }
                backgroundWorker1.RunWorkerAsync(files);
            }
            else
            {
                btnConvert.Text = "Convert";
                backgroundWorker1.CancelAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] files = e.Argument as string[];
            string outputFolder = files[0];
            for (int i = 1; i < files.Length; i++)
            {
                int percent = (int)((float)i / (float)files.Length * 100);
                backgroundWorker1.ReportProgress(percent, new object[]{files[i], 1});
                Slovenia_simulator.Mesh m = new Slovenia_simulator.Mesh();
                object[] data = m.loadObjFile(files[i], false);
                backgroundWorker1.ReportProgress(percent, new object[] { files[i], 2 });
                string[] parts = files[i].Replace('\\', '/').Replace("//", "/").Replace("//", "/").Split('/');
                using (System.IO.StreamWriter w = new System.IO.StreamWriter(new System.IO.FileStream(outputFolder+"/"+parts[parts.Length-1].Split('.')[0]+".mesh", System.IO.FileMode.Create)))
                {
                    OpenTK.Vector3[] Vertices = data[0] as OpenTK.Vector3[];
                    for (int j = 0; j < Vertices.Length; j++)
                    {
                        w.Write(Vertices[j].X.ToString(System.Globalization.CultureInfo.InvariantCulture)+" ");
                        w.Write(Vertices[j].Y.ToString(System.Globalization.CultureInfo.InvariantCulture)+" ");
                        w.Write(Vertices[j].Z.ToString(System.Globalization.CultureInfo.InvariantCulture)+" ");
                    }
                    w.Write(";");
                    OpenTK.Vector2[] TextureCoordinates = data[1] as OpenTK.Vector2[];
                    for (int j = 0; j < TextureCoordinates.Length; j++)
                    {
                        w.Write(TextureCoordinates[j].X.ToString(System.Globalization.CultureInfo.InvariantCulture)+" ");
                        w.Write(TextureCoordinates[j].Y.ToString(System.Globalization.CultureInfo.InvariantCulture)+" ");
                    }
                    w.Write(';');
                    List<int[]> Indicies = data[2] as List<int[]>;
                    for (int j = 0; j < Indicies.Count; j++)
                    {
                        for (int k = 0; k < Indicies[j].Length; k++)
                        {
                            w.Write(Indicies[j][k].ToString(System.Globalization.CultureInfo.InvariantCulture)+" ");
                        }
                        w.Write(';');
                    }
                    w.Close();
                }
                backgroundWorker1.ReportProgress(percent, new object[] { files[i], 3 });
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
            lblCurrent.Text = "Current file: " + (e.UserState as object[])[0] as string;
            progressBar1.Value = (int)(e.UserState as object[])[1];
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("All files are compiled to specified folder.", "Done");
        }
    }
}
