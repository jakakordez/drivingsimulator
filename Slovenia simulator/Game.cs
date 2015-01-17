using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Threading;

namespace Slovenia_simulator
{
    class Game:GameWindow
    {
        int fps;
        float frameTime;
        Camera camera;
        Physics p;
        TextRenderer renderer;
        MeshCollector MeshCollection;
        Font serif = new Font(FontFamily.GenericSerif, 24);
        Maps.Map currentMap;
        public Game()
            : base(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, new GraphicsMode(), "Driving simulator")
        {
            VSync = VSyncMode.On;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = OpenTK.WindowState.Fullscreen;
            System.Diagnostics.Stopwatch stw = new System.Diagnostics.Stopwatch();
            stw.Restart();
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color.CornflowerBlue);

            GL.Enable(EnableCap.ColorMaterial);
            //GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Light0);
            renderer = new TextRenderer(100, 50);
            //GL.Enable(EnableCap.Lighting);
            System.Windows.Forms.Cursor.Hide();
            MeshCollection = new MeshCollector();
            System.Diagnostics.Debugger.Log(1, "Timing", "\nZačetek: " + stw.ElapsedMilliseconds);
            camera = new Camera(this.Height, this.Width);
            currentMap = new Maps.Map("Mapa", ref MeshCollection);
            System.Diagnostics.Debugger.Log(1, "Timing", "\nMapa: " + stw.ElapsedMilliseconds);
           
            p = new Physics(ref MeshCollection);
            System.Diagnostics.Debugger.Log(1, "Timing", "\nFizika: " + stw.ElapsedMilliseconds);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(85), aspect_ratio, 0.1f, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
            base.OnResize(e);
        }
        int ups;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            DebugMove(Keyboard, ref p.Player.DebugLocation);
            p.Update((float)e.Time, Keyboard);
                
            if (Keyboard[OpenTK.Input.Key.Escape] || Keyboard[OpenTK.Input.Key.Q]) Exit();
            ups++;
            base.OnUpdateFrame(e);
        }

        public void DebugMove(OpenTK.Input.KeyboardDevice k, ref Vector3 n)
        {
            if (k[OpenTK.Input.Key.Keypad8]) n.Z += 0.001f;
            if (k[OpenTK.Input.Key.Keypad2]) n.Z -= 0.001f;
            if (k[OpenTK.Input.Key.Keypad4]) n.X += 0.001f;
            if (k[OpenTK.Input.Key.Keypad6]) n.X -= 0.001f;
            if (k[OpenTK.Input.Key.Keypad7]) n.Y += 0.001f;
            if (k[OpenTK.Input.Key.Keypad1]) n.Y -= 0.001f;
        }
        
        OpenTK.Vector3 position = new Vector3(5, 8, 10);
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //renderer.Clear(Color.Black);
            //renderer.DrawString("Hello, world", serif, Brushes.Blue, new PointF(1.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );

            frameTime += (float)e.Time;
            fps++;
            if (frameTime >= 1)
            {
                frameTime = 0;
                Title = "Driving simulator, FPS = " + fps.ToString()+" UPS="+ups.ToString();
                fps = 0;
                ups = 0;
            }
            Matrix4 lookat = camera.GenerateLookAt((Vehicle)p.Player);
            GL.MatrixMode(MatrixMode.Modelview);
            if (this.Focused) camera.Update(Mouse, Height / 2, Width / 2);
            GL.LoadMatrix(ref lookat);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.Green);
            GL.Vertex3(new Vector3(-5000f, 0, -5000f));
            GL.Vertex3(new Vector3(-5000f, 0, 5000f));
            GL.Vertex3(new Vector3(5000f, 0, 5000f));
            GL.Vertex3(new Vector3(5000f, 0, -5000f));
            GL.End();
            currentMap.Draw(ref MeshCollection, lookat);
          
            p.Player.Draw(lookat, ref MeshCollection);
            for (int i = 0; i < p.Vehicles.Length; i++)
            {
                p.Vehicles[i].Draw(lookat, ref MeshCollection);
            }

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            p.ExitPhysics();
            base.OnUnload(e);
        }
    }
}
