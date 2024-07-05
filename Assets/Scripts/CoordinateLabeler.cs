using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor=Color.white;
    TextMeshPro label;
    WayPoint wayPoint;
    Vector2Int coordinates=new Vector2Int();

    void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled=false;
        wayPoint=GetComponentInParent<WayPoint>();
    }
    void Update()
    {
        
        DisplayCoordinates();// Show tiles coordinate
        UpdateObjectName();//Update name of tiles in hierarchy
        ToggleLabels();//Turn on/off coordinates by pressing 'C'
    }

    void ToggleLabels()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            label.enabled=!label.IsActive();
        }
    }

    

    void DisplayCoordinates()
    {
        coordinates.x=Mathf.RoundToInt(transform.parent.position.x/10);
        coordinates.y=Mathf.RoundToInt(transform.parent.position.z/10);
        
        label.text=coordinates.x+","+coordinates.y;
    }

    void UpdateObjectName()
    {
        transform.parent.name=coordinates.ToString();
    }
}
