using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Pathfinding : MonoBehaviour
{
    public Button Visualise, Reset;

    bool startButtonPressed = false;


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

    Scene scene;

    private void Awake() 
    {
        Visualise.onClick.AddListener(StartButtonPressed);
        Reset.onClick.AddListener(ResetGame);
        scene = SceneManager.GetActiveScene();

        StartNode = GameObject.Find("StartNode");
        startNodeScript = StartNode.GetComponent<DragStartNode>();

        EndNode = GameObject.Find("EndNode");
        endNodeScript = EndNode.GetComponent<DragEndNode>();

        HexGrid = GameObject.Find("TileGenerator");
        HexGridScript = GetComponent<HexGrid>();
    }

    private void Update() 
    {

        if (startButtonPressed)
        {
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

                    HexTile = GameObject.Find(x.ToString() + " " + y.ToString());
                    SpriteRenderer tileColour = HexTile.GetComponent<SpriteRenderer>();
                    if (tileColour.color == Color.black)
                        continue;
                    tileColour.color = Color.white;
                }
            }
            startButtonPressed = false;
            FindPath(startNode, targetNode);
        }
    }

    void StartButtonPressed()
    {
        startButtonPressed = true;
    }

    void ResetGame()
    {
        SceneManager.LoadScene(scene.name);
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

                HexTile = GameObject.Find(neighbour.hexTileCorrespondant.x.ToString() + " " + neighbour.hexTileCorrespondant.y.ToString());
                SpriteRenderer tileColour = HexTile.GetComponent<SpriteRenderer>();
                tileColour.color = new Color(0.6784f,0.8471f,0.902f,1f);
            



                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode,neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        HexTile = GameObject.Find(neighbour.hexTileCorrespondant.x.ToString() + " " + neighbour.hexTileCorrespondant.y.ToString());
                        tileColour = HexTile.GetComponent<SpriteRenderer>();
                        tileColour.DOColor(new Color(0.247f,0f,0f,1f),2);
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
