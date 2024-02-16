using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisInventory : MonoBehaviour
{
    public static TetrisInventory instanceTetris;

    void Awake()
    {
        if (instanceTetris != null)
        {
            Debug.LogWarning("More than one Tetris inventory");
            return;
        }
        instanceTetris = this;
    }

    public int numberSlots; // starts with one + the number you put.
}
