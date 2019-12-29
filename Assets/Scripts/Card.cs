using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : MonoBehaviour
{
    public string action;

    public void PrintAction()
    {
        Debug.Log(action);
    }
}
