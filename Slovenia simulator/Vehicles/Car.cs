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
        
        float wheelRadius, wheelWidth, wheelFriction, suspensionStiffness, suspensionDamping, suspensionCompression, rollInfluence, connectionHeight;
        float gear = 1;
        Color4[] colors;
        float suspensionRestLength;
        Vector3 steeringWheelOrigin, frontWheel, rearWheel;
        int wheelMesh, steeringWheelMesh;
        public Car(string path, int color, ref MeshCollector meshCollection):base()
        {
            driverEye = new Vector3();
            exteriorEye = new Vector3();
            string[] file = File.ReadAllLines("data/vehicles/car/" + path + "/data.conf");
            for (int i = 0; i < file.Length; i++)
            {
                if(file[i] != "" && file[i][0] != '#' && file[i].Contains('=')){
                    string[] line = file[i].Replace(" ", "").Split('=');
                    switch (line[0])
                    {
                        case "Mass": Misc.parseFloat(line[1], out Mass); break;
                        case "length": Misc.parseFloat(line[1], out length); break;
                        case "height": Misc.parseFloat(line[1], out height); break;
                        case "width": Misc.parseFloat(line[1], out width); break;
                        case "steeringIncrement": Misc.parseFloat(line[1], out steeringIncrement); break;
                        case "steeringClamp": Misc.parseFloat(line[1], out steeringClamp); break;
                        case "maxEngineForce": Misc.parseFloat(line[1], out maxEngineForce); break;
                        case "maxBrakeForce": Misc.parseFloat(line[1], out maxBrakeForce); break;
                        case "frontWheel": frontWheel = Vehicle.ParseVector(line[1]); break;
                        case "rearWheel": rearWheel = Vehicle.ParseVector(line[1]); break;
                        case "driver": driverEye = Vehicle.ParseVector(line[1]); break;
                        case "exterior": exteriorEye = Vehicle.ParseVector(line[1]); break;
                        case "driverAngle": driverAngle = Vehicle.ParseVector(line[1]).Xz; break;
                        case "steeringWheel": steeringWheelOrigin = Vehicle.ParseVector(line[1]); break;
                        case "wheelRadius": Misc.parseFloat(line[1], out wheelRadius); break;
                        case "wheelWidth": Misc.parseFloat(line[1], out wheelWidth ); break;
                        case "wheelFriction": Misc.parseFloat(line[1], out wheelFriction); break;
                        case "suspensionStiffness": Misc.parseFloat(line[1], out suspensionStiffness); break;
                        case "suspensionDamping": Misc.parseFloat(line[1], out suspensionDamping); break;
                        case "suspensionCompression": Misc.parseFloat(line[1], out suspensionCompression); break;
                        case "rollInfluence": Misc.parseFloat(line[1], out rollInfluence); break;
                        case "suspensionRestLength": Misc.parseFloat(line[1], out suspensionRestLength); break;
                        case "connectionHeight": Misc.parseFloat(line[1], out connectionHeight); break;
                        case "color":
                            string[] colorValues = line[1].Split(',');
                            colors = new Color4[colorValues.Length];
                            for (int j = 0; j < colorValues.Length; j++)
                            {
                                colors[j] = new Color4(System.Drawing.ColorTranslator.FromHtml(colorValues[j].Split(':')[1]));
                            }
                            break;
                    }
                }
            }
            bodyMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/body.obj");
            wheelMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/wheel.obj");
            cabinMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/cabin.obj");
            steeringWheelMesh = meshCollection.LoadMesh("data/vehicles/car/" + path + "/steeringwheel.obj");
            /*for (int i = 0; i < bodyMesh.materials.Length; i++)
            {
              if (bodyMesh.materials[i].Name == "bodyColor") bodyMesh.materials[i].Brush = colors[color];
            }
            for (int i = 0; i < cabinMesh.materials.Length; i++)
            {
                if (cabinMesh.materials[i].Name == "bodyColor") cabinMesh.materials[i].Brush = colors[color];
            }*/
            CollisionShape chassisShape = new BoxShape(width / 2, height / 2, length / 2);
            collisionShape = new CompoundShape();

            //localTrans effectively shifts the center of mass with respect to the chassis
            Matrix4 localTrans = Matrix4.CreateTranslation(2*Vector3.UnitY);
            ((CompoundShape)collisionShape).AddChildShape(localTrans, chassisShape);
        }

        new public void Init(VehicleRaycaster vehicleRayCaster)
        {
            // create vehicle
            RaycastVehicle.VehicleTuning tuning = new RaycastVehicle.VehicleTuning();
            raycastVehicle = new RaycastVehicle(tuning, body, vehicleRayCaster);

            body.ActivationState = ActivationState.DisableDeactivation;

           
            bool isFrontWheel = true;
            float CUBE_HALF_EXTENTS = width/2;
            // choose coordinate system
            raycastVehicle.SetCoordinateSystem(rightIndex, upIndex, forwardIndex);

            Vector3 connectionPointCS0 = new Vector3(frontWheel.Z, connectionHeight, frontWheel.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, suspensionRestLength, wheelRadius, tuning, isFrontWheel);

            connectionPointCS0 = new Vector3(-frontWheel.Z, connectionHeight, frontWheel.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, suspensionRestLength, wheelRadius, tuning, isFrontWheel);

            isFrontWheel = false;
            connectionPointCS0 = new Vector3(-rearWheel.Z, connectionHeight, -rearWheel.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, suspensionRestLength, wheelRadius, tuning, isFrontWheel);

            connectionPointCS0 = new Vector3(rearWheel.Z, connectionHeight, -rearWheel.X);
            raycastVehicle.AddWheel(connectionPointCS0, wheelDirectionCS0, wheelAxleCS, suspensionRestLength, wheelRadius, tuning, isFrontWheel);

            for (int i = 0; i < raycastVehicle.NumWheels; i++)
            {
                WheelInfo wheel = raycastVehicle.GetWheelInfo(i);
                wheel.SuspensionStiffness = suspensionStiffness;
                wheel.WheelsDampingRelaxation = suspensionDamping;
                wheel.WheelsDampingCompression = suspensionCompression;
                wheel.FrictionSlip = wheelFriction;
                wheel.RollInfluence = rollInfluence;
            }
        }
        public override void Draw(Matrix4 LookAt, ref MeshCollector Meshes)
        {
            base.Draw(LookAt, ref Meshes);

            Matrix4 modelLookAt = raycastVehicle.ChassisWorldTransform * LookAt;
            GL.LoadMatrix(ref modelLookAt);
            switch (viewMode)
            {
                case PlayerView.Exterior:case PlayerView.Camera:
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
                    Matrix4 steering =  Matrix4.CreateRotationZ(-8*steeringValue)*Matrix4.CreateRotationX(0.4f) * Matrix4.CreateTranslation(steeringWheelOrigin) * modelLookAt;
                    GL.LoadMatrix(ref steering);
                    Meshes.DrawMesh(steeringWheelMesh);
                    break;
                default:
                    break;
            }
            
        }

        public override void Update(float elaspedTime, OpenTK.Input.KeyboardDevice k)
        {
            if(k != null) manageKeyboard(k);

            raycastVehicle.ApplyEngineForce(engineForce*gear, 2);
            raycastVehicle.SetBrake(brakeForce, 2);
            raycastVehicle.ApplyEngineForce(engineForce*gear, 3);
            raycastVehicle.SetBrake(brakeForce, 3);

            raycastVehicle.SetSteeringValue(steeringValue, 0);
            raycastVehicle.SetSteeringValue(steeringValue, 1);
        }

        private void manageKeyboard(OpenTK.Input.KeyboardDevice k)
        {
            float maxSteering = steeringClamp*(1-(raycastVehicle.CurrentSpeedKmHour/180));
            if (k[OpenTK.Input.Key.Delete]) System.Diagnostics.Debugger.Break();
            if (!k[OpenTK.Input.Key.A]&&!k[OpenTK.Input.Key.D]&&steeringValue > -steeringIncrement * 2 && steeringValue < steeringIncrement * 2) steeringValue = 0;
            if (k[OpenTK.Input.Key.A])
            {
                steeringValue += steeringIncrement;
                if (steeringValue > maxSteering) steeringValue = maxSteering;
            }
            else if (steeringValue > 3 * steeringIncrement) steeringValue -= 3 * steeringIncrement;

            if (k[OpenTK.Input.Key.D])
            {
                steeringValue -= steeringIncrement;
                if (steeringValue < -maxSteering)
                    steeringValue = -maxSteering;
            }
            else if (steeringValue < 3 * -steeringIncrement) steeringValue += 3 * steeringIncrement;

            

            if (k[OpenTK.Input.Key.W] && engineForce < maxEngineForce) engineForce += 50f;
            else if (engineForce > 0) engineForce -= 25f;

            if (k[OpenTK.Input.Key.S] && brakeForce < maxBrakeForce)
            {
                if (engineForce > 0) engineForce -= 50;
                brakeForce += 200f;
            }
            else if (brakeForce > 0) brakeForce -= 1f;
            if (k[OpenTK.Input.Key.F] && System.Math.Abs(raycastVehicle.CurrentSpeedKmHour) < 1f) gear = 1;
            if (k[OpenTK.Input.Key.R] && System.Math.Abs(raycastVehicle.CurrentSpeedKmHour) < 1f) gear = -0.5f;

            if (k[OpenTK.Input.Key.Number1]) viewMode = PlayerView.Cabin;
            if (k[OpenTK.Input.Key.Number2]) viewMode = PlayerView.Exterior;
            if (k[OpenTK.Input.Key.Number3]) viewMode = PlayerView.Camera;
        }
    }
}
