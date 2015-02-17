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
    public class Mesh
    {
        Material[] Materials;
        uint[] ElementArrays;
        int[] ElementArraySizes;
        int VertexBuffer;
        int TextureCoordinateBuffer;

        public Mesh(string filename)
        {
            Materials = new Material[0];
            VertexBuffer = GL.GenBuffer();
            TextureCoordinateBuffer = GL.GenBuffer();
            string[] parts = filename.Split('.');
            switch (parts[parts.Length-1].ToLower())
            {
                case "mesh":loadMeshFile(filename);
                    break;
                case "obj":loadObjFile(filename, true);
                    break;
            }
            
        }

        public Mesh() { Materials = new Material[0]; }

        public void Draw()
        {

            for (int i = 0; i < ElementArraySizes.Length; i++)
            {
               
                if (Materials[i].Texture != 0)
                {
                    GL.Color4(Color.White);
                    GL.BindTexture(TextureTarget.Texture2D, Materials[i].Texture);
                   // GL.TexCoord2(new Vector2(0f, 0f));
                }
                else GL.Color4(Materials[i].Brush);
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
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementArrays[i]);
                GL.DrawElements(PrimitiveType.Triangles, ElementArraySizes[i], DrawElementsType.UnsignedInt, IntPtr.Zero);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                // Restore the state
                GL.PopClientAttrib();
            }
        }

        public void loadMeshFile(string filename)
        {
            loadMtlFile(filename.Replace(".mesh", ".mtl"), ref Materials, true);
            string[] data = File.ReadAllText(filename).Split(';');
            string[] vertices = data[0].Split(' ');
            string[] texcoords = data[1].Split(' ');
            Vector3[] SortedVertices = new Vector3[vertices.Length/3];
            Vector2[] SortedTextureCoordinates = new Vector2[texcoords.Length/2];
            for (int i = 0; i < vertices.Length-1; i+=3)
            {
                SortedVertices[i / 3] = new Vector3(Misc.toFloat(vertices[i]), Misc.toFloat(vertices[i + 1]), Misc.toFloat(vertices[i + 2]));
            }
            for (int i = 0; i < texcoords.Length-1; i+=2)
            {
                SortedTextureCoordinates[i / 2] = new Vector2(Misc.toFloat(texcoords[i]), Misc.toFloat(texcoords[i + 1]));
            }
            int bufferSize;
            ElementArrays = new uint[Materials.Length];
            ElementArraySizes = new int[0];
            GL.GenBuffers(Materials.Length, ElementArrays);
            for (int i = 0; i < data.Length-2; i++)
            {
                if (data[i + 2] != "")
                {
                    string[] indicies = data[i + 2].Split(' ');
                    int[] currentElements = new int[indicies.Length];
                    for (int j = 0; j < indicies.Length; j++)
                    {
                        currentElements[j] = Misc.toInt(indicies[j]);
                    }
                    Misc.Push<int>(currentElements.Length, ref ElementArraySizes);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementArrays[i]);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(currentElements.Length * sizeof(int)), currentElements, BufferUsageHint.StaticDraw);
                    GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                    if (currentElements.Length * sizeof(int) != bufferSize)
                        throw new ApplicationException("Element array not uploaded correctly");
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                }
            }
           
            // Vertex Array Buffer
            {
                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(SortedVertices.Length * Vector3.SizeInBytes), SortedVertices, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size

                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (SortedVertices.Length * Vector3.SizeInBytes != bufferSize)
                    throw new ApplicationException("Vertex array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            {
                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, TextureCoordinateBuffer);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(SortedTextureCoordinates.Length * 8), SortedTextureCoordinates, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (SortedTextureCoordinates.Length * 8 != bufferSize)
                    throw new ApplicationException("TexCoord array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            //object[] a = loadObjFile(filename.Replace(".mesh", ".obj"), false);
        }

        public object[] loadObjFile(string name, bool load)
        {
            Face[] Faces = new Face[0];
            Vector3[] OriginalVertices = new Vector3[0];
            Vector2[] OriginalTextureCoordinates = new Vector2[0];
            Vector3[] SortedVertices = new Vector3[0];
            Vector2[] SortedTextureCoordinates = new Vector2[0];
           
            string[] file = File.ReadAllLines(name);
            int currentMaterial = 0;
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] != "" && file[i][0] != '#')
                {
                    string[] line = file[i].Split(' ');
                    switch (line[0])
                    {
                        case "mtllib":
                            string[] filename = name.Replace('\\', '/').Split('/');
                            string result = "";
                            for (int j = 0; j < filename.Length - 1; j++) result += filename[j] + "/";
                            loadMtlFile(result + line[1], ref Materials, load);
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
                            Misc.Push<Vector3>(new Vector3(Misc.toFloat(line[1]), Misc.toFloat(line[2]), Misc.toFloat(line[3])), ref OriginalVertices);
                            break;
                        case "f":
                            Face f = new Face();
                            f.mtl = currentMaterial;
                            for (int j = 1; j < line.Length; j++)
                            {
                                if (line[j].Contains('/'))
                                {
                                    string[] fac = line[j].Split('/');
                                    int v = Misc.Push<Vector3>(OriginalVertices[Misc.toInt(fac[0]) - 1], ref SortedVertices);
                                    int t = Misc.Push<Vector2>(OriginalTextureCoordinates[Misc.toInt(fac[1]) - 1], ref SortedTextureCoordinates);
                                    Misc.Push<int>(v, ref f.vertices);
                                }
                            }
                            Misc.Push<Face>(f, ref Faces);
                            break;
                        case "vt":
                            Misc.Push<Vector2>(new Vector2(Misc.toFloat(line[2]), Misc.toFloat(line[1])), ref OriginalTextureCoordinates);
                            break;
                    }
                }
            }
            Array.Sort<Face>(Faces, delegate(Face x, Face y) { return x.mtl.CompareTo(y.mtl); });
            currentMaterial = Faces[0].mtl;
            //Misc.Push<Material>(Materials[currentMaterial].Brush, ref Materials);
            ElementArrays = new uint[Materials.Length];
            if(load)GL.GenBuffers(Materials.Length, ElementArrays);
            int[] currentElements = new int[0];
            List<int[]> faces = new List<int[]>(Materials.Length);
            ElementArraySizes = new int[0];
            int set = 0;
            long bufferSize;
            for (int i = 0; i < Faces.Length; i++)
            {
                Misc.Push<int>(Faces[i].vertices, ref currentElements);
                if (Faces.Length-1 == i || currentMaterial != Faces[i+1].mtl)
                {
                    Misc.Push<int>(currentElements.Length, ref ElementArraySizes);
                    if (load)
                    {
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementArrays[set]);
                        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(currentElements.Length * sizeof(int)), currentElements, BufferUsageHint.StaticDraw);
                        GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                        if (currentElements.Length * sizeof(int) != bufferSize)
                            throw new ApplicationException("Element array not uploaded correctly");
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    }
                    else faces.Add(currentElements);
                    if (Faces.Length - 1 > i)
                    {
                        set++;
                        currentMaterial = Faces[i + 1].mtl;
                        Misc.Push<Material>(Materials[currentMaterial], ref Materials);
                        currentElements = new int[0];
                    }
                }
                
            }
            if (load)
            {
                // Vertex Array Buffer
                {
                    // Bind current context to Array Buffer ID
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);

                    // Send data to buffer
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(SortedVertices.Length * Vector3.SizeInBytes), SortedVertices, BufferUsageHint.StaticDraw);

                    // Validate that the buffer is the correct size

                    GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                    if (SortedVertices.Length * Vector3.SizeInBytes != bufferSize)
                        throw new ApplicationException("Vertex array not uploaded correctly");

                    // Clear the buffer Binding
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                }
                {
                    // Bind current context to Array Buffer ID
                    GL.BindBuffer(BufferTarget.ArrayBuffer, TextureCoordinateBuffer);

                    // Send data to buffer
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(SortedTextureCoordinates.Length * 8), SortedTextureCoordinates, BufferUsageHint.StaticDraw);

                    // Validate that the buffer is the correct size
                    GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                    if (SortedTextureCoordinates.Length * 8 != bufferSize)
                        throw new ApplicationException("TexCoord array not uploaded correctly");

                    // Clear the buffer Binding
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                }
            }

            return new object[] { SortedVertices, SortedTextureCoordinates, faces};
        }

        public void loadMtlFile(string filename, ref Material[] materials, bool load)
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
                             if(load) currentMaterial.Texture = (int)Misc.LoadTexture(result + line[1], 1);
                             currentMaterial.TexturePath = result + line[1];
                         }
                        break;
                }
            }
            if (currentMaterial != null) Misc.Push<Material>(currentMaterial, ref materials);
        }

    }

}
