using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using BulletSharp;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;

namespace Slovenia_simulator.Vehicles
{
    class Car:Vehicle
    {

        public float WheelRadius, WheelWidth, WheelFriction, SuspensionStiffness, SuspensionDamping, SuspensionCompression, RollInfluence, SuspensionHeight, SuspensionRestLength;
        float gear = 1, CruiseControl = 0;
        Color4[] colors;
        public Vector3 SteeringWheelLocation, NeedleLocation, NeedleAngle;
        public Vector2 SteeringWheelAngle, FrontWheelLocation, RearWheelLocation;
        int wheelMesh, steeringWheelMesh, needleMesh;
        int road = 10, point = 0;
        bool EngineRunning;

        public Car(string path, int color, ref MeshCollector meshCollection, VehicleController controller, PlayerView view):base(controller, view)
        {
            DriverEyeLocation = new Vector3();
            DataParser.ParseData("data/vehicles/car/" + path + "/config.json", this);
           
            bodyMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/body.mesh");
            wheelMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/wheel.mesh");
            cabinMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/cabin.mesh");
            steeringWheelMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/steeringwheel.mesh");
            needleMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/needle.mesh");
            /*for (int i = 0; i < bodyMesh.materials.Length; i++)
            {
              if (bodyMesh.materials[i].Name == "bodyColor") bodyMesh.materials[i].Brush = colors[color];
            }
            for (int i = 0; i < cabinMesh.materials.Length; i++)
            {
                if (cabinMesh.materials[i].Name == "bodyColor") cabinMesh.materials[i].Brush = colors[color];
            }*/
            CollisionShape chassisShape = new BoxShape(Dimensions.Y / 2, Dimensions.Z / 2, Dimensions.X / 2);
            collisionShape = new CompoundShape();
            //localTrans effectively shifts the center of mass with respect to the chassis
            Matrix4 localTrans = Matrix4.CreateTranslation(0*Vector3.UnitY);
            ((CompoundShape)collisionShape).AddChildShape(localTrans, chassisShape);
        }

        public void Init(VehicleRaycaster vehicleRayCaster)
        {
            // create vehicle
            RaycastVehicle.VehicleTuning tuning = new RaycastVehicle.VehicleTuning();
            raycastVehicle = new RaycastVehicle(tuning, body, vehicleRayCaster);

            body.ActivationState = ActivationState.DisableDeactivation;

           
            bool isFrontWheel = true;
            float CUBE_HALF_EXTENTS = Dimensions.Y/2;
            // choose coordinate system
            raycastVehicle.SetCoordinateSystem(rightIndex, upIndex, forwardIndex);

            Vector3 connectionPointCS0 = new Vector3(FrontWheelLocation.Y, SuspensionHeight, FrontWheelLocation.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, SuspensionRestLength, WheelRadius, tuning, isFrontWheel);

            connectionPointCS0 = new Vector3(-FrontWheelLocation.Y, SuspensionHeight, FrontWheelLocation.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, SuspensionRestLength, WheelRadius, tuning, isFrontWheel);

            isFrontWheel = false;
            connectionPointCS0 = new Vector3(-RearWheelLocation.Y, SuspensionHeight, -RearWheelLocation.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, SuspensionRestLength, WheelRadius, tuning, isFrontWheel);

            connectionPointCS0 = new Vector3(RearWheelLocation.Y, SuspensionHeight, -RearWheelLocation.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, SuspensionRestLength, WheelRadius, tuning, isFrontWheel);

            for (int i = 0; i < raycastVehicle.NumWheels; i++)
            {
                WheelInfo wheel = raycastVehicle.GetWheelInfo(i);
                wheel.SuspensionStiffness = SuspensionStiffness;
                wheel.WheelsDampingRelaxation = SuspensionDamping;
                wheel.WheelsDampingCompression = SuspensionCompression;
                wheel.FrictionSlip = WheelFriction;
                wheel.RollInfluence = RollInfluence;
            }
        }
        public override void Draw(Matrix4 LookAt, ref MeshCollector Meshes)
        {
            base.Draw(LookAt, ref Meshes);

            Matrix4 modelLookAt = raycastVehicle.ChassisWorldTransform * LookAt;
            GL.LoadMatrix(ref modelLookAt);
            switch (viewMode)
            {
                case PlayerView.Exterior:
                case PlayerView.Camera:
                case PlayerView.Debug:
                    Meshes.DrawMesh(bodyMesh);
                    Matrix4 rotation = Matrix4.CreateRotationX(raycastVehicle.GetWheelInfo(1).Rotation);
                    Matrix4 wheel = Matrix4.CreateRotationY(-(float)MathHelper.PiOver2)*rotation;
                    wheel = raycastVehicle.GetWheelTransformWS(0)*LookAt;
                    GL.LoadMatrix(ref wheel);
                    Meshes.DrawMesh(wheelMesh);
                    wheel = Matrix4.CreateRotationY((float)MathHelper.Pi);
                    wheel *= raycastVehicle.GetWheelTransformWS(1)*LookAt;
                    GL.LoadMatrix(ref wheel);
                    Meshes.DrawMesh(wheelMesh);
                    wheel = Matrix4.CreateRotationY((float)MathHelper.Pi);
                    wheel *= raycastVehicle.GetWheelTransformWS(2)*LookAt;
                    GL.LoadMatrix(ref wheel);
                    Meshes.DrawMesh(wheelMesh);
                    wheel = raycastVehicle.GetWheelTransformWS(3)*LookAt;
                    GL.LoadMatrix(ref wheel);
                    Meshes.DrawMesh(wheelMesh);
                    break;
                
                case PlayerView.Cabin:
                    Meshes.DrawMesh(cabinMesh);
                    Matrix4 steering = Matrix4.CreateRotationZ(SteeringWheelAngle.X * steeringValue) * Matrix4.CreateRotationX(SteeringWheelAngle.Y) * Matrix4.CreateTranslation(SteeringWheelLocation) * modelLookAt;
                    GL.LoadMatrix(ref steering);
                    Meshes.DrawMesh(steeringWheelMesh);
                    steering = Matrix4.CreateRotationZ(NeedleAngle.X + (raycastVehicle.CurrentSpeedKmHour / NeedleAngle.Y)) * Matrix4.CreateRotationX(NeedleAngle.Z) * Matrix4.CreateTranslation(NeedleLocation) * modelLookAt;
                    GL.LoadMatrix(ref steering);
                    Meshes.DrawMesh(needleMesh);
                    break;
            }
        }

