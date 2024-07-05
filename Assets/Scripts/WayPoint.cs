using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour
{
    [SerializeField] TMP_Text tileInfo;
    ObjectPool objectPool;
    GridManager gridManager;
    PathFinder pathFinder;
    GameManager gameManager;
    Vector2Int coordinates=new Vector2Int();

    void Awake()
    {
        gridManager=FindObjectOfType<GridManager>(); 
        objectPool = FindObjectOfType<ObjectPool>();
        gameManager=FindObjectOfType<GameManager>();
        pathFinder=FindObjectOfType<PathFinder>(); 
    }

    void Start()
    {
        
        if(gridManager!=null)
        {
            coordinates=gridManager.GetCooordinatesFromPosition(transform.position);
        }
    }
    
    void OnMouseEnter()
    {
        //Display tile info and selection marker
        objectPool.EnableSelectedTile(transform.position);
        tileInfo.text=$"Tile:{transform.name}";
    }
    void OnMouseExit()
    {
        //Disable tile info and selection marker
        tileInfo.text="";
        objectPool.DisableSelectedTile();
    }
    
    void OnMouseDown()
    {
        //if player unit is selected and current tile can be traversed move to it
        if(gridManager.Grid[coordinates].isWalkable && gameManager.isPlayerTurn==true && gameManager.selectedPlayer!=null)
        {
            gameManager.selectedPlayer.BroadcastMessage("Move",coordinates,SendMessageOptions.DontRequireReceiver);
        }
        
    }
}
