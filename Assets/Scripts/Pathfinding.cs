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

    float timeOrder = 0.2f;

    Color currentNodeColour = new Color(0.831f,0.008f,0.91f,1f);
    Color dissipatedNodeColour = new Color(0.61f,0.008f,0.949f,1f);
    Color pathTileColour = new Color(0.098f, 0.008f, 0.949f, 1f);

    //Color neighbourTileColour = new Color(0.6f, 0.6f, 0.6f, 1f); Commented out code that changes neighbour nodes colours as it might look a bit better this way


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

                    timeOrder = 0.2f;
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
            HexTile = GameObject.Find(currentNode.hexTileCorrespondant.x.ToString() + " " + currentNode.hexTileCorrespondant.y.ToString());
            SpriteRenderer tileColour = HexTile.GetComponent<SpriteRenderer>();

            StartCoroutine(TileColourChanger(tileColour, currentNodeColour, timeOrder));
            timeOrder = timeOrder + 0.05f;

            StartCoroutine(TileColourChanger(tileColour, dissipatedNodeColour, timeOrder));
            timeOrder = timeOrder + 0.06f;
            
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

                //HexTile = GameObject.Find(neighbour.hexTileCorrespondant.x.ToString() + " " + neighbour.hexTileCorrespondant.y.ToString());
                //tileColour = HexTile.GetComponent<SpriteRenderer>();


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
                        //tileColour = HexTile.GetComponent<SpriteRenderer>();
                        //StartCoroutine(TileColourChanger(tileColour, neighbourTileColour, timeOrder));
                        //timeOrder = timeOrder + 0.05f;
                    }
                }
            }
        }

    }

    IEnumerator TileColourChanger(SpriteRenderer tile, Color colour, float timeOrder)
    {
        yield return new WaitForSeconds(timeOrder);
        if (colour == currentNodeColour)
            tile.DOColor(colour, 0.001f);
        tile.DOColor(colour,0.2f);
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
                StartCoroutine(TileColourChanger(tileColourComponent, pathTileColour, timeOrder));
                timeOrder = timeOrder + 0.1f;
            }
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int penalty = 0;

        
        int dstX = (int)Mathf.Abs(nodeA.hexTileCorrespondant.x - nodeB.hexTileCorrespondant.x);
        int dstY = (int)Mathf.Abs(nodeA.hexTileCorrespondant.y - nodeB.hexTileCorrespondant.y);

        if ((nodeA.hexTileCorrespondant.y % 2 == 0 && nodeB.hexTileCorrespondant.y % 2 != 0 && (nodeA.hexTileCorrespondant.x < nodeB.hexTileCorrespondant.x)) || (nodeB.hexTileCorrespondant.y % 2 == 0 && nodeA.hexTileCorrespondant.y % 2 != 0 && (nodeB.hexTileCorrespondant.x < nodeA.hexTileCorrespondant.x)))
            penalty = 1;

        return (int)Mathf.Max(dstY, dstX + Mathf.Floor(dstY/2) + penalty);
    }
}
