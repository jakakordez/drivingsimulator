using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using OpenTK.Platform;

namespace Slovenia_simulator
{
    class Mesh
    {
        Color4[] Colors;
        uint[] ElementArrays;
        int[] ElementArraySizes;
        int VertexBuffer;

        public Mesh(string filename)
        {
            Colors = new Color4[0];
            VertexBuffer = GL.GenBuffer();
            loadObjFile(filename);
        }

        public void Draw()
        {
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

            // Vertex Array Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer); //Bind Array Buffer
            // Set the Pointer to the current bound array describing how the data ia stored
            GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);
            // Enable the client state so it will use this array buffer pointer
            GL.EnableClientState(ArrayCap.VertexArray);
            
            for (int i = 0; i < ElementArrays.Length; i++)
            {
                GL.Color4(Colors[i]);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementArrays[i]);
                GL.DrawElements(PrimitiveType.Triangles, ElementArraySizes[i], DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            // Restore the state
            GL.PopClientAttrib();
        }

        public void loadObjFile(string name)
        {
            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            s.Restart();
            Material[] Materials = new Material[0];
            Face[] Faces = new Face[0];
            Vector3[] Vertices = new Vector3[0];
            string[] file = File.ReadAllLines(name);
            int currentMaterial = 0;
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] != "")
                {
                    string[] line = file[i].Split(' ');
                    switch (line[0])
                    {
                        case "mtllib":
                            string[] filename = name.Replace('\\', '/').Split('/');
                            string result = "";
                            for (int j = 0; j < filename.Length - 1; j++) result += filename[j] + "/";
                            loadMtlFile(result + line[1], ref Materials, ref Faces);
                            break;
                        case "usemtl":
                            for (int j = 0; j < Materials.Length; j++)
                            {
                                if (Materials[j].Name == line[1])
                                {
                                    currentMaterial = j;
                                    break;
                                }
                            }
                            break;
                        case "v":
                            Misc.Push<Vector3>(new Vector3(Misc.toFloat(line[1]), Misc.toFloat(line[2]), Misc.toFloat(line[3])), ref Vertices);
                            break;
                        case "f":
                            Face f = new Face();
                            f.mtl = currentMaterial;
                            for (int j = 1; j < line.Length; j++)
                            {
                                if (line[j].Contains('/')) Misc.Push<int>(Misc.toInt(line[j].Split('/')[0]) - 1, ref f.vertices);
                            }
                            Misc.Push<Face>(f, ref Faces);
                            break;
                    }
                }
            }
            Array.Sort<Face>(Faces, delegate(Face x, Face y) { return x.mtl.CompareTo(y.mtl); });
            currentMaterial = Faces[0].mtl;
            Misc.Push<Color4>(Materials[currentMaterial].Brush, ref Colors);
            ElementArrays = new uint[Materials.Length];
            GL.GenBuffers(Materials.Length, ElementArrays);
            int[] currentElements = new int[0];
            ElementArraySizes = new int[0];
            Vector3[] currentVertices = new Vector3[0];
            int set = 0;
            long bufferSize;
            for (int i = 0; i < Faces.Length; i++)
            {
                Misc.Push<int>(Faces[i].vertices, ref currentElements);
                if (Faces.Length-1 == i || currentMaterial != Faces[i+1].mtl)
                {
                    Misc.Push<int>(currentElements.Length, ref ElementArraySizes);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementArrays[set]);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(currentElements.Length * sizeof(int)), currentElements, BufferUsageHint.StaticDraw);
                    GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                    if (currentElements.Length * sizeof(int) != bufferSize)
                        throw new ApplicationException("Element array not uploaded correctly");
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    if (Faces.Length - 1 > i)
                    {
                        set++;
                        currentMaterial = Faces[i + 1].mtl;
                        Misc.Push<Color4>(Materials[currentMaterial].Brush, ref Colors);
                        currentElements = new int[0];
                    }
                }
                
            }
            System.Diagnostics.Debugger.Log(1, "", " Prva: "+s.ElapsedMilliseconds);
            s.Restart();
            // Vertex Array Buffer
            {
                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * Vector3.SizeInBytes), Vertices, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (Vertices.Length * Vector3.SizeInBytes != bufferSize)
                    throw new ApplicationException("Vertex array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            System.Diagnostics.Debugger.Log(1, "", " Druga: "+s.ElapsedMilliseconds);
        }

        public void loadMtlFile(string filename, ref Material[] materials, ref Face[] faces)
        {
            string[] file = File.ReadAllLines(filename);
            Material currentMaterial = null;
            for (int i = 0; i < file.Length; i++)
            {
                string[] line = file[i].Split(' ');
                switch (line[0])
                {
                    case "newmtl":
                        if (currentMaterial != null) Misc.Push<Material>(currentMaterial, ref materials);
                        currentMaterial = new Material(line[1]);
                        break;
                    case "Kd":
                        if (currentMaterial != null)
                        {
                            currentMaterial.Brush.R = Misc.toFloat(line[1]);
                            currentMaterial.Brush.G = Misc.toFloat(line[2]);
                            currentMaterial.Brush.B = Misc.toFloat(line[3]);
                        }
                        break;
                    case "map_Kd":
                         if(currentMaterial != null)
                         {
                             string[] flnm = filename.Replace('\\', '/').Split('/');
                             string result = "";
                             for (int j = 0; j < flnm.Length - 1; j++) result += flnm[j] + "/";
                             currentMaterial.texture = (int)Misc.LoadTexture(result + line[1], 8);
                         }
                        break;
                }
            }
            if (currentMaterial != null) Misc.Push<Material>(currentMaterial, ref materials);
        }
    }

}
