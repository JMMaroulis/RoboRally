using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{

    public Card card;
    public Text action;
    public Image artwork;

    // Start is called before the first frame update
    void Start()
    {
        card.PrintAction();
        action.text = card.action;
        artwork.sprite = card.artwork;

        action.raycastTarget = false;
        artwork.raycastTarget = false;

    }

}
