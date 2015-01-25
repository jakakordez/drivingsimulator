using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Slovenia_simulator.Maps
{
    class Crossroad
    {
        public int Mesh;
        Matrix4 Transform;
        int[] Roads;
        CrossroadPath[] Paths;

        public void FromFile(string path, ref MeshCollector meshes, Road[] roads)
        {

        }
    }

    public class CrossroadPath
    {
        public Vector2[] points;
        int[] MainRoads;
        int RoadA, RoadB;
    }
}
