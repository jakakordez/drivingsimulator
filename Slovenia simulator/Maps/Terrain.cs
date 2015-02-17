using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Slovenia_simulator.Maps
{
    class Terrain
    {
        public RigidBody ground;
        Vector3[] points;
        int s = 601;
        Vector3 scale;
        int grass;
        Meshes.BufferedObject obj;
        public Terrain(RigidBodyCreation addRigidbody)
        {
            grass = Misc.LoadTexture("data/maps/Mapa/textures/grass.png", 1);
            PhyScalarType scalarType = PhyScalarType.PhyFloat;
            //FileStream file = new FileStream("test.raw", FileMode.Open, FileAccess.Read);
            scale = new Vector3(92, 1, 64); //new Vector3(10, 1, 10);
            // Use float data
             byte[] terr = new byte[s * s * 4];
             MemoryStream file = new MemoryStream(terr);
             BinaryWriter writer = new BinaryWriter(file);
            Random r = new Random();
             for (int i = 0; i < s; i++)
                 for (int j = 0; j < s; j++)
                     writer.Write((float)0);
             writer.Flush();
             file.Position = 0;
            HeightfieldTerrainShape t = new HeightfieldTerrainShape(s, s, file, 1, -3000, 3000, 1, scalarType, false);
            t.Margin = 0;
            points = new Vector3[s* s];
            file.Position = 0;
            byte[] b = new byte[4];
            Vector2[] texturecoords = new Vector2[s*s];
            for (int i = 0; i < file.Length; i+=4)
            {
                file.Read(b, 0, 4);
                points[i / 4] = new Vector3((i / 4)%s, BitConverter.ToSingle(b, 0), ((i/4) / s))*scale;
                texturecoords[i / 4] = new Vector2((i / 4) % s, ((i / 4) / s)) * scale.Xz;
            }
            int[] indicies = new int[(s-1)*(s-1)*6];
            int c = 0;
            for (int i = 0; i < points.Length - s; i++)
            {
                if ((i % s) < s - 1)
                {
                    indicies[c++] = i;
                    indicies[c++] = i + 1;
                    indicies[c++] = i + 1+s;
                    indicies[c++] = i + 1+s;
                    indicies[c++] = i + s;
                    indicies[c++] = i;
                }
            }
            obj = new Meshes.BufferedObject(points, indicies, texturecoords);
            t.LocalScaling =scale;
            ground = addRigidbody(0, Matrix4.CreateTranslation(new Vector3(0, 0, 0)), t);
        }

        public void Draw(Matrix4 lookat)
        {
            Matrix4 mat = Matrix4.CreateTranslation(new Vector3((s-1)*scale.X, 0, (s-1)*scale.Z)*-0.5f)*ground.WorldTransform * lookat;
            GL.LoadMatrix(ref mat);
            GL.Color4(Color4.White);
            GL.BindTexture(TextureTarget.Texture2D, grass);
            obj.Draw();
        }
    }
}
