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
using QuickFont;

namespace Slovenia_simulator
{
    class Game:GameWindow
    {
        int frames, fps;
        float frameTime;
        Camera camera;
        World world;
        Font serif = new Font(FontFamily.GenericSansSerif, 24);
        QFont myFont;
        public Game()
            : base(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, new GraphicsMode(), "Driving simulator")
        {
            WindowState = OpenTK.WindowState.Fullscreen;
            VSync = VSyncMode.On;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            myFont = new QFont(serif);
            myFont.Options.CharacterSpacing = 0.2f;
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthClamp);
            GL.ClearColor(Color.CornflowerBlue);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Light0);

            //GL.Enable(EnableCap.Lighting);
            System.Windows.Forms.Cursor.Hide();
            camera = new Camera(this.Height, this.Width);
            
            world = new World();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(85), aspect_ratio, 0.01f, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
            base.OnResize(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[OpenTK.Input.Key.Delete]) System.Diagnostics.Debugger.Break();
            DebugMove(Keyboard, ref world.Player.DebugLocation);
            world.Update((float)e.Time, Keyboard);
                
            if (Keyboard[OpenTK.Input.Key.Escape] || Keyboard[OpenTK.Input.Key.Q]) Exit();
            base.OnUpdateFrame(e);
        }

        public void DebugMove(OpenTK.Input.KeyboardDevice k, ref Vector3 n)
        {
            if (k[OpenTK.Input.Key.Number0]) world.Player.viewMode = PlayerView.Debug;
            if (k[OpenTK.Input.Key.Keypad8]) n.Z += 0.005f;
            if (k[OpenTK.Input.Key.Keypad2]) n.Z -= 0.005f;
            if (k[OpenTK.Input.Key.Keypad4]) n.X += 0.005f;
            if (k[OpenTK.Input.Key.Keypad6]) n.X -= 0.005f;
            if (k[OpenTK.Input.Key.Keypad7]) n.Y += 0.005f;
            if (k[OpenTK.Input.Key.Keypad1]) n.Y -= 0.005f;
        }
        
        OpenTK.Vector3 position = new Vector3(5, 8, 10);
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            QFont.Begin();
            myFont.Print("FPS: " + fps, new Vector2(20, 20));
            myFont.Print("Speed: " + Math.Round(world.Player.raycastVehicle.CurrentSpeedKmHour, 1)+" kph", new Vector2(20, 50));
            QFont.End();
            
            frameTime += (float)e.Time;
            frames++;
            if (frameTime >= 1)
            {
                frameTime = 0;
                fps = frames;
                frames = 0;
            }
            
            Matrix4 lookat = camera.GenerateLookAt((Vehicle)world.Player);
            GL.MatrixMode(MatrixMode.Modelview);
            if (this.Focused) camera.Update(Mouse, Height / 2, Width / 2);
            world.Draw(lookat);
            
            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            world.ExitPhysics();
            base.OnUnload(e);
        }
    }
}
