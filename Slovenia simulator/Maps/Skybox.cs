using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Slovenia_simulator.Maps
{
    class Skybox
    {
        public float distance;
        int texture;

        public Skybox()
        {
            distance = 500;
            texture = Misc.LoadTexture("data/maps/Mapa/textures/skybox.png", 1);
        }

        public void Draw()
        {
            GL.Color4(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Begin(PrimitiveType.Quads);

            // UP
            GL.TexCoord2(new Vector2(0.66f, 0.5f));
            GL.Vertex3(new Vector3(-distance, distance, distance));
            GL.TexCoord2(new Vector2(0.66f, 0.25f));
            GL.Vertex3(new Vector3(distance, distance, distance));
            GL.TexCoord2(new Vector2(1, 0.25f));
            GL.Vertex3(new Vector3(distance, distance, -distance));
            GL.TexCoord2(new Vector2(1, 0.5f));
            GL.Vertex3(new Vector3(-distance, distance, -distance));

            // FRONT
            GL.TexCoord2(new Vector2(0.33f, 0.5f));
            GL.Vertex3(new Vector3(-distance, -distance, distance));
            GL.TexCoord2(new Vector2(0.33f, 0.25f));
            GL.Vertex3(new Vector3(distance, -distance, distance));
            GL.TexCoord2(new Vector2(0.66f, 0.25f));
            GL.Vertex3(new Vector3(distance, distance, distance));
            GL.TexCoord2(new Vector2(0.66f, 0.5f));
            GL.Vertex3(new Vector3(-distance, distance, distance));
            // REAR
            GL.TexCoord2(new Vector2(0.33f, 0.75f));
            GL.Vertex3(new Vector3(-distance, -distance, -distance));
            GL.TexCoord2(new Vector2(0.33f, 1));
            GL.Vertex3(new Vector3(distance, -distance, -distance));
            GL.TexCoord2(new Vector2(0.66f, 1));
            GL.Vertex3(new Vector3(distance, distance, -distance));
            GL.TexCoord2(new Vector2(0.66f, 0.75f));
            GL.Vertex3(new Vector3(-distance, distance, -distance));
            // LEFT
            GL.TexCoord2(new Vector2(0.33f, 0));
            GL.Vertex3(new Vector3(distance, -distance, -distance));
            GL.TexCoord2(new Vector2(0.33f, 0.25f));
            GL.Vertex3(new Vector3(distance, -distance, distance));
            GL.TexCoord2(new Vector2(0.66f, 0.25f));
            GL.Vertex3(new Vector3(distance, distance, distance));
            GL.TexCoord2(new Vector2(0.66f, 0));
            GL.Vertex3(new Vector3(distance, distance, -distance));

            // RIGHT
            GL.TexCoord2(new Vector2(0.33f, 0.75f));
            GL.Vertex3(new Vector3(-distance, -distance, -distance));
            GL.TexCoord2(new Vector2(0.33f, 0.5f));
            GL.Vertex3(new Vector3(-distance, -distance, distance));
            GL.TexCoord2(new Vector2(0.66f, 0.5f));
            GL.Vertex3(new Vector3(-distance, distance, distance));
            GL.TexCoord2(new Vector2(0.66f, 0.75f));
            GL.Vertex3(new Vector3(-distance, distance, -distance));

            GL.End();
        }
    }
}
