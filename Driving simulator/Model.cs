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

namespace Driving_simulator
{
    class Model
    {
        Vector3[] vertices;
        Face[] faces;
        public Material[] materials;
        
        public Model(string filename)
        {
            vertices = new Vector3[0];
            faces = new Face[0];
            materials = new Material[0];
            loadObjFile(filename);
        }

        public void Draw()
        {
            int m = -1;
            Material faceMaterial = new Material("");
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < faces.Length; i++)
            {
                if (faces[i].mtl != m){
                    faceMaterial = materials[faces[i].mtl];
                    if (faceMaterial.texture != 0)
                    {
                        GL.Color4(Color.White);
                        GL.BindTexture(TextureTarget.Texture2D, faceMaterial.texture);
                        GL.Begin(PrimitiveType.Triangles);
                    }
                    else GL.Color4(faceMaterial.Brush);
                    m = faces[i].mtl; 
                    //GL.TexCoord2(0, 0);
                }
                for (int j = 0; j < faces[i].vertices.Length; j++)GL.Vertex3(vertices[faces[i].vertices[j]]);
                if (i < faces.Length-2 && faces[i+1].mtl != m && materials[faces[i+1].mtl].texture != 0) GL.End();
            }
            GL.End();
        }

        public void loadObjFile(string name)
        {
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
                            for (int j = 0; j < filename.Length-1; j++) result += filename[j] + "/";
                            loadMtlFile(result + line[1]);
                            break;
                        case "usemtl":
                            for (int j = 0; j < materials.Length; j++)
                            {
                                if (materials[j].Name == line[1]) {
                                    currentMaterial = j; 
                                    break;
                                }
                            }
                            break;
                        case "v":
                            Misc.Push<Vector3>(new Vector3(Misc.toFloat(line[1]), Misc.toFloat(line[2]), Misc.toFloat(line[3])), ref vertices);
                            break;
                        case "f":
                            Face f = new Face();
                            f.mtl = currentMaterial;
                            for (int j = 1; j < line.Length; j++)
                            {
                                if(line[j].Contains('/')) Misc.Push<int>(Misc.toInt(line[j].Split('/')[0]) - 1, ref f.vertices);
                            }
                            Misc.Push<Face>(f, ref faces);
                            break;
                    }
                }
            }
            Array.Sort<Face>(faces, delegate(Face x, Face y) { return x.mtl.CompareTo(y.mtl); });
        }


        public void loadMtlFile(string filename)
        {
            string[] file = File.ReadAllLines(filename);
            Material currentMaterial = null;
            for (int i = 0; i < file.Length; i++)
            {
                string[] line = file[i].Split(' ');
                switch(line[0])
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
                       /* if(currentMaterial != null)
                        {
                            string[] flnm = filename.Replace('\\', '/').Split('/');
                            string result = "";
                            for (int j = 0; j < flnm.Length - 1; j++) result += flnm[j] + "/";
                            currentMaterial.texture = (int)Misc.LoadTexture(result + line[1], 8);
                        }*/
                        break;
                }
            }
            if (currentMaterial != null) Misc.Push<Material>(currentMaterial, ref materials);
        }
    }

    public class Material
    {
        public Color4 Brush;
        public string Name;
        public int texture;
        public Material(string name)
        {
            Name = name;
            Brush = new Color4();
            Brush.A = 255;
            texture = 0;
        }
    }

    public class Face
    {
        public int[] vertices;
        public int mtl;
        public Face(int[] v)
        {
            vertices = v;
        }
        public Face()
        {
            vertices = new int[0];
        }
    }
}
