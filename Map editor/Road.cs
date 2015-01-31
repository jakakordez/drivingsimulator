using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Map_editor
{
    public enum RoadTypes
    {
        one_lane                              =1 ,
        two_lanes                             =2 ,
        two_lanes_one_way                     =3 ,
        two_lanes_one_way_with_sidewalk       =4 ,
        two_lanes_with_sidewalk               =5 ,
        two_lanes_with_sidewalk_on_one_side   =6 ,
        four_lanes                            =7 ,
        four_lanes_with_sidewalk              =8 ,
        twoX_two_lanes                        =9 ,
        twoX_two_lanes_with_emergency_lanes   =10,
        twoX_three_lanes                      =11,
        twoX_three_lanes_with_emergency_lanes =12,
        twoX_four_lanes                       =13 
    }
    public class Road:Type
    {
        int limit = 50, segments = 10;
        RoadTypes type = RoadTypes.two_lanes_with_sidewalk;
        bool traffic = false;
        string leftObject, rightObject, laneTexturePath, sideWalkTexturePath;
        float laneWidth = 3, sidewalkWidth = 1, laneHeight = 0.02f, sidewalkHeight = 0.1f, splitWidth = 1;

        public float LaneWidth { get { return laneWidth; } set { laneWidth = value; } }
        public float SidewalkWidth { get { return sidewalkWidth; } set { sidewalkWidth = value; } }
        public float LaneHeight { get { return laneHeight; } set { laneHeight = value; } }
        public float SidewalkHeight { get { return sidewalkHeight; } set { sidewalkHeight = value; } }
        public float SplitWidth { get { return splitWidth; } set { splitWidth = value; } }
        public RoadTypes RoadType{get{return type;}set{type = value;}}
        public int Limit { get { return limit; } set { limit = value; } }
        public int Segments { get { return segments; } set { segments = value; } }
        public string LeftObject { get { return leftObject; } set { leftObject = value; } }
        public string RightObject { get { return rightObject; } set { rightObject = value; } }
        public bool Traffic { get { return traffic; } set { traffic = value; } }

        public string LaneTexturePath { get { return laneTexturePath; } set { laneTexturePath = value; } }

        public string SidewalkTexturePath { get { return sideWalkTexturePath; } set { sideWalkTexturePath = value; } }

        public Road(TreeNode t):base(t)
        {
            Name = t.Text;
        }

        public void ExportToFile(string folderPath)
        {
            string[] file = new string[16];
            file[0] = "Name = "+Name;
            file[1] = "RoadType = " + (int)RoadType;
            file[2] = "Segments = " + Segments;
            file[3] = "Traffic = " + Traffic;
            file[4] = "Limit = " + Limit;
            file[5] = "LaneWidth = " + Form1.toString(LaneWidth);
            file[6] = "SidewalkWidth = " + Form1.toString(SidewalkWidth);
            file[7] = "LaneHeight = " + Form1.toString(LaneHeight);
            file[8] = "SidewalkHeight = " + Form1.toString(SidewalkHeight);
            file[9] = "SplitWidth = " + Form1.toString(SplitWidth);
            if(laneTexturePath != "")file[10] = "LaneTexturePath = " + laneTexturePath;
            if(sideWalkTexturePath != "")file[11] = "SidewalkTexturePath = " + sideWalkTexturePath;
            
            string Points = "";
            foreach (TreeNode tr in Node.Nodes)
            {
                ReferencePoint refe = tr.Tag as ReferencePoint;
                Points += (refe.X / 5f).ToString(System.Globalization.CultureInfo.InvariantCulture) + ":" + (refe.Y / 5f).ToString(System.Globalization.CultureInfo.InvariantCulture)+";";
            }
            file[13] = "Line = "+Points;
            string filename = Name.Replace(' ', '_').Replace(".", "") + ".dat";
            if (!Directory.Exists(folderPath + "/roads/")) Directory.CreateDirectory(folderPath + "/roads");
            File.WriteAllLines(folderPath + "/roads/" +filename, file);
        }
    }
}
