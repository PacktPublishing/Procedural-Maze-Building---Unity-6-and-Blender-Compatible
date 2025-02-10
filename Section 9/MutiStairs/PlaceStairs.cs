bool PlaceStairs(int mazeIndex, float rotAngle, Maze.PieceType bottomType, Maze.PieceType upperType, GameObject stairPrefab)
{
    List<MapLocation> startingLocations = new List<MapLocation>();
    List<MapLocation> endingLocations = new List<MapLocation>();

    for (int z = 0; z < depth; z++)
        for (int x = 0; x < width; x++)
        {
            if (mazes[mazeIndex].piecePlaces[x, z].piece == bottomType)
                startingLocations.Add(new MapLocation(x, z));

            if (mazes[mazeIndex + 1].piecePlaces[x, z].piece == upperType)
                endingLocations.Add(new MapLocation(x, z));
        }

    if (startingLocations.Count == 0 || endingLocations.Count == 0) return false;

    MapLocation bottomOfStairs = startingLocations[UnityEngine.Random.Range(0, startingLocations.Count)];
    MapLocation topOfStairs = endingLocations[UnityEngine.Random.Range(0, endingLocations.Count)];

    mazes[mazeIndex + 1].xOffset = bottomOfStairs.x - topOfStairs.x + mazes[mazeIndex].xOffset;
    mazes[mazeIndex + 1].zOffset = bottomOfStairs.z - topOfStairs.z + mazes[mazeIndex].zOffset;

    Vector3 stairPosBottom = new Vector3(bottomOfStairs.x * mazes[mazeIndex].scale,
                                                mazes[mazeIndex].scale * mazes[mazeIndex].level
                                                        * mazes[mazeIndex].levelDistance,
                                                bottomOfStairs.z * mazes[mazeIndex].scale);
    Vector3 stairPosTop = new Vector3(topOfStairs.x * mazes[mazeIndex + 1].scale,
                                                mazes[mazeIndex + 1].scale * mazes[mazeIndex + 1].level
                                                        * mazes[mazeIndex + 1].levelDistance,
                                                topOfStairs.z * mazes[mazeIndex + 1].scale);

    Destroy(mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model);
    Destroy(mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model);

    GameObject stairs = Instantiate(stairPrefab, stairPosBottom, Quaternion.identity);
    stairs.transform.Rotate(0, rotAngle, 0);
    mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model = stairs;
    mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].piece = Maze.PieceType.Manhole;

    mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model = null;
    mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].piece = Maze.PieceType.Manhole;

    stairs.transform.SetParent(mazes[mazeIndex].gameObject.transform);
    return true;
}