using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using BulletSharp;
using Slovenia_simulator.Maps;

namespace Slovenia_simulator
{
    class Map
    {
        public Maps.Road[] Roads;
        Skybox CurrentSkybox;
        public Terrain CurrentTerrain;
        public Map(string mapName, ref MeshCollector meshes, RigidBodyCreation addRigidBody)
        {
            CurrentSkybox = new Maps.Skybox();
            string[] roadsFile = Directory.GetFiles("data/maps/"+mapName+"/roads/");
            Roads = new Maps.Road[roadsFile.Length];
            for (int i = 0; i < roadsFile.Length; i++)
            {
                Roads[i] = new Maps.Road();
                Roads[i].FromFile(roadsFile[i], ref meshes);
            }

            CurrentTerrain = new Terrain(addRigidBody);
        }

        public void Draw(ref MeshCollector MeshCollection, Matrix4 modelLookAt, Vector3 playerLocation)
        {
            CurrentTerrain.Draw(modelLookAt);


            for (int i = 0; i < Roads.Length; i++)
            {
                GL.LoadMatrix(ref modelLookAt);
                Roads[i].Draw(modelLookAt, ref MeshCollection);
            }
            modelLookAt = Matrix4.CreateTranslation(playerLocation) * modelLookAt;
            GL.LoadMatrix(ref modelLookAt);
            CurrentSkybox.Draw();
        }
    }
}
