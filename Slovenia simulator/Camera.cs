using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Slovenia_simulator
{
    class Camera
    {
        public Camera(int h, int w){
            H = h;
            W = w;
        }
        public float x, y;
        int H, W;

        public void Update(OpenTK.Input.MouseDevice mouse, int h, int w)
        {
            x = (float)mouse.X/W;
            y = (float)mouse.Y/H;
        }

        public Matrix4 GenerateLookAt(Vehicle vehicle)
        {
            Matrix4 e = new Matrix4(), t = new Matrix4();
            switch (vehicle.viewMode)
            {
                case PlayerView.Cabin:
                    e = Matrix4.CreateTranslation(vehicle.DriverEyeLocation);
                    t = Matrix4.CreateTranslation(vehicle.DriverEyeLocation + new Vector3(0, 0, 2)) * Matrix4.CreateRotationX((y * vehicle.DriverViewAngle.X) - (vehicle.DriverViewAngle.X / 2)) * Matrix4.CreateRotationY((-x * vehicle.DriverViewAngle.Y) + (vehicle.DriverViewAngle.Y / 2));
                    break;
                case PlayerView.Exterior:
                    e = Matrix4.CreateTranslation(vehicle.ExteriorEyeLocation);
                    t = Matrix4.CreateTranslation(new Vector3(0, 1.2f, -2));
                    break;
                case PlayerView.Camera:
                    return Matrix4.LookAt(new Vector3(4, 0.1f, 2), vehicle.body.CenterOfMassPosition, Vector3.UnitY);
                case PlayerView.Debug:
                     e = Matrix4.CreateTranslation(vehicle.DebugLocation);
                    t = Matrix4.CreateTranslation(vehicle.DebugLocation + new Vector3(0, 0, 2)) * Matrix4.CreateRotationX((y*MathHelper.Pi)-(MathHelper.Pi/2)) * Matrix4.CreateRotationY((-x*MathHelper.Pi)+(MathHelper.Pi/2));
                    break;
                case PlayerView.Rear:
                    e = Matrix4.CreateTranslation(vehicle.ExteriorEyeLocation*new Vector3(-1, 1, -1.5f));
                    t = Matrix4.CreateTranslation(new Vector3(0, 1.2f, -2));
                    break;
            }
            e *= Matrix4.CreateFromQuaternion(vehicle.raycastVehicle.RigidBody.Orientation);
            t *= Matrix4.CreateFromQuaternion(vehicle.raycastVehicle.RigidBody.Orientation);
            e *= Matrix4.CreateTranslation(vehicle.body.CenterOfMassPosition);
            t *= Matrix4.CreateTranslation(vehicle.body.CenterOfMassPosition);
            return Matrix4.LookAt(e.ExtractTranslation(), t.ExtractTranslation(), Vector3.UnitY);
        }
    }
}
