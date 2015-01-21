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
    class Lane
    {
        public long Length { get { return Points.Length; } }
        public Vector3[] Points;
        public int VertexBuffer, TextureCoordinateBuffer, ElementBuffer, ElementArraySize;

        public void GenerateLane(Vector2[] roadLine, int Segments, float Offset, float Width, float Height)
        {
            Vector2[] roadCurve = Misc.GetBezierApproximation(roadLine, Segments);
            Vector2[] pointsL = new Vector2[roadCurve.Length * 2];
            Vector2[] pointsR = new Vector2[roadCurve.Length * 2];
            Vector2[] TextureCoordinates = new Vector2[0];
            int[] ElementArray = new int[0];
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
                float y = (Offset + Width) * (float)Math.Sin(kot);
                float x = (Offset + Width) * (float)Math.Cos(kot);
                pointsL[i] = new Vector2(roadCurve[i].X + x, roadCurve[i].Y + y);
                y = (Offset - Width) * (float)Math.Sin(kot);
                x = (Offset - Width) * (float)Math.Cos(kot);
                pointsR[i] = new Vector2(roadCurve[i].X + x, roadCurve[i].Y + y);
            }

            for (int i = 0; i < roadCurve.Length - 1; i++)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsL[i].X, Height, pointsL[i].Y),//0 0
                    new Vector3(pointsR[i].X, Height, pointsR[i].Y),//1 0
                    new Vector3(pointsL[i+1].X, Height, pointsL[i+1].Y),//0 1
                    new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y)}, ref Points);//1 1
            }
            for (int i = roadCurve.Length - 1; i > 0; i--)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsL[i].X, Height, pointsL[i].Y),
                    new Vector3(pointsL[i].X, 0, pointsL[i].Y),
                    new Vector3(pointsL[i-1].X, Height, pointsL[i-1].Y),
                    new Vector3(pointsL[i-1].X, 0, pointsL[i-1].Y)}, ref Points);
            }
            for (int i = 0; i < roadCurve.Length - 1; i++)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsR[i].X, Height, pointsR[i].Y),
                    new Vector3(pointsR[i].X, 0, pointsR[i].Y),
                    new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y),
                    new Vector3(pointsR[i+1].X, 0, pointsR[i+1].Y)}, ref Points);
            }
            for (int i = 0; i < (roadCurve.Length - 1) * 3; i++)
            {
                Misc.Push<Vector2>(new Vector2[]{new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1)}, ref TextureCoordinates);
            }
            for (int i = 0; i < (roadCurve.Length - 1) * 3; i++)
            {
                int off = i * 4;
                Misc.Push<int>(new int[] { off+0, off+1, off+2,
                                            off+2, off+1, off+3}, ref ElementArray);
            }
            VertexBuffer = GL.GenBuffer();
            TextureCoordinateBuffer = GL.GenBuffer();
            ElementBuffer = GL.GenBuffer();
            int bufferSize;
            // Vertex Array Buffer
            {
                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Points.Length * Vector3.SizeInBytes), Points, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size

                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (Points.Length * Vector3.SizeInBytes != bufferSize)
                    throw new ApplicationException("Vertex array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            {
                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, TextureCoordinateBuffer);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(TextureCoordinates.Length * 8), TextureCoordinates, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (TextureCoordinates.Length * 8 != bufferSize)
                    throw new ApplicationException("TexCoord array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            ElementArraySize = ElementArray.Length;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(ElementArray.Length * sizeof(int)), ElementArray, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
            if (ElementArray.Length * sizeof(int) != bufferSize)
                throw new ApplicationException("Element array not uploaded correctly");
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Draw()
        {
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);
            GL.BindBuffer(BufferTarget.ArrayBuffer, TextureCoordinateBuffer);
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 8, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            // Vertex Array Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer); //Bind Array Buffer
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBuffer);
            GL.DrawElements(PrimitiveType.Triangles, ElementArraySize, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DrawElements(PrimitiveType.Lines, ElementArraySize, DrawElementsType.UnsignedInt, IntPtr.Zero);
            //GL.BindTexture(TextureTarget.Texture2D, 0);
            // Restore the state
            GL.PopClientAttrib();
        }
    }
}
