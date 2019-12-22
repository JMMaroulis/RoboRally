using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public static Graph Graph;
    public GameObject Tile;
    public GameObject VictoryTile;
    public GameObject BlockedTile;
    public int MapSizeY = 10;
    public int MapSizeX = 10;
    public float mapScalingFactor = 0.75f;

    void Awake()
    {
        Debug.Log("Started Generating Map Data");
        GenerateMapData();
        Debug.Log("Finished Generating Map Data");

        Debug.Log("Started Generating Map Tiles");
        GenerateMapTiles(Graph.nodes);
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

        //choose random tile to be victory tile
        Graph.nodes[Random.Range(0, MapSizeX - 1), Random.Range(0, MapSizeY - 1)].VictorySpace = true;

    }

    //iterate over nodemap, spawn tiles according to tile properties
    void GenerateMapTiles(Node[,] nodes)
    {
        float xcoord = -(MapSizeX / 2);
        float ycoord = -(MapSizeY / 2);

        foreach (Node node in nodes)
        {
            if (node.VictorySpace == true)
            {
                node.Tile = Instantiate(VictoryTile, new Vector3((xcoord + node.GraphX)*mapScalingFactor, (ycoord + node.GraphY)*mapScalingFactor, 0), Quaternion.identity);
                //node.Tile.transform.localScale = new Vector3(mapScalingFactor, mapScalingFactor, mapScalingFactor);
                node.VictorySpace = true;
            }
            else if (Random.Range(0f, 10f) > 9f)
            {
                node.Tile = Instantiate(BlockedTile, new Vector3((xcoord + node.GraphX) * mapScalingFactor, (ycoord + node.GraphY) * mapScalingFactor, 0), Quaternion.identity);
                //node.Tile.transform.localScale = new Vector3(mapScalingFactor, mapScalingFactor, mapScalingFactor);
                node.BlockedSpace = true;
            }
            else
            {
                node.Tile = Instantiate(Tile, new Vector3((xcoord + node.GraphX) * mapScalingFactor, (ycoord + node.GraphY) * mapScalingFactor, 0), Quaternion.identity);
                //node.Tile.transform.localScale = new Vector3(mapScalingFactor, mapScalingFactor, mapScalingFactor);
            }
        }

    }

}
