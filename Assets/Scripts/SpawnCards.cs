using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCards : MonoBehaviour
{

    public GameObject Forward;
    public GameObject Backward;
    public GameObject TurnRight;
    public GameObject TurnLeft;
    private GameObject[] Cards;
    private GameObject Hand;

    // Start is called before the first frame update
    void Start()
    {
        Cards = new GameObject[] {Forward, Backward, TurnLeft, TurnRight };
        Hand = GameObject.FindGameObjectWithTag("Hand");
        SpawnNCards(7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNCards(int numCards)
    {
        int cardCount = 0;
        while (cardCount < numCards)
        {
            int index = Random.Range(0, Cards.Length);
            Instantiate(Cards[index], Hand.transform);
            cardCount += 1;
        }
    }

    public void DestroyAllCards()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards)
        {
            Destroy(card);
        }
    }
}
