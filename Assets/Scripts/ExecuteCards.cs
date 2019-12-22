using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteCards : MonoBehaviour
{
    public float TimeBetweenCards = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beepshell()
    {
        StartCoroutine(beepbeep());
    }

    public IEnumerator beepbeep()
    {
        Debug.Log("beepbeep");

        //get cardslots
        GameObject[] cardSlots = GameObject.FindGameObjectsWithTag("CardSlot");

        //get player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GridMovement playerGridMovement = player.GetComponent<GridMovement>();

        //insist on all cardslots being full
        foreach (GameObject cardSlot in cardSlots)
        {
            if (cardSlot.transform.childCount == 0)
            {
                Debug.Log("PUT CARDS IN ALL THE SLOTS");
                yield break;
            }
        }

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
            yield return new WaitForSeconds(TimeBetweenCards);
        }

        //end of movement checks
        playerGridMovement.CurrentNodeCheck_MovementEnd();

        SpawnCards cardSpawner = GameObject.FindObjectOfType<SpawnCards>();
        cardSpawner.DestroyAllCards();
        cardSpawner.SpawnNCards(7);


    }
}
