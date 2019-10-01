using System.Collections;
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

        foreach (GameObject cardSlot in cardSlots)
        {
            GameObject card = cardSlot.transform.GetChild(0).gameObject;
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            Debug.Log(cardDisplay.action.text);
            Debug.Log(cardSlot.GetComponent<CardSlot_Properties>().slotNumber);
        }

    }
}
