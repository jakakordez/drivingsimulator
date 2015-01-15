using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Driving_simulator.Maps
{
    class Map
    {
        Road[] Roads;
        public Map(string mapName, ref MeshCollector meshes)
        {
            string[] roadsFile = System.IO.File.ReadAllLines("data/maps/"+mapName+"/roads.dat");
            Roads = new Road[0];
            for (int i = 0; i < roadsFile.Length; i++)
            {
                Road r = new Road();
                r.FromString(roadsFile[i], ref meshes);
                Misc.Push<Road>(r, ref Roads);
            }
        }

        public void Draw(ref MeshCollector MeshCollection, Matrix4 modelLookAt)
        {
            for (int i = 0; i < Roads.Length; i++)
            {
                GL.LoadMatrix(ref modelLookAt);
                Roads[i].Draw(modelLookAt, ref MeshCollection);
            }
        }
    }
}
