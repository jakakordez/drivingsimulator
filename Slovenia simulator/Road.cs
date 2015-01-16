using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Slovenia_simulator
{
    class Road
    {
        Lane[] Lanes;
        ObjectLine[] Lines;
        int RoadType = 1;
        public void FromString(string line, ref MeshCollector meshes)
        {
            Lines = new ObjectLine[0];
            string[] components = line.Split(';');
            float Width = Misc.toFloat(components[1]);
            float Height = Misc.toFloat(components[2]);
            int Segments = Misc.toInt(components[4]);
            RoadType = Misc.toInt(components[5]);
            Vector2[] pointCollection = new Vector2[components.Length - 8];
            for (int i = 8; i < components.Length; i++)
            {
                string[] l = components[i].Split(':');
                pointCollection[i - 8] = new Vector2(Misc.toFloat(l[0]), Misc.toFloat(l[1]));
            }
            if (components[6] != "") Misc.Push<ObjectLine>(new ObjectLine(meshes.LoadMesh("./data/maps/Mapa/models/" + components[6] + "/body.obj"), pointCollection, Segments, Width + 0.3f), ref Lines);
            Lanes = new Lane[] { new Lane(), new Lane(), new Lane()};
            Lanes[0].GenerateRoad(pointCollection, Segments, 0, Width * 2, Height);
            Lanes[1].GenerateRoad(pointCollection, Segments, Width+0.75f, 1.5f, 0.2f);
            Lanes[2].GenerateRoad(pointCollection, Segments, -Width - 0.75f, 1.5f, 0.2f);
            Array.Reverse(pointCollection);
            if (components[7] != "") Misc.Push<ObjectLine>(new ObjectLine(meshes.LoadMesh("./data/maps/Mapa/models/" + components[7] + "/body.obj"), pointCollection, Segments, -Width - 0.3f), ref Lines);
        }

        public void Draw(Matrix4 world, ref MeshCollector meshes)
        {
            GL.Color4(Color4.Black);
            
            
            for (int i = 0; i < Lanes.Length; i++)
            {
                GL.Begin(PrimitiveType.TriangleStrip);
                for (int j = 0; j < Lanes[i].Length; j++)
                {
                    GL.Vertex3(Lanes[i].Points[j]);
                }
                GL.Color4(Color4.Red);
                GL.End();
            }
            
            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i].Draw(world, ref meshes);
            }
        }
    }
    class Lane
    {
        public long Length { get { return Points.Length; } }
        public Vector3[] Points;

        public void GenerateRoad(Vector2[] roadLine, int Segments, float Offset, float Width, float Height)
        {
            Vector2[] roadCurve = Misc.GetBezierApproximation(roadLine, Segments);
            Vector2[] pointsL = new Vector2[roadCurve.Length*2];
            Vector2[] pointsR = new Vector2[roadCurve.Length*2];
            Points = new Vector3[0];
            float kot = 0;
            Width /= 2;
            for (int i = 0; i < roadCurve.Length; i++)
            {
                if (i > 1 && i < roadLine.Length - 2)
                {
                    float kot1 = (float)Math.Atan((roadCurve[i - 1].Y - roadCurve[i].Y) / (roadCurve[i - 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                    float kot2 = (float)Math.Atan((roadCurve[i + 1].Y - roadCurve[i].Y) / (roadCurve[i + 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                    kot = (kot1 + kot2) / 2;
                }
                else if (i < 2) kot = (float)Math.Atan((roadCurve[i + 1].Y - roadCurve[i].Y) / (roadCurve[i + 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                else kot = (float)Math.Atan((roadCurve[i - 1].Y - roadCurve[i].Y) / (roadCurve[i - 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                float y = (Offset+Width) * (float)Math.Sin(kot);
                float x = (Offset+Width) * (float)Math.Cos(kot);
                pointsL[i] = new Vector2(roadCurve[i].X + x, roadCurve[i].Y + y);
                y = (Offset-Width) * (float)Math.Sin(kot);
                x = (Offset-Width) * (float)Math.Cos(kot);
                pointsR[i] = new Vector2(roadCurve[i].X + x, roadCurve[i].Y + y);
            }

            for (int i = 0; i < roadCurve.Length-1; i++)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsL[i].X, Height, pointsL[i].Y),
                    new Vector3(pointsR[i].X, Height, pointsR[i].Y),
                    new Vector3(pointsL[i+1].X, Height, pointsL[i+1].Y),
                    new Vector3(pointsL[i+1].X, Height, pointsL[i+1].Y),
                    new Vector3(pointsR[i].X, Height, pointsR[i].Y),
                    new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y)}, ref Points);
            }
            for (int i = roadCurve.Length-1; i > 0 ; i--)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsL[i].X, Height, pointsL[i].Y),
                    new Vector3(pointsL[i].X, 0, pointsL[i].Y),
                    new Vector3(pointsL[i-1].X, Height, pointsL[i-1].Y),
                    new Vector3(pointsL[i-1].X, Height, pointsL[i-1].Y),
                    new Vector3(pointsL[i].X, 0, pointsL[i].Y),
                    new Vector3(pointsL[i-1].X, 0, pointsL[i-1].Y)}, ref Points);
            }
            for (int i = 0; i < roadCurve.Length-1; i++)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsR[i].X, Height, pointsR[i].Y),
                    new Vector3(pointsR[i].X, 0, pointsR[i].Y),
                    new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y),
                    new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y),
                    new Vector3(pointsR[i].X, 0, pointsR[i].Y),
                    new Vector3(pointsR[i+1].X, 0, pointsR[i+1].Y)}, ref Points);
            }
        }
    }
    class ObjectLine
    {
        int ObjectMesh;
        Matrix4[] Matrices;
        public ObjectLine(int objectMesh, Vector2[] Points, int Segments, float Offset)
        {
            ObjectMesh = objectMesh;
            Points = Misc.GetBezierApproximation(Points, Segments);
            Matrices = new Matrix4[Points.Length];
            float kot;
            for (int i = 0; i < Points.Length; i++)
            {
                if (i > 1 && i < Points.Length - 2)
                {
                    float kot1 = (float)Math.Atan((Points[i - 1].Y - Points[i].Y) / (Points[i - 1].X - Points[i].X));
                    float kot2 = (float)Math.Atan((Points[i + 1].Y - Points[i].Y) / (Points[i + 1].X - Points[i].X));
                    kot = (kot1 + kot2) / 2;
                }
                else if (i < 2) kot = (float)Math.Atan((Points[i + 1].Y - Points[i].Y) / (Points[i + 1].X - Points[i].X));
                else kot = (float)Math.Atan((Points[i - 1].Y - Points[i].Y) / (Points[i - 1].X - Points[i].X));
                float y = Offset * (float)Math.Sin(kot - MathHelper.PiOver2);
                float x = Offset * (float)Math.Cos(kot - MathHelper.PiOver2);
                Matrices[i] = Matrix4.CreateRotationY(kot) * Matrix4.CreateTranslation(new Vector3(Points[i].X + x, 0, Points[i].Y + y));
            }
        }

        public void Draw(Matrix4 lookat, ref MeshCollector meshes)
        {
            Matrix4 t;
            for (int i = 0; i < Matrices.Length; i++)
            {
                t = Matrices[i] * lookat;
                GL.LoadMatrix(ref t);
                meshes.DrawMesh(ObjectMesh);
            }
        }
    }
    
}
