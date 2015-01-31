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
        Path[] Paths;

        public void FromFile(string path, ref MeshCollector meshes, Road[] roads)
        {

        }
    }
}
