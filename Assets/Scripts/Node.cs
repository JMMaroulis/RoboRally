using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int GraphX;
    public int GraphY;
    public Dictionary<string, Node> Neighbours = new Dictionary<string, Node>();
    //public Tile Tile;
    public bool VictorySpace;
}
