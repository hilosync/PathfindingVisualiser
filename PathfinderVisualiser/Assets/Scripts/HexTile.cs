using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HexTile : MonoBehaviour
{
    int x;
    int y;
    public GameObject HexGrid;
    public GameObject StartNode;
    public GameObject EndNode;
    private HexGrid hexGridScript;

    private DragStartNode StartNodeDraggingScript;
    private DragEndNode EndNodeDraggingScript;
    private Collider2D hexTileCollider;


    
    // Start is called before the first frame update
    void Start()
    {
        HexGrid = GameObject.Find("TileGenerator");
        hexGridScript = HexGrid.GetComponent<HexGrid>();

        StartNode = GameObject.Find("StartNode");
        StartNodeDraggingScript = StartNode.GetComponent<DragStartNode>();

        EndNode = GameObject.Find("EndNode");
        EndNodeDraggingScript = EndNode.GetComponent<DragEndNode>();

        int x = Int16.Parse(gameObject.name.Split(char.Parse(" "))[0]);
        int y = Int16.Parse(gameObject.name.Split(char.Parse(" "))[1]);

        if (hexGridScript.gridArray[x, y] == 2)
        {
            StartNode.transform.position = gameObject.transform.position;
        }

        if (hexGridScript.gridArray[x, y] == 3)
        {
            EndNode.transform.position = gameObject.transform.position;
        }

        hexTileCollider = gameObject.GetComponent<Collider2D>(); 

    }

     void OnMouseOver() 
        {
            int x = Int16.Parse(gameObject.name.Split(char.Parse(" "))[0]);
            int y = Int16.Parse(gameObject.name.Split(char.Parse(" "))[1]);

            if (Input.GetMouseButton(0) && hexGridScript.gridArray[x, y] != 2 && hexGridScript.gridArray[x, y] != 3 && !StartNodeDraggingScript.draggingMouse && !EndNodeDraggingScript.draggingMouse)
            {

                hexGridScript.SetValue(x, y, 1);
                gameObject.GetComponent<SpriteRenderer>().color = Color.black; 
                
            }

        }






    // Update is called once per frame
    void Update()
    {
        x = Int16.Parse(gameObject.name.Split(char.Parse(" "))[0]);
        y = Int16.Parse(gameObject.name.Split(char.Parse(" "))[1]);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 finalPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButtonUp(0) && hexTileCollider.bounds.Intersects(StartNode.GetComponent<Collider2D>().bounds) && hexTileCollider.bounds.Contains(finalPos) && hexGridScript.gridArray[x, y] != 3)
        {
            int x = Int16.Parse(gameObject.name.Split(char.Parse(" "))[0]);
            int y = Int16.Parse(gameObject.name.Split(char.Parse(" "))[1]);
            StartNode.transform.position = gameObject.transform.position;
            hexGridScript.SetValue(x, y, 2);
            StartNodeDraggingScript.startNodeSnapped = true;
            StartNodeDraggingScript.latestTileSnappedOnStart = gameObject.transform.position;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        

        if (Input.GetMouseButtonUp(0) && hexTileCollider.bounds.Intersects(EndNode.GetComponent<Collider2D>().bounds) && hexTileCollider.bounds.Contains(finalPos) && hexGridScript.gridArray[x, y] != 2)
        {
            int x = Int16.Parse(gameObject.name.Split(char.Parse(" "))[0]);
            int y = Int16.Parse(gameObject.name.Split(char.Parse(" "))[1]);
            EndNode.transform.position = gameObject.transform.position;
            hexGridScript.SetValue(x, y, 3);
            EndNodeDraggingScript.endNodeSnapped = true;
            EndNodeDraggingScript.latestTileSnappedOnEnd = gameObject.transform.position;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;          
        }




       
    }
}
