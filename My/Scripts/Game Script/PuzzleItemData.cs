using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzle", menuName = "New Puzzle/Puzzle")]
public class PuzzleItemData : ScriptableObject
{
    public string puzzleID;
    public Sprite puzzleIcon;
    public string puzzleName;
    public string puzzleDesc;
    public int currentStackSize;
    public int maxStackSize;
    public Vector2 puzzleSize;
    public bool isHave = false;


    public float damage;
}
