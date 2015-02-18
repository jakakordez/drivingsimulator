using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BulletSharp;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Slovenia_simulator
{
    public delegate RigidBody RigidBodyCreation(float mass, Matrix4 startTransform, CollisionShape shape);
    class World
    {
        public DiscreteDynamicsWorld DynamicsWorld { get; set; }
        CollisionDispatcher dispatcher;
        DbvtBroadphase broadphase;
        CollisionConfiguration collisionConf;
        public Vehicle[] Vehicles = new Vehicle[0];
        public Vehicle Player;
        Map CurrentMap;
        MeshCollector MeshCollection;
        int grass;
        Camera camera;
        List<RigidBody> ball = new List<RigidBody>();
        public World()
        {
            MeshCollection = new MeshCollector();
            

            // collision configuration contains default setup for memory, collision setup
            collisionConf = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConf);

            broadphase = new DbvtBroadphase();
            DynamicsWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, null, collisionConf);
            DynamicsWorld.Gravity = new Vector3(0, -10, 0);
            CurrentMap = new Map("Mapa", ref MeshCollection, new RigidBodyCreation(LocalCreateRigidBody));
            grass = Misc.LoadTexture("data/maps/Mapa/textures/grass.png", 1);
            //LocalCreateRigidBody(0,  Matrix4.CreateTranslation(-50*Vector3.UnitY), new BoxShape(5000, 50, 5000));
            addCar("BMW/X5", Matrix4.CreateRotationY(MathHelper.Pi * 0.5f) * Matrix4.CreateTranslation(new Vector3(0, 1, 0)), VehicleController.Player, ref MeshCollection);//712
            /*for (int i = 0; i < 1; i++)
            {
                addCar("BMW/M3-E92", Matrix4.CreateTranslation(new Vector3(10, 1, i*10)), VehicleController.AI, ref MeshCollection);
            }*/
            DynamicsWorld.DebugDrawer = new DebugDrawer() { DebugMode = DebugDrawModes.DrawWireframe };
            camera = new Camera();
        }

        void Addball()
        {
            SphereShape s = new SphereShape(0.5f);
            RigidBody r = LocalCreateRigidBody(5, Matrix4.CreateTranslation(new Vector3(0, 5, 0)), s);
            ball.Add(r);
        }
        bool e;
        public void Update(float elaspedTime, OpenTK.Input.KeyboardDevice k)
        {
            if (k[OpenTK.Input.Key.Enter] && !e) Addball();
            e = k[OpenTK.Input.Key.Enter];
            DynamicsWorld.StepSimulation(elaspedTime);
            Player.Update(elaspedTime, new Controller(k), CurrentMap, Player);
            for (int i = 0; i < Vehicles.Length; i++)
            {
                Vehicles[i].Update(elaspedTime, null, CurrentMap, Player);
            }
        }

        public void Draw(bool Focused, OpenTK.Input.MouseDevice mouse)
        {
            Matrix4 WorldMatrix = camera.GenerateLookAt((Vehicle)Player);
            GL.MatrixMode(MatrixMode.Modelview);
            if (Focused) camera.Update(mouse);

            /*GL.LoadMatrix(ref lookat);
            Matrix4 t = CurrentMap.CurrentTerrain.ground.CenterOfMassTransform * lookat;
            GL.LoadMatrix(ref t);*/
            /*GL.Color4(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, grass);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            float d = 64f;//5000f;
            GL.Vertex3(new Vector3(-d, 0, -d));
            GL.TexCoord2(new Vector2(0, 10000));
            GL.Vertex3(new Vector3(-d, 0, d));
            GL.TexCoord2(new Vector2(10000, 10000));
            GL.Vertex3(new Vector3(d, 0, d));
            GL.TexCoord2(new Vector2(10000, 0));
            GL.Vertex3(new Vector3(d, 0, -d));
            GL.End();*/
            //GL.LoadMatrix(ref WorldMatrix);
            
            /*GL.Begin(PrimitiveType.Lines);
            GL.Color4(OpenTK.Graphics.Color4.Red);
            DynamicsWorld.DebugDrawObject(Matrix4.Identity, CurrentMap.CurrentTerrain.ground.CollisionShape, OpenTK.Graphics.Color4.Red);
            GL.End();*/
            CurrentMap.Draw(ref MeshCollection, WorldMatrix, Player.body.CenterOfMassPosition);
            GL.Begin(PrimitiveType.Triangles);
            GL.Color4(OpenTK.Graphics.Color4.Red);
            for (int i = 0; i < ball.Count; i++)
            {
                Vector3 pos = ball[i].CenterOfMassPosition;
                GL.Vertex3(pos + new Vector3(0, 0.5f, 0));
                GL.Vertex3(pos + new Vector3(0, 0.5f, 0.5f));
                GL.Vertex3(pos + new Vector3(0, -0.5f, 0));

                GL.Vertex3(pos + new Vector3(0.5f, 0.5f, 0));
                GL.Vertex3(pos + new Vector3(0, 0.5f, 0.5f));
                GL.Vertex3(pos + new Vector3(0, 0.5f, 0));
            }
            GL.End();
            
            Player.Draw(WorldMatrix, ref MeshCollection);
            for (int i = 0; i < Vehicles.Length; i++)
            {
                Vehicles[i].Draw(WorldMatrix, ref MeshCollection);
            }
            GL.LoadMatrix(ref WorldMatrix);
        }

        public void addCar(string path, Matrix4 startTransform, VehicleController controller, ref MeshCollector meshCollection)
        {
            Vehicles.Car a = new Vehicles.Car(path, 4, ref meshCollection, controller, (controller == VehicleController.Player)?PlayerView.Cabin:PlayerView.Exterior);
            a.body = LocalCreateRigidBody(a.Mass, startTransform, a.collisionShape);
            a.Init(new DefaultVehicleRaycaster(DynamicsWorld));
            DynamicsWorld.AddAction(a.raycastVehicle);
            if (controller == VehicleController.Player && Player == null) Player = a;
            else Misc.Push<Vehicle>(a, ref Vehicles);
        }

        public RigidBody LocalCreateRigidBody(float mass, Matrix4 startTransform, CollisionShape shape)
        {
            bool isDynamic = (mass != 0.0f);

            Vector3 localInertia = Vector3.Zero;
            if (isDynamic)
                shape.CalculateLocalInertia(mass, out localInertia);

            DefaultMotionState myMotionState = new DefaultMotionState(startTransform);

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, myMotionState, shape, localInertia);
            RigidBody body = new RigidBody(rbInfo);
            DynamicsWorld.AddRigidBody(body);
            return body;
        }
        public void ExitPhysics()
        {
            //remove/dispose constraints
            int i;
            for (i = DynamicsWorld.NumConstraints - 1; i >= 0; i--)
            {
                TypedConstraint constraint = DynamicsWorld.GetConstraint(i);
                DynamicsWorld.RemoveConstraint(constraint);
                constraint.Dispose();
            }

            //remove the rigidbodies from the dynamics world and delete them
            for (i = DynamicsWorld.NumCollisionObjects - 1; i >= 0; i--)
            {
                CollisionObject obj = DynamicsWorld.CollisionObjectArray[i];
                RigidBody body = obj as RigidBody;
                if (body != null && body.MotionState != null)
                {
                    body.MotionState.Dispose();
                }
                DynamicsWorld.RemoveCollisionObject(obj);
                obj.Dispose();
            }

            DynamicsWorld.Dispose();
            broadphase.Dispose();
            if (dispatcher != null) dispatcher.Dispose();
            collisionConf.Dispose();
        }
    }
}
