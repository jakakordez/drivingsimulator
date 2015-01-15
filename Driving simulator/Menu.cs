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

namespace Driving_simulator
{
    class Menu:GameWindow
    {
        TextRenderer renderer;
        public Menu()
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

            renderer = new TextRenderer(100, 50);
            System.Windows.Forms.Cursor.Hide();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), aspect_ratio, 0.1f, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            if (Keyboard[OpenTK.Input.Key.Escape] || Keyboard[OpenTK.Input.Key.Q]) Exit();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //renderer.Clear(Color.Black);
            //renderer.DrawString("Hello, world", serif, Brushes.Blue, new PointF(1.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SwapBuffers();
        }
    }
}
