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
        public World()
        {
            MeshCollection = new MeshCollector();
            CurrentMap = new Map("Mapa", ref MeshCollection);
            grass = Misc.LoadTexture("data/maps/Mapa/textures/grass.png", 1);

            // collision configuration contains default setup for memory, collision setup
            collisionConf = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConf);

            broadphase = new DbvtBroadphase();
            DynamicsWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, null, collisionConf);
            DynamicsWorld.Gravity = new Vector3(0, -10, 0);

            LocalCreateRigidBody(0,  Matrix4.CreateTranslation(-50*Vector3.UnitY), new BoxShape(5000, 50, 5000));
            addCar("BMW/M3-E92", Matrix4.CreateRotationX(MathHelper.Pi*0) * Matrix4.CreateTranslation(new Vector3(10, 10, 20)), VehicleController.Player, ref MeshCollection);//
            for (int i = 0; i < 1; i++)
            {
                addCar("BMW/M3-E92", Matrix4.CreateTranslation(new Vector3(10, 1, i*10)), VehicleController.AI, ref MeshCollection);
            }
            //a = System.IO.File.OpenRead("data/maps/map1/h.raw");
            //HeightfieldTerrainShape t = new HeightfieldTerrainShape(128, 128, a, 1, -100, 100, 1, PhyScalarType.PhyFloat, false);
            //LocalCreateRigidBody(0, Matrix4.CreateTranslation(new Vector3(-64, -20, -64)), t);
            //LocalCreateRigidBody(0, Matrix4.CreateTranslation(new Vector3(15, 0, 0)), new BoxShape(5, 5, 5));
            //map = new Maps.Map();
            //RigidBody ground = LocalCreateRigidBody(0, map.tr, map.groundShape);
            //ground.UserObject = "Ground";
        }

        public void Update(float elaspedTime, OpenTK.Input.KeyboardDevice k)
        {
            DynamicsWorld.StepSimulation(elaspedTime);
            Player.Update(elaspedTime, new Controller(k), CurrentMap, Player);
            for (int i = 0; i < Vehicles.Length; i++)
            {
                Vehicles[i].Update(elaspedTime, null, CurrentMap, Player);
            }
        }

        public void Draw(Matrix4 lookat)
        {
            GL.LoadMatrix(ref lookat);

            GL.Color4(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, grass);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex3(new Vector3(-5000f, 0, -5000f));
            GL.TexCoord2(new Vector2(0, 10000));
            GL.Vertex3(new Vector3(-5000f, 0, 5000f));
            GL.TexCoord2(new Vector2(10000, 10000));
            GL.Vertex3(new Vector3(5000f, 0, 5000f));
            GL.TexCoord2(new Vector2(10000, 0));
            GL.Vertex3(new Vector3(5000f, 0, -5000f));
            GL.End();
            Player.Draw(lookat, ref MeshCollection);
            for (int i = 0; i < Vehicles.Length; i++)
            {
                Vehicles[i].Draw(lookat, ref MeshCollection);
            }
            CurrentMap.Draw(ref MeshCollection, lookat, Player.body.CenterOfMassPosition);
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
