using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisUI : MonoBehaviour
{

    TetrisInventory playerInventory;

    [SerializeField]
    GameObject slotPreFab;

    void Start()
    {
        playerInventory = TetrisInventory.instanceTetris;

        for (int i = 0; i < playerInventory.numberSlots; i++)
        {
            var itemUI = Instantiate(slotPreFab, transform);  //generate the slots grid.
        }
    }
}
