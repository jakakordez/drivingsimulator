using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_simulator
{
    class MeshCollector
    {
        Mesh[] Meshes;
        string[] MeshNames;

        public MeshCollector()
        {
            Meshes = new Mesh[0];
            MeshNames = new string[0];
        }

        public int LoadMesh(string Path)
        {
            System.Diagnostics.Stopwatch stw = new System.Diagnostics.Stopwatch();
            stw.Restart();
            for (int i = 0; i < MeshNames.Length; i++)
            {
                if (Path == MeshNames[i]) return i;
            }
            Misc.Push<string>(Path, ref MeshNames);
            int a = Misc.Push<Mesh>(new Mesh(Path), ref Meshes);
            System.Diagnostics.Debugger.Log(1, "", "\n"+Path + " : " + stw.ElapsedMilliseconds);
            return a;
        }

        public void DrawMesh(int i)
        {
            Meshes[i].Draw();
        }
    }
}
