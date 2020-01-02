using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public void ExecuteCardsShell()
    {
        StartCoroutine(ExecuteAllCards());
    }

    public IEnumerator ExecuteAllCards()
    {

        Debug.Log("Executing All Cards");

        //get player objects
        //currently running the AI turns in an essentially random order. No bueno.
        List<GameObject> players = GameObject.FindGameObjectsWithTag("AI Player").ToList<GameObject>();
        players.Add(GameObject.FindGameObjectWithTag("Player"));

        //get card list for each AI player
        //TODO: remove assumption that there's only one human player (players.count-1)
        List<List<GameObject>> hands = new List<List<GameObject>>();
        for (int i = 0; i < players.Count - 1; i++)
        {
            hands.Add(cardSpawner.SpawnNCards_List(5));
        }

        //get cards for human player, insist all cardslots are full
        GameObject[] cardSlots = GameObject.FindGameObjectsWithTag("CardSlot");
        foreach (GameObject cardSlot in cardSlots)
        {
            if (cardSlot.transform.childCount == 0)
            {
                Debug.Log("PUT CARDS IN ALL THE SLOTS");
                yield break;
            }
        }

        //get cards from cardslots
        List<GameObject> humanCards = new List<GameObject>();
        foreach (var cardSlot in cardSlots)
        {
            humanCards.Add(cardSlot.transform.GetChild(0).gameObject);
        }
        hands.Add(humanCards);

        //ACTUALLY EXECUTING THE CARDS NOW
        //loop over card number
        for (int i = 0; i < 5; i++)
        {
            //loop over player
            for (int j = 0; j < hands.Count; j++)
            {
                GridMovement playerGridMovement = players[j].GetComponent<GridMovement>();
                string action = hands[j][i].GetComponent<Card>().action;

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
        }

        //end of movement checks
        foreach (GameObject player in players)
        {
            player.GetComponent<GridMovement>().CurrentNodeCheck_MovementEnd();
        }

        //reset hand for new turn
        cardSpawner.DestroyAllCards();
        cardSpawner.SpawnNCards(7);
    }
}
