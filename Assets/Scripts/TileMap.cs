using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public static Graph Graph;
    public GameObject Tile;
    int MapSizeX = 10;
    int MapSizeY = 10;

    void Awake()
    {
        Debug.Log("Started Generating Map Data");
        GenerateMapData();
        Debug.Log("Finished Generating Map Data");

        Debug.Log("Started Generating Map Tiles");
        GenerateMapTiles();
        Debug.Log("Finished Generating Map Tiles");

    }

    void GenerateMapData()
    {
        Graph = new Graph();
        Graph.nodes = new Node[MapSizeX, MapSizeY];

        //allocate tile types to array,
        //initialise graph nodes
        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                Graph.nodes[x, y] = new Node();
                Graph.nodes[x, y].GraphX = x;
                Graph.nodes[x, y].GraphY = y;
            }
        }

        //set node neighbours in graph
        //4-way neigbours; up down left right
        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                //probably need to make this bit more resilient to missing nodes; should be fine for an x by y grid though
                try
                {
                    if (x > 0) Graph.nodes[x, y].Neighbours.Add("left", Graph.nodes[x - 1, y]);
                    if (x < MapSizeX-1) Graph.nodes[x, y].Neighbours.Add("right", Graph.nodes[x + 1, y]);
                    if (y > 0) Graph.nodes[x, y].Neighbours.Add("down", Graph.nodes[x, y - 1]);
                    if (y < MapSizeY-1) Graph.nodes[x, y].Neighbours.Add("up", Graph.nodes[x, y + 1]);
                }
                catch(System.Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

    }

    void GenerateMapTiles()
    {
        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                Instantiate(Tile, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

    }

}
