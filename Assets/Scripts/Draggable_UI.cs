using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Transform fullScreenCanvas;
    private Transform hand;
    private int previousHandPosition;

    void Start()
    {
        fullScreenCanvas = GameObject.FindGameObjectWithTag("FullScreenCanvas").transform;
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
        if (this.transform.parent.tag == "Hand")
        {
            previousHandPosition = this.transform.GetSiblingIndex();
            Debug.Log(previousHandPosition);
        } 

        this.transform.parent = fullScreenCanvas;

        //disable raycast targeting for all cards
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards)
        {
            card.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //this.transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject potentialParent = eventData.pointerCurrentRaycast.gameObject;

        Debug.Log(potentialParent);

        //default to hand if no new parent found
        if (potentialParent == null)
        {
            this.transform.parent = hand;
            this.transform.SetSiblingIndex(previousHandPosition);
        }

        //put in slot if slot is not full
        else if (potentialParent.tag == "CardSlot" && potentialParent.transform.childCount == 0)
        {  this.transform.parent = potentialParent.transform; }

        //put in appropriate place in hand
        //TODO: cards defaulting to left of hand; would rather they went back where they were
        else if (potentialParent.tag == "Hand")
        {

            //loop over all card in hand, check x coordinates
            float x = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
            int siblingIndex = 0;

            //check if hand is empty
            if ( hand.childCount == 0)
            { this.transform.parent = hand; }

            //far left
            else if (x < hand.GetChild(0).position.x)
            { siblingIndex = 0; }

            //far right
            else if(x < hand.GetChild(hand.childCount-2).position.x)
            { siblingIndex = hand.childCount - 1;}

            //in-between
            for (int i = 0; i < hand.childCount-1; i++)
            {
                float card_left = hand.GetChild(i).position.x;
                float card_right = hand.GetChild(i+1).position.x;

                if (x > card_left && this.transform.position.x < card_right)
                {
                    siblingIndex = i + 1;
                }

            }
            this.transform.parent = potentialParent.transform;
            this.transform.SetSiblingIndex(siblingIndex);
        }

        //default to putting back in hand
        else
        {
            this.transform.parent = hand;
            this.transform.SetSiblingIndex(previousHandPosition);
        }

        //re-enable raycast targeting for all cards
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards)
        {
            card.GetComponent<Image>().raycastTarget = true;
        }

        Debug.Log("end drag");
    }
}
