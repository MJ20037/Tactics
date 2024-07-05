using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    
    public Vector2Int coordinates;
    public bool isWalkable;//if tile can be traversed
    public bool isExplored;//if we have explored tile
    public bool isPath;//if tile is path
    public Node connectedTo;//Which tile is connected to current one

    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
