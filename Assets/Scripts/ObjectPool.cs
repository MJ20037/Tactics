using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject selectedTile;
    GameObject tile;

    void Awake(){
        PopulatePool();//Instantiate tile selection marker
    }

    
    void PopulatePool()
    {
        tile=Instantiate(selectedTile);
        tile.SetActive(false);
    }

    public void EnableSelectedTile(Vector3 position)
    {
        tile.SetActive(true);
        tile.transform.position=position;
    }
    public void DisableSelectedTile()
    {
        tile.SetActive(false);
    }
}
