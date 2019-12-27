using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public GameObject graph;
    static Node graphPosition;
    public static string facing;

    void Start()
    {
        //set initial node. match position to it
        //(we're assuming a 1:1 relationship between graph coordinates and gamespace coordinates here)
        graphPosition = TileMap.Graph.nodes[0, 0];
        Debug.Log("start position:" + transform.position);
        transform.position = graphPosition.Tile.transform.position;
        Debug.Log("new position:" + transform.position);
        //transform.localScale = graphPosition.Tile.transform.localScale;
        //TODO: this is a horrific little hack that lets me do sprite layering easily with a perpective camera
        //absolutely have to figure out how to use an orthographic camera properly
        //(also it's being used elsewhere. eep.)
        transform.Translate(0, 0, -0.0001f);
        Debug.Log("new_new position:" + transform.position);

        //set sprite size
        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.size = new Vector2(0.001f, 0.001f);
        
        //set facing
        facing = "up";
    }

    //get neighbour with given input as direction string
    static Node GetNeighbour(Node graphPosition, string direction)
    {
        //Debug.Log("looking for neighbour in direction: " + direction);
        try
        {
            return graphPosition.Neighbours[direction];
        }
        catch
        {
            //Debug.Log("No neighbour found in given direction, returning self");
            return graphPosition;
        }
    }

    //external calling points to move whatever object this script is attached to
    //mostly going to be used in ExecuteCards.cs
    public void MoveDirection(string direction)
    {
        Node graphDestination = GetNeighbour(graphPosition, direction);

        //check that destination is not blocked off or occupied
        if (graphDestination.BlockedSpace == true | graphDestination.Occupied == true)
        {
            Debug.Log("Collision; no movement");
            CurrentNodeCheck();
            return;
        }

        //vacate current space, find next space, occupy it
        graphPosition.Occupied = false;
        graphPosition = GetNeighbour(graphPosition, direction);
        graphPosition.Occupied = true;

        //a bit concerned this is gonna cause some sort of 'blinking' effect as it moves from z=-1 -> z=0 -> z=-1
        transform.position = graphPosition.Tile.transform.position;
        transform.Translate(0, 0, -0.00001f);
        CurrentNodeCheck();
    }

    public void MoveDirectionRelative(string direction)
    {
        string newDirection = RelativeDirectionToAbsolute(direction, facing);
        MoveDirection(newDirection);
    }

    public string RelativeDirectionToAbsolute(string direction, string facing)
    {
        //turn relative facing direction into absolute coordinate direction
        //(this is going to be awful, but I'm put myself into a bit of a corner. Should've thought about doing
        //this with maths instead of doing it all with strings.)

        //populate dict with direction replacements based on facing
        var converter = new Dictionary<string, string>();
        switch (facing)
        {
            case "up":
                converter.Add("forward", "up");
                converter.Add("backward", "down");
                break;

            case "down":
                converter.Add("forward", "down");
                converter.Add("backward", "up");
                break;

            case "left":
                converter.Add("forward", "left");
                converter.Add("backward", "right");
                break;

            case "right":
                converter.Add("forward", "right");
                converter.Add("backward", "left");
                break;

            default:
                break;
        }

        //Debug.Log("convering via dictionary: " + direction);
        //Debug.Log("returning via dictionary: " + converter[direction]);
        return converter[direction];
    }

    public void Turn(string direction)
    {
        //populate dict with direction replacements based on facing
        var converter = new Dictionary<string, string>();
        switch (facing)
        {
            case "up":
                converter.Add("left", "left");
                converter.Add("right", "right");
                break;

            case "down":
                converter.Add("left", "right");
                converter.Add("right", "left");
                break;

            case "left":
                converter.Add("left", "down");
                converter.Add("right", "up");
                break;

            case "right":
                converter.Add("left", "up");
                converter.Add("right", "down");
                break;

            default:
                break;
        }

        //rotate object transform
        switch (direction)
        {
            case "left":
                this.transform.Rotate(0, 0, 90);
                break;
            case "right":
                this.transform.Rotate(0, 0, -90);
                break;
            default:
                break;
        }

        //Debug.Log("convering via dictionary: " + direction);
        //Debug.Log("returning via dictionary: " + converter[direction]);
        facing = converter[direction];
    }

    //for checking for collision and such
    public void CurrentNodeCheck()
    {
        if (graphPosition.VictorySpace == true)
        {
            Debug.Log("Yay, you made it!");
        }
    }

    //for checking end-of turn stuff, like if you're on the right space
    public void CurrentNodeCheck_MovementEnd()
    {
        if (graphPosition.VictorySpace == true)
        {
            Debug.Log("Yay, you made it!");
            LevelReset levelReset = GameObject.FindObjectOfType<LevelReset>();
            levelReset.ResetScene();
        }
    }

    /*
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //Debug.Log(graphPosition.GraphX + "_" + graphPosition.GraphY);
            graphPosition = GetNeighbour(graphPosition, "left");
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Debug.Log(graphPosition.GraphX + "_" + graphPosition.GraphY);
            graphPosition = GetNeighbour(graphPosition, "right");
        }
        if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log(graphPosition.GraphX + "_" + graphPosition.GraphY);
            graphPosition = GetNeighbour(graphPosition, "up");
        }
        if (Input.GetKey(KeyCode.S))
        {
            //Debug.Log(graphPosition.GraphX + "_" + graphPosition.GraphY);
            graphPosition = GetNeighbour(graphPosition, "down");
        }
    }
    */
}
