using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Slovenia_simulator
{
    class DebugDrawer:DebugDraw
    {
        
        public override DebugDrawModes DebugMode
        {
            get;
            set;
        }
        public override void DrawLine(ref Vector3 from, ref Vector3 to, OpenTK.Graphics.Color4 color)
        {
           
            GL.Vertex3(from);
            GL.Vertex3(to);

        }
        public override void Draw3dText(ref Vector3 location, string textString)
        {
            throw new NotImplementedException();
        }
        public override void DrawContactPoint(ref Vector3 pointOnB, ref Vector3 normalOnB, float distance, int lifeTime, OpenTK.Graphics.Color4 color)
        {
            throw new NotImplementedException();
        }

        public override void ReportErrorWarning(string warningString)
        {
            throw new NotImplementedException();
        }
    }
}
