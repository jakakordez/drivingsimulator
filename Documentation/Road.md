#Roads

Roads are stored in separate file, located in the *data/maps/mapName/roads* directory.
File is named with road name and json extension.

##Attributes

- **Name** [*string*] - Road name
- **RoadType** [*int*] - See Road types below
- **Segments** [*int*] - Number of road segments
- **Traffic** [*bool*] - Populate road with traffic
- **Limit** [*int*] - Speed limit
- **LaneWidth** [*float*]
- **SidewalkWidth** [*float*]
- **LaneHeight** [*float*]
- **SidewalkHeight** [*float*]
- **SplitWidth** [*float*]
- **Line** - Set of points used for road curve generation
- **LaneTexturePath** [*string*] - Texture for driving lanes
- **SidewalkTexturePath** [*string*] - Texture for sidewalk

##Road types

1. one lane
2. two lanes
3. two lanes, one way
4. two lanes, one way with sidewalk
5. two lanes with sidewalk
6. two lanes with sidewalk on one side
7. four lanes
8. four lanes with sidewalk
9. 2X two lanes
10. 2X two lanes with emergency lanes
11. 2X three lanes
12. 2X three lanes with emergency lanes
13. 2X four lanes