        public override void Update(float elaspedTime, Controller k, Map currentMap, Vehicle target)
        {
            base.Update(elaspedTime, k, currentMap, target);

            raycastVehicle.ApplyEngineForce(engineForce*gear, 2);
            raycastVehicle.SetBrake(brakeForce, 2);
            raycastVehicle.ApplyEngineForce(engineForce*gear, 3);
            raycastVehicle.SetBrake(brakeForce, 3);
            raycastVehicle.SetBrake(brakeForce, 0);
            raycastVehicle.SetBrake(brakeForce, 1);

            raycastVehicle.SetSteeringValue(steeringValue, 0);
            raycastVehicle.SetSteeringValue(steeringValue, 1);
        }
        public override void HandleInput(Controller k)
        {
            
            float maxSteering = SteeringClamp * (1 - (raycastVehicle.CurrentSpeedKmHour / MaximumSpeed));
            float incSteering = SteeringIncrement * (1 - (raycastVehicle.CurrentSpeedKmHour / MaximumSpeed));
            if (k.Brake || k.Accelerate) CruiseControl = 0;
            else if (k.CruiseControl && !prevState.CruiseControl)
            {
                if (CruiseControl == 0) CruiseControl = 10;
                else CruiseControl = 0;
            }
            if (CruiseControl > 0 && k.CControlInc && !prevState.CControlInc) CruiseControl += 10;
            if (CruiseControl > 0 && k.CControlDec && !prevState.CControlDec) CruiseControl -= 10;
            if (!k.Left && !k.Right && steeringValue > -SteeringIncrement * 3 && steeringValue < SteeringIncrement * 3) steeringValue = 0;
            if (k.Left)
            {
                steeringValue += incSteering;
                if (steeringValue > maxSteering) steeringValue = maxSteering;
            }
            else if (steeringValue > 3 * SteeringIncrement) steeringValue -= 3 * SteeringIncrement;

            if (k.Right)
            {
                steeringValue -= incSteering;
                if (steeringValue < -maxSteering)
                    steeringValue = -maxSteering;
            }
            else if (steeringValue < 3 * -SteeringIncrement) steeringValue += 3 * SteeringIncrement;

            if (CruiseControl > 0 && raycastVehicle.CurrentSpeedKmHour < CruiseControl) k.Accelerate = true;

            if (k.Accelerate && engineForce < MaxEngineForce) engineForce += 50f;
            else if (engineForce > 0) engineForce*=0.5f;

            if (k.Brake && brakeForce < MaxBrakeForce)
            {
                engineForce *= 0.5f;
                brakeForce = MaxBrakeForce;
            }
            else brakeForce = 0;
            if (k.EngineStart && !prevState.EngineStart) EngineRunning = !EngineRunning;
            if (!EngineRunning) gear = 0;
            if (k.Forward && System.Math.Abs(raycastVehicle.CurrentSpeedKmHour) < 1f) gear = 1;
            if (k.Reverse && System.Math.Abs(raycastVehicle.CurrentSpeedKmHour) < 1f) gear = -0.5f;

            if (k.CabinView) viewMode = PlayerView.Cabin;
            if (k.ExteriorView) viewMode = PlayerView.Exterior;

            prevState = k;
        }
        public override Controller HandleAI(Map CurrentMap, Vehicle tar)
        {
            Controller result = new Slovenia_simulator.Controller();
            //result.CruiseControl = true;
            CruiseControl = 20;
            //result.Brake = true;
            Vector2 pos = body.CenterOfMassPosition.Xz;

            Vector2 target = CurrentMap.Roads[road].RPaths[0].Points[point].Xz;
            //Vector2 target = tar.body.CenterOfMassPosition.Xz;

            float angle = Misc.getVectorAngle(pos-target)+MathHelper.PiOver2;
            angle = Misc.normalizeAngle(angle);
            float curAngle = ((float)Math.Asin(raycastVehicle.ChassisWorldTransform.ExtractRotation().Y) * -2);
            curAngle = Misc.normalizeAngle(curAngle);
            curAngle = Misc.normalizeAngle(angle - curAngle);
            if (curAngle >Math.PI && curAngle < MathHelper.TwoPi-0.01f) result.Left = true;
            else if(curAngle > 0.01f) result.Right = true;
            float dis = (pos - target).Length;
            if (dis < Dimensions.X/2 && CurrentMap.Roads[road].RPaths[0].Points.Length - 1 > point) point++;
           
            return result;
        }
    }
}
