using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Map_editor
{
    public partial class Form1 : Form
    {
        bool PlaceMode;
        public delegate void ReferencePointAdded(ReferencePoint p);
        ReferencePointAdded newPointOnMap;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Visual Studio 2013\\Projects\\Slovenia simulator\\Slovenia simulator\\bin\\Debug\\data\\maps";
            cmbType.SelectedIndex = 0;
            newPointOnMap = new ReferencePointAdded(PointAdded);
        }
        string[] Types = new string[] { "Road", "Object", "Utility" };
        private void button1_Click(object sender, EventArgs e)
        {
            var a = treeView1.Nodes[cmbType.SelectedIndex].Nodes.Add("", "New " + Types[cmbType.SelectedIndex], cmbType.SelectedIndex+1);
            switch (cmbType.SelectedIndex)
            {
                case 0:
                    a.Tag = new Road(a);
                    break;
                case 1:
                    a.Tag = new Object(a);
                    break;
                case 2:
                    a.Tag = new Utility(a);
                    break;
            }
            treeView1.SelectedNode = a;
            TypeObj = a;
            StartDrawing();
        }
        TreeNode TypeObj, SelectedObj;
        public void PointAdded(ReferencePoint r)
        {
            
                TreeNode node = new TreeNode("Point");
                node.Tag = r;
                node.ImageIndex = 4;
                var b = TypeObj.Nodes.Add(node);
                TypeObj.Expand();
            
        }

        private void Map_Click(object sender, EventArgs e)
        {
            Point p = new Point((e as MouseEventArgs).X, (e as MouseEventArgs).Y);
            if (PlaceMode && TypeObj != null)
            {
                if (TypeObj.Parent.Text == "Roads" || TypeObj.Nodes.Count < 2)
                {
                    ReferencePoint r = new ReferencePoint(p);
                    r.MouseUp += r_MouseUp;
                    Map.Controls.Add(r);
                    newPointOnMap(r);
                }
            }
        }

        void r_MouseUp(object sender, MouseEventArgs e)
        {
            Draw();
            foreach (TreeNode a in treeView1.Nodes)
            {
                foreach (TreeNode b in a.Nodes)
                {
                    foreach (TreeNode c in b.Nodes)
                    {
                        if (c.Tag == sender)
                        {
                            treeView1.SelectedNode = c;
                            return;
                        }
                    }
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 2 || e.Node.Level == 1)
            {
                SelectedObj = e.Node;
                if (e.Node.Level == 2)
                {

                    (e.Node.Tag as ReferencePoint).Focus();
                    for (int i = 0; i < Map.Controls.Count; i++)
                    {
                        if (Map.Controls[i] != e.Node.Tag) (Map.Controls[i] as ReferencePoint).Unfocus();
                    }
                    TypeObj = e.Node.Parent;

                }
                else
                {
                    propertyGrid1.SelectedObject = e.Node.Tag;
                    TypeObj = e.Node;
                }
            }
        }

        public void Draw()
        {
            System.Drawing.Graphics g = Map.CreateGraphics();
            //g.Clear(Color.Green);
            g.DrawImage(Properties.Resources.Untitled2, new Point(0, 0));
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            myPen.Color = Color.Blue;
            myPen.Width = 10;
            foreach (TreeNode n in treeView1.Nodes[0].Nodes)
	        {
                PointF[] tocke = new PointF[n.Nodes.Count];
		        for (int i = 0; i < n.Nodes.Count; i++)
			    {
			        tocke[i] = (n.Nodes[i].Tag as ReferencePoint).Location;
			    }
                tocke = GetBezierApproximation(tocke, 10);//tocke.Length*
                for (int i = 0; i < tocke.Length; i++)
                {
                    g.DrawRectangle(myPen, new Rectangle((int)tocke[i].X, (int)tocke[i].Y, 1, 1));
                }
	        }
            myPen.Dispose();
            g.Dispose();
        }

        PointF[] GetBezierApproximation(PointF[] controlPoints, int outputSegmentCount)
        {
            if (controlPoints.Length == 0) return controlPoints;
            PointF[] points = new PointF[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
            }
            return points;
        }

        PointF GetBezierPoint(double t, PointF[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new PointF((float)((1 - t) * P0.X + t * P1.X), (float)((1 - t) * P0.Y + t * P1.Y));
        }

        private void Map_MouseUp(object sender, MouseEventArgs e)
        {
            //Draw();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            btnPointer.BackColor = Color.White;
            btnDefault.BackColor = Color.Gray;
            Map.Cursor = Cursors.Default;
            PlaceMode = false;
        }

        private void btnPointer_Click(object sender, EventArgs e)
        {
            StartDrawing();
        }

        private void StartDrawing()
        {
            btnPointer.BackColor = Color.Gray;
            btnDefault.BackColor = Color.White;
            Map.Cursor = Cursors.Cross;
            PlaceMode = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(SelectedObj != null){
                Map.Controls.Remove((SelectedObj.Tag as Control));
                SelectedObj.Remove();
                Draw();
            }
        }
        string MapPath = "";
        private void button4_Click(object sender, EventArgs e)
        {
            
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MapPath = folderBrowserDialog1.SelectedPath;
                foreach (string f in Directory.GetFiles(MapPath + "/roads/"))
	            {
                    TreeNode n = new TreeNode("", 1, 0);
                    Road r = new Road(n);
                    n.Tag = r;
                    if (f.Split('.')[1] == "json")
                    {
                        Newtonsoft.Json.JsonConvert.PopulateObject(File.ReadAllText(f), r);
                        for (int i = 0; i < r.Line.Length; i++)
                        {
                            TreeNode tr = new TreeNode("Point", 4, 0);
                            ReferencePoint point = new ReferencePoint(new Point((int)(r.Line[i].X * 5), (int)(r.Line[i].Y * 5)));
                            tr.Tag = point;
                            point.MouseUp += r_MouseUp;
                            Map.Controls.Add(point);
                            n.Nodes.Add(tr);
                        }
                    }
                    else { 
                        string[] file = File.ReadAllLines(f);
                        for (int i = 0; i < file.Length; i++)
                        {
                            if (file[i] != "" && file[i][0] != '#' && file[i].Contains('='))
                            {
                                string[] line = file[i].Replace(" ", "").Split('=');
                                switch (line[0])
                                {
                                    case "Name": r.Name = line[1]; break;
                                    case "RoadType": r.RoadType = (RoadTypes)toInt(line[1]); break;
                                    case "Segments": r.Segments = toInt(line[1]); break;
                                    case "Traffic": r.Traffic = toBool(line[1]); break;
                                    case "Limit": r.Limit = toInt(line[1]); break;
                                    case "LaneWidth": r.LaneWidth = toFloat(line[1]); break;
                                    case "SidewalkWidth": r.SidewalkWidth = toFloat(line[1]); break;
                                    case "LaneHeight": r.LaneHeight = toFloat(line[1]); break;
                                    case "SidewalkHeight": r.SidewalkHeight = toFloat(line[1]); break;
                                    case "SplitWidth": r.SplitWidth = toFloat(line[1]); break;
                                    case "LaneTexturePath": r.LaneTexturePath = line[1]; break;
                                    case "SidewalkTexturePath": r.SidewalkTexturePath = line[1]; break;
                                    case "Line":
                                        string[] points = line[1].Split(';');
                                        for (int j = 0; j < points.Length; j++)
                                        {
                                            if (points[j].Contains(':'))
                                            {
                                                TreeNode tr = new TreeNode("Point", 4, 0);
                                                string[] tocka = points[j].Split(':');
                                                ReferencePoint point = new ReferencePoint(new Point((int)(toFloat(tocka[0]) * 5), (int)(toFloat(tocka[1]) * 5)));
                                                tr.Tag = point;
                                                point.MouseUp += r_MouseUp;
                                                Map.Controls.Add(point);
                                                n.Nodes.Add(tr);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    treeView1.Nodes[0].Nodes.Add(n);
	            }
                Draw();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MapPath = "";
            treeView1.Nodes[0].Nodes.Clear();
            treeView1.Nodes[1].Nodes.Clear();
            treeView1.Nodes[2].Nodes.Clear();
            Map.Controls.Clear();
            Draw();
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                if (treeView1.SelectedNode.Level == 2)
                {
                    Map.Controls.Remove(treeView1.SelectedNode.Tag as ReferencePoint);
                    treeView1.SelectedNode.Remove();
                    
                }
                else if (treeView1.SelectedNode.Level == 1)
                {
                    foreach (TreeNode t in treeView1.SelectedNode.Nodes)
                    {
                        Map.Controls.Remove(t.Tag as ReferencePoint);
                    }
                    treeView1.SelectedNode.Remove();
                }
                Draw();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MapPath != ""  || folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(MapPath == "")MapPath = folderBrowserDialog1.SelectedPath;
                List<string> roads = new List<string>();
                foreach (TreeNode t in treeView1.Nodes[0].Nodes)
                {
                    (t.Tag as Road).ExportToFile(MapPath);
                }
            }
        }

        public static float toFloat(string data)
        {
            float result = 0;
            float.TryParse(data, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static string toString(float data)
        {
            return data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int toInt(string data)
        {
            int result = 0;
            int.TryParse(data, out result);
            return result;
        }

        public static bool toBool(string data)
        {
            return Convert.ToBoolean(data);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
