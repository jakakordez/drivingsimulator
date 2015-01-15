using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Map_editor
{
    public enum RoadTypes
    {
        OneLane,
        TwoLanes,
        FourLanes,
        SixLanes
    }
    public class Road:Type
    {
        float height = 0.02f, width = 3;
        int limit = 50, segments = 10;
        RoadTypes type = RoadTypes.TwoLanes;
        string leftObject, rightObject;

        public float Height { get { return height; } set { height = value; } }
        public float LaneWidth{get{return width;}set{width = value;}}
        public RoadTypes Type{get{return type;}set{type = value;}}
        public int Limit { get { return limit; } set { limit = value; } }
        public int Segments { get { return segments; } set { segments = value; } }
        public string LeftObject { get { return leftObject; } set { leftObject = value; } }
        public string RightObject { get { return rightObject; } set { rightObject = value; } }

        public Road(TreeNode t):base(t)
        {
            Name = t.Text;
        }
    }
}
