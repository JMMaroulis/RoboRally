﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteCards : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beepbeep()
    {
        Debug.Log("beepbeep");

        //get cardslots
        GameObject[] cardSlots = GameObject.FindGameObjectsWithTag("CardSlot");

        //get player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GridMovement playerGridMovement = player.GetComponent<GridMovement>();

        //go through cardslots in order, execute card in each carslot
        foreach (GameObject cardSlot in cardSlots)
        {
            GameObject card = cardSlot.transform.GetChild(0).gameObject;
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();

            //todo: move this logic to gridmovement.cs
            Debug.Log(cardDisplay.action.text);
            if (cardDisplay.action.text == "forward" || cardDisplay.action.text == "backward")
            {
                playerGridMovement.MoveDirectionRelative(cardDisplay.action.text);
            }
            if (cardDisplay.action.text == "left" || cardDisplay.action.text == "right")
            {
                playerGridMovement.Turn(cardDisplay.action.text);
            }
            Debug.Log(cardSlot.GetComponent<CardSlot_Properties>().slotNumber);
        }

    }
}