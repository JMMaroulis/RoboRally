using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteCards : MonoBehaviour
{
    public float TimeBetweenCards = 0.5f;
    public GameObject[] cards;
    private SpawnCards cardSpawner;

    // Start is called before the first frame update
    void Start()
    {
       cardSpawner = GameObject.FindObjectOfType<SpawnCards>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //shell exists to allow use of co-routines in a button
    public void ExecutePlayerCardsShell()
    {
        StartCoroutine(ExecuteAICards());
    }

    public IEnumerator ExecutePlayerCards()
    {
        Debug.Log("Executing Player Cards");

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
            Card card = cardSlot.transform.GetChild(0).gameObject.GetComponent<Card>();
            string action = card.action;

            //todo: move this logic to gridmovement.cs
            Debug.Log(action);
            if (action == "forward" || action == "backward")
            {
                playerGridMovement.MoveDirectionRelative(action);
            }
            if (action == "left" || action == "right")
            {
                playerGridMovement.Turn(action);
            }
            Debug.Log(cardSlot.GetComponent<CardSlot_Properties>().slotNumber);
            yield return new WaitForSeconds(TimeBetweenCards);
        }

        //end of movement checks
        playerGridMovement.CurrentNodeCheck_MovementEnd();

        //reset hand for new turn
        cardSpawner.DestroyAllCards();
        cardSpawner.SpawnNCards(7);
    }

    public IEnumerator ExecuteAICards()
    {
        Debug.Log("Executing AI Cards");

        //get player object
        GameObject player = GameObject.FindGameObjectWithTag("AI Player");
        GridMovement playerGridMovement = player.GetComponent<GridMovement>();

        //generate chosen AI cards
        //(this is going to need rewriting sooner rather than later, I imagine. not condusive to weighting cards differently right now)
        List<GameObject> AICards = cardSpawner.SpawnNCards_List(5);
        
        //go through ai cards, execute in order
        foreach (GameObject cardObject in AICards)
        {
            string action = cardObject.GetComponent<Card>().action;

            //todo: move this logic to gridmovement.cs
            Debug.Log(action);
            if (action == "forward" || action == "backward")
            {
                playerGridMovement.MoveDirectionRelative(action);
            }
            if (action == "left" || action == "right")
            {
                playerGridMovement.Turn(action);
            }
            yield return new WaitForSeconds(TimeBetweenCards);
        }

        //end of movement checks
        playerGridMovement.CurrentNodeCheck_MovementEnd();

        //run player cards after the ai cards
        StartCoroutine(ExecutePlayerCards());

    }
}
