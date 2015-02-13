using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Map_editor
{
    public class Vector2
    {
        public float X, Y;
        public Vector2(float X, float Y) { this.X = X; this.Y = Y; }
    }
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

        public Vector2[] Line;

        public Road(TreeNode t):base(t)
        {
            Name = t.Text;
            
        }

        public void ExportToFile(string folderPath)
        {
            Line = new Vector2[Node.Nodes.Count];
            for (int i = 0; i < Node.Nodes.Count; i++)
            {
                ReferencePoint refe = Node.Nodes[i].Tag as ReferencePoint;
                Line[i] = new Vector2(refe.X / 5f, refe.Y / 5f);
            }
            string file = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            string filename = Name.Replace(' ', '_').Replace(".", "") + ".json";
            if (!Directory.Exists(folderPath + "/roads/")) Directory.CreateDirectory(folderPath + "/roads");
            File.WriteAllText(folderPath + "/roads/" +filename, file);
        }
    }
}
