using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Node targetNode;
    Node startNode;
    GameObject HexTile;
    GameObject HexGrid;

    HexGrid HexGridScript;
    SpriteRenderer tileColourComponent;

    GameObject StartNode;
    private DragStartNode startNodeScript;

    GameObject EndNode;
    private DragEndNode endNodeScript;

    private void Awake() 
    {
        StartNode = GameObject.Find("StartNode");
        startNodeScript = StartNode.GetComponent<DragStartNode>();

        EndNode = GameObject.Find("EndNode");
        endNodeScript = EndNode.GetComponent<DragEndNode>();

        HexGrid = GameObject.Find("TileGenerator");
        HexGridScript = GetComponent<HexGrid>();
    }

    private void Update() 
    {

        if (Input.GetKeyUp("s"))
            for (int x = 0; x < HexGridScript.width; x++)
            {
                for (int y = 0; y < HexGridScript.height; y++)
                {
                    if(HexGridScript.gridArray[x,y] == 2)
                    {
                        startNode = HexGridScript.nodeGrid[x,y];
                    }

                    if(HexGridScript.gridArray[x,y] == 3)
                    {
                        targetNode = HexGridScript.nodeGrid[x,y];
                    }
                }
            }
            FindPath(startNode, targetNode);
    }
    void FindPath(Node startNode, Node endNode)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();


        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                } 
            }
            
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }
            
            foreach (Node neighbour in HexGridScript.GetNeighbours(currentNode))
            {
                if (neighbour.valueType == 1 || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode,neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

    }

    void RetracePath (Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        if (path != null)
        {
            foreach (Node n in path)
            {
                HexTile = GameObject.Find(n.hexTileCorrespondant.x.ToString() + " " + n.hexTileCorrespondant.y.ToString());
                tileColourComponent = HexTile.GetComponent<SpriteRenderer>();
                tileColourComponent.color = Color.blue;
            }
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = (int)Mathf.Abs(nodeA.hexTileCorrespondant.x - nodeB.hexTileCorrespondant.x);
        int dstY = (int)Mathf.Abs(nodeA.hexTileCorrespondant.y - nodeB.hexTileCorrespondant.y);

        if ((float)dstY/2 > (float)dstX)
        {
            return dstY;
        }
        else
        {
            return (int)(dstY + (dstX - Mathf.Floor(dstY/2)));
        }
    }
}
