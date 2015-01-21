using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Slovenia_simulator.Maps
{
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
