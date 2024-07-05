using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField][Range(0f,5f)] float speed=1f;
    List<Node> path=new List<Node>();

    PathFinder pathFinder;
    GridManager gridManager;
    GameManager gameManager;
    Animator anim;

    bool isMoving=false;

    void Awake()
    {
        gridManager=FindObjectOfType<GridManager>();
        pathFinder=FindObjectOfType<PathFinder>();
        gameManager=FindObjectOfType<GameManager>();
        anim=GetComponent<Animator>();
    }

    
    public void Move(Vector2Int destCoo)
    {
        if(!isMoving)//To prevent player motion if already moving
        {
            isMoving=true;
            anim.SetBool("isRunning", true);
            //Make current tile available for motion
            gridManager.Grid[gridManager.GetCooordinatesFromPosition(transform.position)].isWalkable=true;

            //Get the path player will follow
            path=pathFinder.GetNewPath(gridManager.GetCooordinatesFromPosition(transform.position),destCoo);   
            StartCoroutine(FollowPath());
        }
    }

    void OnMouseDown()
    {
        //To select or deselect player
        if(gameManager.selectedPlayer!=gameObject)
        {
            gameManager.selectedPlayer=gameObject;
        }
        else
        {
            gameManager.selectedPlayer=null;
        }
        
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
        anim.SetBool("isRunning", false);
        gameManager.isPlayerTurn=false;
        gameManager.selectedPlayer=null; 
        gridManager.Grid[gridManager.GetCooordinatesFromPosition(transform.position)].isWalkable=false;
    }
}