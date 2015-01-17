using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Slovenia_simulator.Maps
{
    class Map
    {
        Road[] Roads;
        public Map(string mapName, ref MeshCollector meshes)
        {
            string[] roadsFile = Directory.GetFiles("data/maps/"+mapName+"/roads/");
            Roads = new Road[roadsFile.Length];
            for (int i = 0; i < roadsFile.Length; i++)
            {
                Roads[i] = new Road();
                Roads[i].FromFile(roadsFile[i], ref meshes);
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
