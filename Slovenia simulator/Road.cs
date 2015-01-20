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
        Lane[] drivingLanes, auxiliaryLanes;
        public Vector2[] Line;
        ObjectLine[] Lines;
        public string Name, LaneTexturePath, SidewalkTexturePath;
        public int RoadType = 3, Segments = 5;
        private int LaneTexture, SidewalkTexture;
        public bool Traffic = false;
        public float LaneWidth = 3, SidewalkWidth = 1, LaneHeight = 0.02f, SidewalkHeight = 0.1f, SplitWidth = 1;
        public void FromFile(string path, ref MeshCollector meshes)
        {
            //if (components[6] != "") Misc.Push<ObjectLine>(new ObjectLine(meshes.LoadMesh("./data/maps/Mapa/models/" + components[6] + "/body.obj"), pointCollection, Segments, Width + 0.3f), ref Lines);*/
            string[] file = System.IO.File.ReadAllLines(path);
            DataParser.ParseData(file, this);
            auxiliaryLanes = new Lane[0];
            Lines = new ObjectLine[0];
            if (LaneTexturePath != null) LaneTexture = Misc.LoadTexture(LaneTexturePath, 1);
            if (SidewalkTexturePath != null) SidewalkTexture = Misc.LoadTexture(SidewalkTexturePath, 1);
            switch (RoadType)
            {
                case 1: //one lane, one way
                    drivingLanes = new Lane[]{new Lane()};
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth, LaneHeight);
                    break;
                case 2: //two lanes
                case 3: //two lanes, one way
                    drivingLanes = new Lane[]{new Lane()};
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    break;
                case 4: //two lanes, one way with sidewalk
                case 5: //two lanes with sidewalk
                    drivingLanes = new Lane[]{new Lane()};
                    auxiliaryLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    auxiliaryLanes[0].GenerateLane(Line, Segments, LaneWidth + (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    auxiliaryLanes[1].GenerateLane(Line, Segments, -LaneWidth - (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    break;
                case 6: //two lanes with sidewalk on one side
                    drivingLanes = new Lane[]{new Lane()};
                    auxiliaryLanes = new Lane[] { new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    auxiliaryLanes[0].GenerateLane(Line, Segments, LaneWidth + (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    break;
                case 7: //four lanes
                    drivingLanes = new Lane[]{new Lane()};
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 4, LaneHeight);
                    break;
                case 8: //four lanes with sidewalk
                    drivingLanes = new Lane[]{new Lane()};
                    auxiliaryLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 4, LaneHeight);
                    auxiliaryLanes[0].GenerateLane(Line, Segments, (LaneWidth * 2) + (SidewalkWidth / 2), SidewalkWidth * 2, SidewalkHeight);
                    auxiliaryLanes[1].GenerateLane(Line, Segments, -(LaneWidth * 2) - (SidewalkWidth / 2), SidewalkWidth * 2, SidewalkHeight);
                    break;
                case 9: //2X two lanes
                    drivingLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, (SplitWidth / 2) + LaneWidth, LaneWidth * 2, LaneHeight);
                    drivingLanes[1].GenerateLane(Line, Segments, -(SplitWidth / 2) - LaneWidth, LaneWidth * 2, LaneHeight);
                    break;
                case 10: //2X two lanes with emergency lanes
                case 11: //2X three lanes
                    drivingLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, (SplitWidth / 2) + (LaneWidth * 1.5f), LaneWidth * 3, LaneHeight);
                    drivingLanes[1].GenerateLane(Line, Segments, -(SplitWidth / 2) - (LaneWidth * 1.5f), LaneWidth * 3, LaneHeight);
                    break;
                case 12: //2X three lanes with emergency lanes
                case 13: //2X four lanes
                    drivingLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, (SplitWidth / 2) + (LaneWidth * 2), LaneWidth * 4, LaneHeight);
                    drivingLanes[1].GenerateLane(Line, Segments, -(SplitWidth / 2) - (LaneWidth * 2), LaneWidth * 4, LaneHeight);
                    break;
            }
            //if (components[7] != "") Misc.Push<ObjectLine>(new ObjectLine(meshes.LoadMesh("./data/maps/Mapa/models/" + components[7] + "/body.obj"), pointCollection, Segments, -Width - 0.3f), ref Lines);
        }

        public void Draw(Matrix4 world, ref MeshCollector meshes)
        {
            
            for (int i = 0; i < drivingLanes.Length; i++)
            {
                //GL.Begin(PrimitiveType.Triangles);
                /*for (int j = 0; j < drivingLanes[i].Length; j+=6)
                {
                    GL.TexCoord2(new Vector2(0, 0));
                    GL.Vertex3(drivingLanes[i].Points[j]);
                    GL.TexCoord2(new Vector2(1, 0));
                    GL.Vertex3(drivingLanes[i].Points[j+1]);
                    GL.TexCoord2(new Vector2(0, 1));
                    GL.Vertex3(drivingLanes[i].Points[j+2]);
                    GL.TexCoord2(new Vector2(0, 1));
                    GL.Vertex3(drivingLanes[i].Points[j + 3]);
                    GL.TexCoord2(new Vector2(1, 0));
                    GL.Vertex3(drivingLanes[i].Points[j + 4]);
                    GL.TexCoord2(new Vector2(1, 1));
                    GL.Vertex3(drivingLanes[i].Points[j + 5]);
                }*/
                if (LaneTexture != 0) { GL.BindTexture(TextureTarget.Texture2D, LaneTexture); GL.Color4(Color4.White); }
                else GL.Color4(Color4.Black);
                drivingLanes[i].Draw();
                //GL.End();
            }
            
            for (int i = 0; i < auxiliaryLanes.Length; i++)
            {
                /*GL.Begin(PrimitiveType.Triangles);
                for (int j = 0; j < auxiliaryLanes[i].Length; j+=6)
                {
                    GL.TexCoord2(new Vector2(0, 0));
                    GL.Vertex3(auxiliaryLanes[i].Points[j]);
                    GL.TexCoord2(new Vector2(1, 0));
                    GL.Vertex3(auxiliaryLanes[i].Points[j + 1]);
                    GL.TexCoord2(new Vector2(0, 1));
                    GL.Vertex3(auxiliaryLanes[i].Points[j + 2]);
                    GL.TexCoord2(new Vector2(0, 1));
                    GL.Vertex3(auxiliaryLanes[i].Points[j + 3]);
                    GL.TexCoord2(new Vector2(1, 0));
                    GL.Vertex3(auxiliaryLanes[i].Points[j + 4]);
                    GL.TexCoord2(new Vector2(1, 1));
                    GL.Vertex3(auxiliaryLanes[i].Points[j + 5]);
                }
                GL.End();*/
                if (SidewalkTexture != 0) GL.BindTexture(TextureTarget.Texture2D, SidewalkTexture);
                else GL.Color4(Color4.Black);
                auxiliaryLanes[i].Draw();
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
        public int VertexBuffer, TextureCoordinateBuffer, ElementBuffer, ElementArraySize;

        public void GenerateLane(Vector2[] roadLine, int Segments, float Offset, float Width, float Height)
        {
            Vector2[] roadCurve = Misc.GetBezierApproximation(roadLine, Segments);
            Vector2[] pointsL = new Vector2[roadCurve.Length*2];
            Vector2[] pointsR = new Vector2[roadCurve.Length*2];
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
                    new Vector3(pointsL[i].X, Height, pointsL[i].Y),//0 0
                    new Vector3(pointsR[i].X, Height, pointsR[i].Y),//1 0
                    //new Vector3(pointsL[i+1].X, Height, pointsL[i+1].Y),//0 1
                    new Vector3(pointsL[i+1].X, Height, pointsL[i+1].Y),//0 1
                    //new Vector3(pointsR[i].X, Height, pointsR[i].Y),//1 0
                    new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y)}, ref Points);//1 1
            }
            for (int i = roadCurve.Length-1; i > 0 ; i--)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsL[i].X, Height, pointsL[i].Y),
                    new Vector3(pointsL[i].X, 0, pointsL[i].Y),
                    //new Vector3(pointsL[i-1].X, Height, pointsL[i-1].Y),
                    new Vector3(pointsL[i-1].X, Height, pointsL[i-1].Y),
                    //new Vector3(pointsL[i].X, 0, pointsL[i].Y),
                    new Vector3(pointsL[i-1].X, 0, pointsL[i-1].Y)}, ref Points);
            }
            for (int i = 0; i < roadCurve.Length-1; i++)
            {
                Misc.Push<Vector3>(new Vector3[]{
                    new Vector3(pointsR[i].X, Height, pointsR[i].Y),
                    new Vector3(pointsR[i].X, 0, pointsR[i].Y),
                    //new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y),
                    new Vector3(pointsR[i+1].X, Height, pointsR[i+1].Y),
                    //new Vector3(pointsR[i].X, 0, pointsR[i].Y),
                    new Vector3(pointsR[i+1].X, 0, pointsR[i+1].Y)}, ref Points);
            }
            for (int i = 0; i < (roadCurve.Length-1)*3; i++)
            {
                Misc.Push<Vector2>(new Vector2[]{new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    /*new Vector2(0, 1)/*,
                    new Vector2(1, 1)*/}, ref TextureCoordinates);
            }
            for (int i = 0; i < (roadCurve.Length-1)*3; i++)
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
