using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public Vector3 posToGo;
    public PuzzleItemData item;

    private Button myBtn;
    private Image myimage;

    private void Start()
    {
        myBtn = GetComponent<Button>();
        myimage = GetComponent<Image>();
        myimage.sprite = item.puzzleIcon;
    }

    public void ClickBtn()
    {
        bool wasPickUpTetris = false;
        wasPickUpTetris = TetrisSlot.instanceSlot.addInFirstSpace(item); //add to the bag matrix.
        if (wasPickUpTetris) // took
        {
            myBtn.interactable = !wasPickUpTetris;
        }
    }
}
