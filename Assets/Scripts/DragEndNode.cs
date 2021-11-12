using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEndNode : MonoBehaviour
{
    GameObject startingNode;
    public bool endNodeSnapped = false;
    public bool draggingMouse = false;

    public Vector3 latestTileSnappedOnEnd;

    private void Awake() 
    {

    }
    private void Start() 
    {
        startingNode = GameObject.Find("19 6");
        latestTileSnappedOnEnd = startingNode.transform.position;    
    }

    void OnMouseDrag() 
    {
        draggingMouse = true;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0))
        {
            draggingMouse = false;
            if (endNodeSnapped)
            {
                endNodeSnapped = false;
            }
            else
            {       
                transform.position = latestTileSnappedOnEnd;
            }
        }
    }

}

