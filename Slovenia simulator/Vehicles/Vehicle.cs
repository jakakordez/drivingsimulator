using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using BulletSharp;
using Slovenia_simulator.Vehicles;

namespace Slovenia_simulator
{
    public enum PlayerView { Exterior, Cabin, Camera, Debug}
    class Vehicle
    {
        public PlayerView viewMode = PlayerView.Cabin;
        public Vector3 DebugLocation;
        public float Mass, MaxEngineForce, MaxBrakeForce, MaximumSpeed;
        public float engineForce, brakeForce, steeringValue;
        public CollisionShape collisionShape;
        public RigidBody body;
        public int bodyMesh, cabinMesh;
        public Vector3 Dimensions, DriverEyeLocation, ExteriorEyeLocation;
        public Vector2 DriverViewAngle;

        public const int rightIndex = 0;
        public const int upIndex = 1;
        public const int forwardIndex = 2;
        public Vector3 wheelDirectionCS0 = new Vector3(0, -1, 0);
        public Vector3 wheelAxleCS = new Vector3(-1, 0, 0);

        public float FrameDelta = 0;

        public float SteeringIncrement, SteeringClamp;

        public RaycastVehicle raycastVehicle;

        public void Init() { }

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

        public virtual void Update(float elaspedTime, OpenTK.Input.KeyboardDevice k) { }
    }
    
}
