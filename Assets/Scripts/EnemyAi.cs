
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour,AI
{
    [SerializeField][Range(0,5)] float speed=1f;
    Vector2Int[] directions={Vector2Int.up, Vector2Int.right,Vector2Int.left, Vector2Int.down};

    Vector2Int startCoordinates;
    List<Vector2Int> destinationCoordinates=new List<Vector2Int>();
    List<Node> destinationNode=new List<Node>();

    List<Node> path=new List<Node>();
    Dictionary<Vector2Int,Node> grid=new Dictionary<Vector2Int, Node>();

    Node startNode;
    Node currentSearchNode;
    Node finalDestinationNode;
    
    Queue<Node> frontier=new Queue<Node>(); 
    Dictionary<Vector2Int,Node> reached=new Dictionary<Vector2Int, Node>();
   
    GridManager gridManager;
    GameManager gameManager;
    Player player;

    bool isMoving=false;

    void Awake()
    {
        gridManager=FindObjectOfType<GridManager>();
        if(gridManager!=null)
        {
            grid=gridManager.Grid;
        } 
        gameManager=FindObjectOfType<GameManager>();
        player=FindObjectOfType<Player>();
    }

    void Start()
    {
        //Set tile occupied to not walkable
        gridManager.Grid[gridManager.GetCooordinatesFromPosition(transform.position)].isWalkable=false;
    }
    
    void Update()
    {
        if(!gameManager.isPlayerTurn)//Move if it is not player turn
        {
            Move();
        }
    }

    public void Move()
    {
        if(!isMoving)//Move only if not in motion
        {
            isMoving=true;
            //Get start and destination Nodes
            gridManager.Grid[gridManager.GetCooordinatesFromPosition(transform.position)].isWalkable=true;
            startCoordinates=gridManager.GetCooordinatesFromPosition(transform.position);
            FindDestinationCoordinates();
            startNode=grid[startCoordinates];
            GetDestinationNode();
        
            gridManager.ResetNode();
            //Find path
            BreadthFirstSearch(startCoordinates);
            path=BuildPath();
            //start following path
            StartCoroutine(FollowPath());
        }
    }


    void FindDestinationCoordinates()
    {
        destinationCoordinates.Clear();
        Vector2Int playerCoordinates=gridManager.GetCooordinatesFromPosition(player.transform.position);
        foreach(Vector2Int direction in directions)
        {
            Vector2Int destCoo=direction+playerCoordinates;
            if(gridManager.Grid[destCoo].isWalkable)//Check if the adjacent tile is available
            {
                destinationCoordinates.Add(destCoo);
            }
        }
    }

    void GetDestinationNode()
    {
        destinationNode.Clear();
        foreach(Vector2Int destinationCoordinate in destinationCoordinates)
        {
            destinationNode.Add(grid[destinationCoordinate]);
        }
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
        foreach(Node destination in destinationNode)
        {
            destination.isWalkable=true;
        }
        //clear previous path
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
            foreach(Vector2Int destinationCoordinate in destinationCoordinates)
            {
                //Check if current tile is also destination
                if(currentSearchNode.coordinates==destinationCoordinate)
                {
                    isRunning=false;
                    finalDestinationNode=currentSearchNode;
                }
            } 
        }
        BuildPath();
    }

    List<Node> BuildPath()//Move in reverse from destination to start while traversing every node connected to initial one
    {
        List<Node> path=new List<Node>();
        Node currentNode = finalDestinationNode;

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

    IEnumerator FollowPath()
    {
        for(int i=1;i<path.Count;i++)
        {
            Vector3 startPosition=transform.position;
            Vector3 endPosition=gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent=0f;

            //Rotate player in direction of motion
            transform.LookAt(endPosition);
            //Move player from one tile to next
            while(travelPercent < 1f)
            {
                travelPercent+=Time.deltaTime*speed;
                transform.position=Vector3.Lerp(startPosition,endPosition,travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }  
        isMoving=false;
        gameManager.isPlayerTurn=true;
        gridManager.Grid[gridManager.GetCooordinatesFromPosition(transform.position)].isWalkable=false;
    }
}
