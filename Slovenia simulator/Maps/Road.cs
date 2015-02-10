using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Slovenia_simulator.Maps
{
    class Road
    {
        Lane[] drivingLanes, auxiliaryLanes;
        public Vector2[] Line;
        ObjectLine[] Lines;
        public string Name, LaneTexturePath, SidewalkTexturePath;
        public Path[] RPaths, LPaths;
        public int RoadType = 3, Segments = 5;
        private int LaneTexture, SidewalkTexture;
        public bool Traffic = false;
        public float LaneWidth = 3, SidewalkWidth = 1, LaneHeight = 0.02f, SidewalkHeight = 0.1f, SplitWidth = 1;
        public void FromFile(string path, ref MeshCollector meshes)
        {
            //if (components[6] != "") Misc.Push<ObjectLine>(new ObjectLine(meshes.LoadMesh("./data/maps/Mapa/models/" + components[6] + "/body.obj"), pointCollection, Segments, Width + 0.3f), ref Lines);*/
            //string[] file = System.IO.File.ReadAllLines(path);
            DataParser.ParseData(path, this);
            auxiliaryLanes = new Lane[0];
            Lines = new ObjectLine[0];
            if (LaneTexturePath != null) LaneTexture = Misc.LoadTexture(LaneTexturePath, 1);
            if (SidewalkTexturePath != null) SidewalkTexture = Misc.LoadTexture(SidewalkTexturePath, 1);
            switch (RoadType)
            {
                case 1: //one lane, one way
                    drivingLanes = new Lane[]{new Lane()};
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth, LaneHeight);
                    RPaths = new Path[] { new Path(Line, Segments, 0, LaneHeight, false) };
                    LPaths = new Path[0];
                    break;
                case 2: //two lanes
                    drivingLanes = new Lane[]{new Lane()};
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    RPaths = new Path[] { new Path(Line, Segments, LaneWidth*0.5f, LaneHeight, false) };
                    LPaths = new Path[] { new Path(Line, Segments, LaneWidth*-0.5f, LaneHeight, true) };
                    break;
                case 3: //two lanes, one way
                    drivingLanes = new Lane[]{new Lane()};
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    RPaths = new Path[] { new Path(Line, Segments, LaneWidth*0.5f, LaneHeight, false), new Path(Line, Segments, LaneWidth*-0.5f, LaneHeight, false) };
                    LPaths = new Path[0];
                    break;
                case 4: //two lanes, one way with sidewalk
                    drivingLanes = new Lane[]{new Lane()};
                    auxiliaryLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    auxiliaryLanes[0].GenerateLane(Line, Segments, LaneWidth + (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    auxiliaryLanes[1].GenerateLane(Line, Segments, -LaneWidth - (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    RPaths = new Path[] { new Path(Line, Segments, LaneWidth*0.5f, LaneHeight, false), new Path(Line, Segments, LaneWidth*-0.5f, LaneHeight, false) };
                    LPaths = new Path[0];
                break;
                case 5: //two lanes with sidewalk
                    drivingLanes = new Lane[]{new Lane()};
                    auxiliaryLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    auxiliaryLanes[0].GenerateLane(Line, Segments, LaneWidth + (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    auxiliaryLanes[1].GenerateLane(Line, Segments, -LaneWidth - (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    RPaths = new Path[] { new Path(Line, Segments, LaneWidth*0.5f, LaneHeight, false) };
                    LPaths = new Path[]{ new Path(Line, Segments, LaneWidth*-0.5f, LaneHeight, true) };
                    break;
                case 6: //two lanes with sidewalk on one side
                    drivingLanes = new Lane[]{new Lane()};
                    auxiliaryLanes = new Lane[] { new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 2, LaneHeight);
                    auxiliaryLanes[0].GenerateLane(Line, Segments, LaneWidth + (SidewalkWidth / 2), SidewalkWidth, SidewalkHeight);
                    RPaths = new Path[] { new Path(Line, Segments, LaneWidth*0.25f, LaneHeight, false) };
                    LPaths = new Path[]{ new Path(Line, Segments, LaneWidth*-0.25f, LaneHeight, true) };
                    break;
                case 7: //four lanes
                    drivingLanes = new Lane[]{new Lane()};
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 4, LaneHeight);
                    RPaths = new Path[] { new Path(Line, Segments, LaneWidth * 1.5f, LaneHeight, false), new Path(Line, Segments, LaneWidth * 0.5f, LaneHeight, false) };
                    LPaths = new Path[] { new Path(Line, Segments, LaneWidth * -1.5f, LaneHeight, true), new Path(Line, Segments, LaneWidth * -0.5f, LaneHeight, true) };
                    break;
                case 8: //four lanes with sidewalk
                    drivingLanes = new Lane[]{new Lane()};
                    auxiliaryLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, 0, LaneWidth * 4, LaneHeight);
                    auxiliaryLanes[0].GenerateLane(Line, Segments, (LaneWidth * 2) + (SidewalkWidth / 2), SidewalkWidth * 2, SidewalkHeight);
                    auxiliaryLanes[1].GenerateLane(Line, Segments, -(LaneWidth * 2) - (SidewalkWidth / 2), SidewalkWidth * 2, SidewalkHeight);
                    RPaths = new Path[] { new Path(Line, Segments, LaneWidth * 1.5f, LaneHeight, false), new Path(Line, Segments, LaneWidth * 0.5f, LaneHeight, false) };
                    LPaths = new Path[] { new Path(Line, Segments, LaneWidth * -1.5f, LaneHeight, true), new Path(Line, Segments, LaneWidth * -0.5f, LaneHeight, true) };
                    break;
                case 9: //2X two lanes
                    drivingLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, (SplitWidth / 2) + LaneWidth, LaneWidth * 2, LaneHeight);
                    drivingLanes[1].GenerateLane(Line, Segments, -(SplitWidth / 2) - LaneWidth, LaneWidth * 2, LaneHeight);
                    RPaths = new Path[] { new Path(Line, Segments, (SplitWidth / 2)+(LaneWidth * 1.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2)+(LaneWidth * 0.5f), LaneHeight, false) };
                    LPaths = new Path[] { new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -1.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -0.5f), LaneHeight, true) };
                    break;
                case 10: //2X two lanes with emergency lanes
                case 11: //2X three lanes
                    drivingLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, (SplitWidth / 2) + (LaneWidth * 1.5f), LaneWidth * 3, LaneHeight);
                    drivingLanes[1].GenerateLane(Line, Segments, -(SplitWidth / 2) - (LaneWidth * 1.5f), LaneWidth * 3, LaneHeight);
                    if (RoadType == 10)
                    {
                        RPaths = new Path[] { new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 1.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 0.5f), LaneHeight, false) };
                        LPaths = new Path[] { new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -1.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -0.5f), LaneHeight, true) };
                    }
                    else
                    {
                        RPaths = new Path[] { new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 2.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 1.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 0.5f), LaneHeight, false) };
                        LPaths = new Path[] { new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -2.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -1.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -0.5f), LaneHeight, true) };
                    }
                    break;
                case 12: //2X three lanes with emergency lanes
                case 13: //2X four lanes
                    drivingLanes = new Lane[] { new Lane(), new Lane() };
                    drivingLanes[0].GenerateLane(Line, Segments, (SplitWidth / 2) + (LaneWidth * 2), LaneWidth * 4, LaneHeight);
                    drivingLanes[1].GenerateLane(Line, Segments, -(SplitWidth / 2) - (LaneWidth * 2), LaneWidth * 4, LaneHeight);
                    if (RoadType == 12)
                    {
                        RPaths = new Path[] { new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 2.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 1.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 0.5f), LaneHeight, false) };
                        LPaths = new Path[] { new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -2.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -1.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -0.5f), LaneHeight, true) };
                    }
                    else
                    {
                        RPaths = new Path[] { new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 3.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 2.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 1.5f), LaneHeight, false), new Path(Line, Segments, (SplitWidth / 2) + (LaneWidth * 0.5f), LaneHeight, false) };
                        LPaths = new Path[] { new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -3.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -2.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -1.5f), LaneHeight, true), new Path(Line, Segments, -(SplitWidth / 2) - (LaneWidth * -0.5f), LaneHeight, true) };
                    }
                    break;
            }
            //if (components[7] != "") Misc.Push<ObjectLine>(new ObjectLine(meshes.LoadMesh("./data/maps/Mapa/models/" + components[7] + "/body.obj"), pointCollection, Segments, -Width - 0.3f), ref Lines);
        }

        public void Draw(Matrix4 world, ref MeshCollector meshes)
        {
            
            for (int i = 0; i < drivingLanes.Length; i++)
            {
                if (LaneTexture != 0) { GL.BindTexture(TextureTarget.Texture2D, LaneTexture); GL.Color4(Color4.White); }
                else { GL.BindTexture(TextureTarget.Texture2D, 0);  GL.Color4(Color4.Black); }
                drivingLanes[i].Draw();
            }
            
            for (int i = 0; i < auxiliaryLanes.Length; i++)
            {
                if (SidewalkTexture != 0) GL.BindTexture(TextureTarget.Texture2D, SidewalkTexture);
                else { GL.BindTexture(TextureTarget.Texture2D, 0); GL.Color4(Color4.Black); }
                auxiliaryLanes[i].Draw();
            }
            
            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i].Draw(world, ref meshes);
            }
        }
    }
}
