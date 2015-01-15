using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;

namespace Driving_simulator.Maps
{
    class Map_old
    {
        public CollisionShape groundShape;
        public Matrix4 tr;
        float[,] data;
        public Map_old()
        {
            //int width = 40, length = 40;
            int width = 1024, length = 1024; // Debugging is too slow for this
            float maxHeight = 100.0f;
            float minHeight = -100;
            //float heightScale = maxHeight / 256.0f;
            //Vector3 scale = new Vector3(1.0f, maxHeight, 1.0f);

            //PhyScalarType scalarType = PhyScalarType.PhyUChar;
            //FileStream file = new FileStream("data/maps/map1/h.raw", FileMode.Open, FileAccess.Read);

            string[] lines = System.IO.File.ReadAllLines("data/maps/map1/terrain.asc");
            data = new float[width, length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');
                for (int j = 0; j < line.Length; j++)
                {
                    data[i, j] = Misc.toFloat(line[j]);
                }
            }

            // Use float data
            PhyScalarType scalarType = PhyScalarType.PhyFloat;
            byte[] terr = new byte[width * length * 4];
            MemoryStream file = new MemoryStream(terr);
            BinaryWriter writer = new BinaryWriter(file);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < length; j++)
                    writer.Write(data[i, j]);// Math.Sin(j * 0.5f) * Math.Cos(i)
            writer.Flush();
            file.Position = 0;

            HeightfieldTerrainShape heightterrainShape = new HeightfieldTerrainShape(width, length,
                file, 1, minHeight, maxHeight, 1, scalarType, false);
            heightterrainShape.SetUseDiamondSubdivision(true);

            groundShape = heightterrainShape;

            tr = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
        }
        public void Draw(Vector3 camera)
        {
            camera += new Vector3(512, 0, 512);
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.White);
           // float w = 512, h = 512;
            int z = 2;
            int s = 8 * z;
            s = 1;
            Vector2 lowLimit, highLimit;
            lowLimit = new Vector2((camera.X / s) - 100, (camera.Z / s) - 100);
            highLimit = new Vector2((camera.X / s) + 100, (camera.Z / s) + 100);
            if (lowLimit.X < 0) lowLimit.X = 0;
            if (lowLimit.Y < 0) lowLimit.Y = 0;
            for (int i = (int)lowLimit.X; i < highLimit.X; i++)
            {
                for (int j = (int)lowLimit.Y; j < highLimit.Y; j++)
                {
                    Vector2 P = new Vector2(i-512, j-512);
                    
                    //GL.TexCoord2(i / w, j / h);
                    GL.Color4(Color.Red);
                    GL.Vertex3(P.X, data[i, 1024-j], P.Y);
                    GL.Color4(Color.Yellow);
                    //GL.TexCoord2(i / w, (j + 1) / h);
                    GL.Vertex3(P.X, data[i, 1024-(j + 1)], P.Y + s);
                    GL.Color4(Color.Blue);
                    //GL.TexCoord2((i + 1) / w, (j + 1) / h);
                    GL.Vertex3(P.X + s, data[(i + 1) , 1024-(j + 1)], P.Y + s);
                    //GL.TexCoord2((i + 1) / w, j / h);
                    GL.Vertex3(P.X + s, data[(i + 1), 1024-j ], P.Y);
                }
            }
            GL.End();
            /*GL.BindTexture(TextureTarget.Texture2D, smallTexture);
            GL.Begin(BeginMode.Quads);

            w = 128;
            h = 128;
            z = 8;
            s = 8 * z;
            for (int i = 0; i < w - 1; i++)
            {
                for (int j = 0; j < h - 1; j++)
                {
                    int d = (int)(new Vector2(i * s, j * s) - new Vector2(camera.X, camera.Z)).LengthFast;
                    if (d > 450 && d < 6000)
                    {
                        //GL.Color3(Color.BurlyWood);
                        GL.TexCoord2(i / w, j / h);
                        GL.Vertex3(i * s, heightmap[i * z, j * z], j * s);
                        GL.TexCoord2((i + 1) / w, j / h);
                        GL.Vertex3((i + 1) * s, heightmap[(i + 1) * z, j * z], j * s);
                        GL.TexCoord2((i + 1) / w, (j + 1) / h);
                        GL.Vertex3((i + 1) * s, heightmap[(i + 1) * z, (j + 1) * z], (j + 1) * s);
                        GL.TexCoord2(i / w, (j + 1) / h);
                        GL.Vertex3(i * s, heightmap[i * z, (j + 1) * z], (j + 1) * s);
                    }
                }
            }
            GL.End();*/
        }
    }
}
