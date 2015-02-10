using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Slovenia_simulator.Maps
{
    class Path
    {
        public Vector3[] Points;

        public Path(Vector2[] roadLine, int Segments, float Offset, float Height, bool Invert)
        {
            Vector2[] roadCurve = Misc.GetBezierApproximation(roadLine, Segments);
            Points = new Vector3[Segments+1];
            float kot = 0;
            for (int i = 0; i < Segments+1; i++)
            {
                if (i > 1 && i < roadLine.Length - 2)
                {
                    float kot1 = (float)Math.Atan((roadCurve[i - 1].Y - roadCurve[i].Y) / (roadCurve[i - 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                    float kot2 = (float)Math.Atan((roadCurve[i + 1].Y - roadCurve[i].Y) / (roadCurve[i + 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                    kot = (kot1 + kot2) / 2;
                }
                else if (i < 2) kot = (float)Math.Atan((roadCurve[i + 1].Y - roadCurve[i].Y) / (roadCurve[i + 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                else kot = (float)Math.Atan((roadCurve[i - 1].Y - roadCurve[i].Y) / (roadCurve[i - 1].X - roadCurve[i].X)) - MathHelper.PiOver2;
                float y = (Offset + Offset) * (float)Math.Sin(kot);
                float x = (Offset + Offset) * (float)Math.Cos(kot);
                Points[(Invert)?Segments-i:i] = new Vector3(roadCurve[i].X + x, Height, roadCurve[i].Y + y);
            }
        }

        public Path(Vector3[] Points)
        {
            this.Points = Points;
        }
    }
}
