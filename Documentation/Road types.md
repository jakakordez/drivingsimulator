#Roads

#Attributes

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

##Road types

1. one lane
2. one lane, one way
3. two lanes
4. two lanes, one way
5. two lanes with sidewalk
6. two lanes with sidewalk on one side
7. four lanes
8. four lanes with sidewalk
9. 2X two lanes
10. 2X two lanes with emergency lanes
11. 2X three lanes
12. 2X three lanes with emergency lanes
13. 2X four lanes