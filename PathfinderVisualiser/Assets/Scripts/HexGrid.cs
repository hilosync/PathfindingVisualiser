using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class HexGrid : MonoBehaviour
{
    private int startNodePreviousX = 5;
    private int startNodePreviousY = 4;

    private int endNodePreviousX = 15;
    private int endNodePreviousY = 4;
    public GameObject hexTilePrefab;
    public int width;
    public int height;
    public float cellSize;
    public float tileXOffset = 2f;
    public float tileYOffset = 1.765f;
    public int[,] gridArray;
    public TextMesh[,] debugTextArray;

    public Pathfinding PathfindingScript;

    public Node[,] nodeGrid;





    void Start() 
    {
        PathfindingScript = GetComponent<Pathfinding>();
        CreateHexGrid();
    }
    void CreateHexGrid () 
    {
        nodeGrid = new Node[width,height];

        gridArray = new int[width,height];
        debugTextArray = new TextMesh[width,height];

        gridArray[5, 4] = 2;
        gridArray[15, 4] = 3;

        for (int x = 0; x < gridArray.GetLength(0); x++) 
        {
            for (int y = 0; y < gridArray.GetLength(1); y++) 
            {
                GameObject TempObj = Instantiate(hexTilePrefab);

                if (y % 2 == 0)
                {
                    TempObj.transform.position = new Vector3(x * tileXOffset, y * tileYOffset, 0);
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x,y].ToString(), null, GetWorldPosition(TempObj.transform.position.x,TempObj.transform.position.y), 12, Color.magenta, TextAnchor.MiddleCenter);
                }

                else 
                {
                    TempObj.transform.position = new Vector3(x * tileXOffset + tileXOffset / 2, y * tileYOffset, 0);
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x,y].ToString(), null, GetWorldPosition(TempObj.transform.position.x,TempObj.transform.position.y), 12, Color.magenta, TextAnchor.MiddleCenter);
                }
                SetTileInfo(TempObj, x, y);
                nodeGrid[x,y] = new Node(gridArray[x,y], new Vector2(x,y));

            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if((checkY % 2 != 0 && x == 1 && y == 1) || (checkY % 2 != 0 && x == 1 && y == -1) || (checkY % 2 == 0 && x == -1 && y == 1))
                {
                    Debug.Log(checkX + " " + checkY);
                    continue;
                }

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbours.Add(nodeGrid[checkX,checkY]);
                }
            }
        }
        return neighbours;
    }



    private Vector3 GetWorldPosition(float x, float y) 
    {
        return new Vector3(x,y) * cellSize;
    }

    void SetTileInfo(GameObject TempObj, int x, int y)
    {
        TempObj.transform.parent = transform;
        TempObj.name = x.ToString() + " " + y.ToString();
    }

    public void SetValue (int x, int y, int value)
    {
        if (value == 2)
        {
            gridArray[startNodePreviousX, startNodePreviousY] = 0;
            debugTextArray[startNodePreviousX, startNodePreviousY].text = gridArray[startNodePreviousX, startNodePreviousY].ToString();  
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            startNodePreviousX = x;
            startNodePreviousY = y;
            nodeGrid[x,y] = new Node(2, new Vector2(x,y));
        }

        if (value == 3)
        {
            gridArray[endNodePreviousX, endNodePreviousY] = 0;
            debugTextArray[endNodePreviousX, endNodePreviousY].text = gridArray[endNodePreviousX, endNodePreviousY].ToString();
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            endNodePreviousX = x;
            endNodePreviousY = y;
            nodeGrid[x,y] = new Node(3, new Vector2(x,y));
        }
        else
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            nodeGrid[x,y] = new Node(value, new Vector2(x,y));
        }
    }


}
