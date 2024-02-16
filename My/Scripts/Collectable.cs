using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public Vector3 posToGo;
    public PuzzleItemData item;

    public void ClickBtn()
    {
        bool wasPickUpTetris = false;
        wasPickUpTetris = TetrisSlot.instanceSlot.addInFirstSpace(item); //add to the bag matrix.
        if (wasPickUpTetris) // took
        {
            gameObject.GetComponent<Button>().interactable = !wasPickUpTetris;
        }
    }
}
