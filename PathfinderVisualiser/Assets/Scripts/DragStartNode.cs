using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragStartNode : MonoBehaviour
{
    GameObject startingNode;
    public bool draggingMouse = false;
    public bool startNodeSnapped = false;

    public Vector3 latestTileSnappedOnStart;


    private void Awake() 
    {

    }
    private void Start() 
    {
        startingNode = GameObject.Find("5 4");
        latestTileSnappedOnStart = startingNode.transform.position;
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
            if (startNodeSnapped)
            {
                startNodeSnapped = false;
            }
            else
            {
                transform.position = latestTileSnappedOnStart;
            }
        }

    }

}
