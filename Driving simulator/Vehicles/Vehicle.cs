using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using BulletSharp;

namespace Driving_simulator
{
    public enum PlayerView { Exterior, Cabin, Camera}
    class Vehicle
    {
        public PlayerView viewMode = PlayerView.Exterior;
        public float Mass, maxEngineForce, maxBrakeForce, width, height, length;
        public float engineForce, brakeForce, steeringValue;
        public CollisionShape collisionShape;
        public RigidBody body;
        public int bodyMesh, cabinMesh;
        public Vector3 driverEye, exteriorEye;
        public Vector2 driverAngle;

        public const int rightIndex = 0;
        public const int upIndex = 1;
        public const int forwardIndex = 2;
        public Vector3 wheelDirectionCS0 = new Vector3(0, -1, 0);
        public Vector3 wheelAxleCS = new Vector3(-1, 0, 0);

        public float FrameDelta = 0;

        public float steeringIncrement, steeringClamp;

        public RaycastVehicle raycastVehicle;

        public void Init() { }

        public void Update(float elaspedTime, OpenTK.Input.KeyboardDevice k) { }
        public static Vector3 ParseVector(string value)
        {
            string[] values = value.Split(',');
            Vector3 result = new Vector3(Misc.toFloat(values[0]), 0, 0);
            if (values.Length == 2) result.Z = Misc.toFloat(values[1]);
            else if(values.Length == 3)
            {
                result.Y = Misc.toFloat(values[1]);
                result.Z = Misc.toFloat(values[2]);
            }
            return result;
        }

        public virtual void Draw(Matrix4 LookAt, ref MeshCollector Meshes) { }
    }
    
}
