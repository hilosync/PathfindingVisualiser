using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int valueType;
    public Vector2 hexTileCorrespondant;
    public int gCost;
    public int hCost;
    public Node parent;
    public int gridX;
    public int gridY;

    public Node (int _valueType, Vector2 _hexTileCorrespondant)
    {
        valueType = _valueType;
        hexTileCorrespondant = _hexTileCorrespondant;
        gridX = (int)hexTileCorrespondant.x;
        gridY = (int)hexTileCorrespondant.y;

    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
