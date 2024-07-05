using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    Vector2Int startCoordinates;
    public Vector2Int StartCoordinates{get{return startCoordinates;}}
    public Vector2Int DestinationCoordinates{get{return destinationCoordinates;}}
    Vector2Int destinationCoordinates;

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier=new Queue<Node>(); 
    Dictionary<Vector2Int,Node> reached=new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions={Vector2Int.up, Vector2Int.right,Vector2Int.left, Vector2Int.down};
    GridManager gridManager;
    Dictionary<Vector2Int,Node> grid=new Dictionary<Vector2Int, Node>();

    void Awake()
    {
        gridManager=FindObjectOfType<GridManager>();
        if(gridManager!=null)
        {
            grid=gridManager.Grid;
        } 
    }
    

    public List<Node> GetNewPath(Vector2Int startCoo,Vector2Int destCoo)
    {
        //Set start and destination node
        startCoordinates=startCoo;
        destinationCoordinates=destCoo;
        startNode=grid[startCoordinates];
        destinationNode=grid[destinationCoordinates];
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNode();
        BreadthFirstSearch(coordinates);//Find path using BFS algo
        return BuildPath();
    }

    void ExploreNeighbours()
    {
        List<Node> nieghbours=new List<Node>();

        //Find all the tile adjacent to current one
        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighbourCoords=currentSearchNode.coordinates+direction;
            if(grid.ContainsKey(neighbourCoords))
            {
                nieghbours.Add(grid[neighbourCoords]);
            } 
        }
        //if neighbour is not reached and can be traversed enque in frontier queue
        foreach(Node neighbour in nieghbours)
        {
            if(!reached.ContainsKey(neighbour.coordinates) && neighbour.isWalkable)
            {
                neighbour.connectedTo=currentSearchNode;
                reached.Add(neighbour.coordinates,neighbour);
                frontier.Enqueue(neighbour);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)//BFS algorthm to find path to destination
    {
        startNode.isWalkable=true;
        destinationNode.isWalkable=true;
        frontier.Clear();
        reached.Clear();
        bool isRunning=true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates,grid[coordinates]);

        while(frontier.Count>0 && isRunning)
        {
            currentSearchNode=frontier.Dequeue();
            currentSearchNode.isExplored=true;
            ExploreNeighbours();//Get neighbours of current tile
            //Check if current tile is also destination
            if(currentSearchNode.coordinates==destinationCoordinates)
            {
                isRunning=false;
            }
        }
        BuildPath();
    }

    List<Node> BuildPath()//Move in reverse from destination to start while traversing every node connected to initial one
    {
        List<Node> path=new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath=true;

        while(currentNode.connectedTo!=null)//if node is connected to current one, add in path
        {
            currentNode=currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath=true;
        }
        path.Reverse();
        return path;
    }


}
